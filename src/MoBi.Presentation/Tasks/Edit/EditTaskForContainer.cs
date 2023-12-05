using System.Collections.Generic;
using System.Globalization;
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
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
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
      void SaveWithIndividual(IContainer container);
   }

   public class EditTaskForContainer : EditTaskFor<IContainer>, IEditTaskForContainer
   {
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IIndividualParameterToParameterMapper _individualParameterToParameterMapper;
      private readonly IObjectBaseFactory _objectBaseFactory;

      public EditTaskForContainer(IInteractionTaskContext interactionTaskContext,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IObjectPathFactory objectPathFactory,
         ICloneManagerForBuildingBlock cloneManager,
         IIndividualParameterToParameterMapper individualParameterToParameterMapper,
         IObjectBaseFactory objectBaseFactory) : base(interactionTaskContext)
      {
         _spatialStructureFactory = spatialStructureFactory;
         _objectPathFactory = objectPathFactory;
         _cloneManager = cloneManager;
         _individualParameterToParameterMapper = individualParameterToParameterMapper;
         _objectBaseFactory = objectBaseFactory;
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

      private void addIndividualParametersToContainers(IndividualBuildingBlock individual, IContainer container)
      {
         if (individual == null || !individual.Any())
            return;

         var allSubContainers = container.GetAllContainersAndSelf<IContainer>();
         allSubContainers.Each(subContainer =>
         {
            var subContainerPath = container.ParentPath.Concat(_objectPathFactory.CreateAbsoluteObjectPath(subContainer));
            addIndividualParametersToContainer(individual, subContainer, new ObjectPath(subContainerPath));
         });
      }

      private void addIndividualParametersToContainer(IndividualBuildingBlock individual, IContainer container, ObjectPath containerPath)
      {
         var individualParametersToExport = individual.Where(individualParameter => individualParameter.ContainerPath.StartsWith(containerPath)).ToList();

         // Ensure that the distributed parameters are added to the container first. After all distributed parameters are added, then the other parameters can be added.
         individualParametersToExport.Where(x => x.DistributionType != null).Each(x => addIndividualParameterToContainer(x, container, containerPath));
         individualParametersToExport.Where(x => x.DistributionType == null).Each(x => addIndividualParameterToContainer(x, container, containerPath));
      }

      private void addIndividualParameterToContainer(IndividualParameter individualParameter, IContainer container, string containerPath)
      {
         var targetContainer = findTargetContainer(individualParameter.ContainerPath, container, containerPath);
         // The target container for this parameter is not found - skip
         if (targetContainer == null)
            return;

         var parameterToAdd = createParameter(individualParameter);
         var existingParameter = targetContainer.FindByName(parameterToAdd.Name);

         if (existingParameter != null)
            targetContainer.RemoveChild(existingParameter);

         targetContainer.Add(parameterToAdd);
      }

      private IParameter createParameter(IndividualParameter individualParameter)
      {
         var parameterToAdd = _individualParameterToParameterMapper.MapFrom(individualParameter);

         if (parameterToAdd.Name.Equals(Constants.Distribution.PERCENTILE) && individualParameter.Value.HasValue)
            setValuesForPercentile(parameterToAdd, individualParameter.Value.Value, individualParameter.Dimension);
         else
            setValues(individualParameter, parameterToAdd);

         // NaN formulas do not need to be serialized
         if (parameterToAdd.Formula is ConstantFormula constantFormula && double.IsNaN(constantFormula.Value)) 
            parameterToAdd.Formula = null;

         return parameterToAdd;
      }

      private static void setValues(IndividualParameter individualParameter, IParameter parameterToAdd)
      {
         if (individualParameter.Formula != null)
            parameterToAdd.Formula = individualParameter.Formula;
         if (individualParameter.Value.HasValue)
            parameterToAdd.Value = individualParameter.Value.Value;
      }

      private void setValuesForPercentile(IParameter parameterToAdd, double value, IDimension dimension)
      {
         // When setting the values for a percentile parameter within a distribution, the IsFixedValue property must be false.
         // The value of the parameter should be set using an explicit formula rather than by setting the value directly.
         // A constant formula will not be enough because during export constant formula will be removed.
         parameterToAdd.Value = double.NaN;
         parameterToAdd.IsFixedValue = false;
         // TODO name this formula
         var explicitFormula = _objectBaseFactory.Create<ExplicitFormula>();
         explicitFormula.FormulaString = value.ToString(CultureInfo.InvariantCulture);
         explicitFormula.Dimension = dimension;
         parameterToAdd.Formula = explicitFormula;
      }

      private IContainer findTargetContainer(string individualParameterContainerPath, IContainer container, string containerPath)
      {
         if(individualParameterContainerPath == containerPath)
            return container;
         
         // if the individualParameterContainerPath is pointing to a sub-container, then search for a child container with the relative path
         var relativePath = individualParameterContainerPath.Substring(containerPath.Length + 1).ToPathArray();
         return container.EntityAt<IContainer>(relativePath);
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