using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Exchange;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTaskForContainer : IEditTaskFor<IContainer>
   {
      IMoBiCommand SetContainerMode(IBuildingBlock buildingBlock, IContainer container, ContainerMode containerMode);

      /// <summary>
      ///    Opens a dialog for the user to select file path
      /// </summary>
      /// <returns>The path if dialog is dismissed with ok, empty string if canceled</returns>
      string BrowseSavePathFor(string name);

      /// <summary>
      ///    The user can select an individual to combine with the container and export combined parameters.
      ///    The individual parameters are exported if they match the path of the <paramref name="container" /> tree
      /// </summary>
      void SaveWithIndividualAndExpression(IContainer container);
   }

   public class EditTaskForContainer : EditTaskFor<IContainer>, IEditTaskForContainer
   {
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IPathAndValueEntityToParameterValueMapper _pathAndValueEntityToParameterValueMapper;

      public EditTaskForContainer(IInteractionTaskContext interactionTaskContext,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IObjectPathFactory objectPathFactory,
         ICloneManagerForBuildingBlock cloneManager,
         IPathAndValueEntityToParameterValueMapper pathAndValueEntityToParameterValueMapper) : base(interactionTaskContext)
      {
         _spatialStructureFactory = spatialStructureFactory;
         _objectPathFactory = objectPathFactory;
         _cloneManager = cloneManager;
         _pathAndValueEntityToParameterValueMapper = pathAndValueEntityToParameterValueMapper;
      }

      protected override IEnumerable<string> GetUnallowedNames(IContainer container, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         if (existingObjectsInParent != null)
            return existingObjectsInParent.AllNames();

         var spatialStructure = _interactionTaskContext.Active<SpatialStructure>();
         if (spatialStructure == null)
            return Enumerable.Empty<string>();

         return spatialStructure.TopContainers.Select(x => x.Name).Union(AppConstants.UnallowedNames);
      }

      public void SaveWithIndividualAndExpression(IContainer container)
      {
         using (var presenter = _applicationController.Start<ISelectFolderAndIndividualAndExpressionFromProjectPresenter>())
         {
            var (filePath, individual, expressionProfiles) = presenter.GetPathIndividualAndExpressionsForExport(container);
            if (filePath.IsNullOrEmpty())
               return;

            exportContainer(container, filePath, individual, expressionProfiles);
         }
      }

      public override void Save(IContainer container)
      {
         var fileName = _interactionTask.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, container.Name);
         if (fileName.IsNullOrEmpty())
            return;

         exportContainer(container, fileName);
      }

      private void exportContainer(IContainer container, string fileName, IndividualBuildingBlock individual = null, IReadOnlyList<ExpressionProfileBuildingBlock> expressionProfileBuildingBlocks = null)
      {
         var tmpSpatialStructure = (MoBiSpatialStructure)_spatialStructureFactory.Create();

         var clonedEntity = _cloneManager.Clone(container, tmpSpatialStructure.FormulaCache);

         clonedEntity.ParentPath = parentPathFrom(container);

         tmpSpatialStructure.AddTopContainer(clonedEntity);

         var entitiesToExport = new List<PathAndValueEntity>();

         entitiesToExport.AddRange(pathAndValueEntitiesForContainer(individual, clonedEntity));

         if(expressionProfileBuildingBlocks != null)
            entitiesToExport.AddRange(expressionProfileBuildingBlocks.SelectMany(x => pathAndValueEntitiesForContainer(x, clonedEntity, x.MoleculeName)));

         var existingSpatialStructure = _interactionTaskContext.Active<MoBiSpatialStructure>();
         if (existingSpatialStructure != null)
         {
            var neighborhoods = existingSpatialStructure.GetConnectingNeighborhoods(new[] { container }, _objectPathFactory);
            neighborhoods.Each(tmpSpatialStructure.AddNeighborhood);
            if (existingSpatialStructure.DiagramModel != null)
               tmpSpatialStructure.DiagramModel = existingSpatialStructure.DiagramModel.CreateCopy(container.Id);
         }

         if(!entitiesToExport.Any())
            _interactionTask.Save(tmpSpatialStructure, fileName);
         else
            exportSpatialStructureTransfer(tmpSpatialStructure, fileName, entitiesToExport.MapAllUsing(_pathAndValueEntityToParameterValueMapper), container.Name);
      }

      private void exportSpatialStructureTransfer(MoBiSpatialStructure tmpSpatialStructure, string fileName, IEnumerable<ParameterValue> entitiesToExport, string containerName)
      {
         var spatialStructureTransfer = new SpatialStructureTransfer
         {
            ParameterValues = _interactionTaskContext.Context.Create<ParameterValuesBuildingBlock>().WithName(containerName),
            SpatialStructure = tmpSpatialStructure
         };
         entitiesToExport.Each(x => spatialStructureTransfer.ParameterValues.Add(x));
         _interactionTask.Save(spatialStructureTransfer, fileName);
      }

      private ObjectPath parentPathFrom(IContainer container)
      {
         return container.ParentPath ?? (container.ParentContainer == null ? new ObjectPath() : _objectPathFactory.CreateAbsoluteObjectPath(container.ParentContainer));
      }

      private IEnumerable<TPathAndValueEntity> pathAndValueEntitiesForContainer<TPathAndValueEntity>(PathAndValueEntityBuildingBlock<TPathAndValueEntity> buildingBlock, IContainer container, string moleculeName = null) where TPathAndValueEntity : PathAndValueEntity
      {
         if (buildingBlock == null || !buildingBlock.Any())
            return Enumerable.Empty<TPathAndValueEntity>();

         var containerPath = new ObjectPath(container.ParentPath, _objectPathFactory.CreateAbsoluteObjectPath(container));
         var parametersToExport = buildingBlock.Where(x => x.ContainerPath.StartsWith(containerPath)).ToList();

         // export parameter values for container and its existing sub-containers
         var containerParameters = parametersToExport.Where(x => containerHasSubContainerFor(containerPathWithoutMoleculeName(x, moleculeName), container, containerPath)).ToList();

         // find parameters that support distributed parameters
         var supportingParameters = parametersToExport.Where(x => !containerParameters.Contains(x) && containerParameters.Any(containerParameter => containerParameter.Path.Equals(x.ContainerPath)));

         return containerParameters.Concat(supportingParameters);
      }

      private ObjectPath containerPathWithoutMoleculeName<TPathAndValueEntity>(TPathAndValueEntity pathAndValueEntity, string moleculeName) where TPathAndValueEntity : PathAndValueEntity
      {
         var modifiedPath = new ObjectPath(pathAndValueEntity.ContainerPath);
         modifiedPath.Remove(moleculeName);

         return modifiedPath;
      }

      private bool containerHasSubContainerFor(ObjectPath individualParameterContainerPath, IContainer container, ObjectPath containerPath)
      {
         if (individualParameterContainerPath.Equals(containerPath))
            return true;

         // if the individualParameterContainerPath is pointing to a sub-container, then search for a child container with the relative path
         var relativePath = individualParameterContainerPath.PathAsString.Substring(containerPath.PathAsString.Length + 1).ToPathArray();
         return container.EntityAt<IContainer>(relativePath) != null;
      }

      public IMoBiCommand SetContainerMode(IBuildingBlock buildingBlock, IContainer container, ContainerMode containerMode)
      {
         return new SetContainerModeCommand(buildingBlock, container, containerMode).Run(_context);
      }

      public string BrowseSavePathFor(string name)
      {
         return _interactionTask.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, name);
      }

      protected override IMoBiCommand GetRenameCommandFor(IContainer container, IBuildingBlock buildingBlock, string newName, string objectType)
      {
         //when renaming a container in a spatial structure, we need to ensure that we are also renaming the path in the neighborhood
         if (buildingBlock.IsAnImplementationOf<SpatialStructure>() && containerCanBePartOfNeighborhoodPath(container))
            return new RenameContainerCommand(container, newName, buildingBlock.DowncastTo<SpatialStructure>());

         return base.GetRenameCommandFor(container, buildingBlock, newName, objectType);
      }

      private bool containerCanBePartOfNeighborhoodPath(IContainer container)
      {
         return !(container.IsAnImplementationOf<NeighborhoodBuilder>() ||
                  container.IsAnImplementationOf<IParameter>() ||
                  container.IsNamed(Constants.MOLECULE_PROPERTIES));
      }
   }
}