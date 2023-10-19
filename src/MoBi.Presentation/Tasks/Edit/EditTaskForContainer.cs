using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
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
      void SaveWithIndividual(IContainer container);
   }

   public class EditTaskForContainer : EditTaskFor<IContainer>, IEditTaskForContainer
   {
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IIndividualParameterToParameterMapper _individualParameterToParameterMapper;

      public EditTaskForContainer(IInteractionTaskContext interactionTaskContext,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IObjectPathFactory objectPathFactory,
         ICloneManagerForBuildingBlock cloneManager,
         IIndividualParameterToParameterMapper individualParameterToParameterMapper) : base(interactionTaskContext)
      {
         _spatialStructureFactory = spatialStructureFactory;
         _objectPathFactory = objectPathFactory;
         _cloneManager = cloneManager;
         _individualParameterToParameterMapper = individualParameterToParameterMapper;
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

      public void SaveWithIndividual(IContainer container)
      {
         using (var presenter = _applicationController.Start<ISelectFolderAndIndividualFromProjectPresenter>())
         {
            var (filePath, individual) = presenter.GetPathAndIndividualForExport(container);
            if (filePath.IsNullOrEmpty() || individual == null)
               return;

            exportContainer(container, filePath, individual);
         }
      }

      public override void Save(IContainer container)
      {
         var fileName = _interactionTask.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, container.Name);
         if (fileName.IsNullOrEmpty())
            return;

         exportContainer(container, fileName);
      }

      private void exportContainer(IContainer container, string fileName, IndividualBuildingBlock individual = null)
      {
         var tmpSpatialStructure = (MoBiSpatialStructure)_spatialStructureFactory.Create();

         var clonedEntity = _cloneManager.Clone(container, tmpSpatialStructure.FormulaCache);

         clonedEntity.ParentPath = parentPathFrom(container);

         tmpSpatialStructure.AddTopContainer(clonedEntity);

         addIndividualParametersToContainers(individual, clonedEntity);

         var existingSpatialStructure = _interactionTaskContext.Active<MoBiSpatialStructure>();
         if (existingSpatialStructure != null)
         {
            var neighborhoods = existingSpatialStructure.GetConnectingNeighborhoods(new[] { container }, _objectPathFactory);
            neighborhoods.Each(tmpSpatialStructure.AddNeighborhood);
            if (existingSpatialStructure.DiagramModel != null)
               tmpSpatialStructure.DiagramModel = existingSpatialStructure.DiagramModel.CreateCopy(container.Id);
         }

         _interactionTask.Save(tmpSpatialStructure, fileName);
      }

      private ObjectPath parentPathFrom(IContainer container)
      {
         return container.ParentPath ?? (container.ParentContainer == null ? new ObjectPath() : _objectPathFactory.CreateAbsoluteObjectPath(container.ParentContainer));
      }

      private void addIndividualParametersToContainers(IndividualBuildingBlock individual, IContainer entityToSerialize)
      {
         if (individual == null || !individual.Any())
            return;

         var allSubContainers = entityToSerialize.GetAllContainersAndSelf<IContainer>();
         allSubContainers.Each(container => { addIndividualParametersToContainer(individual, container, _objectPathFactory.CreateObjectPathFrom(entityToSerialize.ParentPath.Concat(_objectPathFactory.CreateAbsoluteObjectPath(container)).ToArray())); });
      }

      private void addIndividualParametersToContainer(IndividualBuildingBlock individual, IContainer container, ObjectPath containerPath)
      {
         individual.Where(individualParameter => individualParameter.ContainerPath.Equals(containerPath)).Each(x => addIndividualParameterToContainer(x, container));
      }

      private void addIndividualParameterToContainer(IndividualParameter individualParameter, IContainer container)
      {
         var parameterToAdd = _individualParameterToParameterMapper.MapFrom(individualParameter);
         var existingParameter = container.FindByName(parameterToAdd.Name);

         if (existingParameter != null)
            container.RemoveChild(existingParameter);

         container.Add(parameterToAdd);
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