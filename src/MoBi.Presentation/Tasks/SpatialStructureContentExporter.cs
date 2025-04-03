using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Exchange;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Builder;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Assets;

namespace MoBi.Presentation.Tasks
{
   public interface ISpatialStructureContentExporter
   {
      void SaveWithIndividualAndExpression(IContainer container);
      void Save(NeighborhoodBuilder neighborhoodBuilder);
      void Save(IContainer container);
   }

   public class SpatialStructureContentExporter : ISpatialStructureContentExporter
   {
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IPathAndValueEntityToParameterValueMapper _pathAndValueEntityToParameterValueMapper;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IParameterValueToParameterMapper _individualParameterToParameterMapper;
      private readonly IInteractionTaskContext _interactionTaskContext;
      private readonly IMoBiApplicationController _applicationController;

      public SpatialStructureContentExporter(IMoBiSpatialStructureFactory spatialStructureFactory, 
         ICloneManagerForBuildingBlock cloneManager, 
         IObjectPathFactory objectPathFactory, 
         IPathAndValueEntityToParameterValueMapper pathAndValueEntityToParameterValueMapper, 
         IFormulaFactory formulaFactory, 
         IParameterValueToParameterMapper individualParameterToParameterMapper, IInteractionTaskContext interactionTaskContext, IMoBiApplicationController applicationController)
      {
         _spatialStructureFactory = spatialStructureFactory;
         _cloneManager = cloneManager;
         _objectPathFactory = objectPathFactory;
         _pathAndValueEntityToParameterValueMapper = pathAndValueEntityToParameterValueMapper;
         _formulaFactory = formulaFactory;
         _individualParameterToParameterMapper = individualParameterToParameterMapper;
         _interactionTaskContext = interactionTaskContext;
         _applicationController = applicationController;
      }

      private void addIndividualParametersToContainerAndSubContainers(IndividualBuildingBlock individual, IContainer container)
      {
         if (individual == null || !individual.Any())
            return;

         var allSubContainers = container.GetAllContainersAndSelf<IContainer>();
         allSubContainers.Each(subContainer =>
         {
            var subContainerPath = _objectPathFactory.CreateAbsoluteObjectPath(subContainer);
            addIndividualParametersToContainerByPath(individual, subContainer, new ObjectPath(subContainerPath));
         });
      }

      private void addIndividualParametersToContainerByPath(IndividualBuildingBlock individual, IContainer container, ObjectPath containerPath)
      {
         var individualParametersToExport = individual.Where(individualParameter => individualParameter.ContainerPath.StartsWith(containerPath)).ToList();

         // Ensure that the distributed parameters are added to the container first. After all distributed parameters are added, then the other parameters can be added.
         individualParametersToExport.Where(x => x.IsDistributed()).Each(x => addIndividualParameterToContainerByPath(x, container, containerPath));
         individualParametersToExport.Where(x => !x.IsDistributed()).Each(x => addIndividualParameterToContainerByPath(x, container, containerPath));
      }

      public void Save(NeighborhoodBuilder neighborhoodBuilder)
      {
         var fileName = _interactionTaskContext.InteractionTask.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, neighborhoodBuilder.Name);
         if (fileName.IsNullOrEmpty())
            return;

         var spatialStructure = createTemporarySpatialStructure();
         var clonedNeighborhood = _cloneManager.Clone(neighborhoodBuilder, spatialStructure.FormulaCache);
         spatialStructure.AddNeighborhood(clonedNeighborhood);
         _interactionTaskContext.InteractionTask.Save(spatialStructure, fileName);
      }

      private void exportContainer(IContainer container, string fileName, IndividualBuildingBlock individual = null, IReadOnlyList<ExpressionProfileBuildingBlock> expressionProfileBuildingBlocks = null)
      {
         var tmpSpatialStructure = createTemporarySpatialStructure();

         var clonedContainer = _cloneManager.Clone(container, tmpSpatialStructure.FormulaCache);

         clonedContainer.ParentPath = parentPathFrom(container);

         tmpSpatialStructure.AddTopContainer(clonedContainer);

         var expressionParametersToExport = new List<ExpressionParameter>();
         var initialConditionsToExport = new List<InitialCondition>();

         addIndividualParametersToContainerAndSubContainers(individual, clonedContainer);

         if (expressionProfileBuildingBlocks != null)
         {
            expressionParametersToExport.AddRange(expressionProfileBuildingBlocks.SelectMany(x => pathAndValueEntitiesForContainer(x.ExpressionParameters.ToList(), clonedContainer, x.MoleculeName)));
            initialConditionsToExport.AddRange(expressionProfileBuildingBlocks.SelectMany(x => pathAndValueEntitiesForContainer(x.InitialConditions.ToList(), clonedContainer, x.MoleculeName)));
         }

         var existingSpatialStructure = _interactionTaskContext.Active<MoBiSpatialStructure>();
         if (existingSpatialStructure != null)
         {
            var neighborhoods = existingSpatialStructure.GetConnectingNeighborhoods(new[] { container }, _objectPathFactory);
            neighborhoods.Each(tmpSpatialStructure.AddNeighborhood);
            if (existingSpatialStructure.DiagramModel != null)
               tmpSpatialStructure.DiagramModel = existingSpatialStructure.DiagramModel.CreateCopy(container.Id);
         }

         if (!expressionParametersToExport.Any() && !initialConditionsToExport.Any())
            _interactionTaskContext.InteractionTask.Save(tmpSpatialStructure, fileName);
         else
            exportSpatialStructureTransfer(tmpSpatialStructure, fileName, expressionParametersToExport, initialConditionsToExport, container.Name);
      }

      private MoBiSpatialStructure createTemporarySpatialStructure()
      {
         var tmpSpatialStructure = (MoBiSpatialStructure)_spatialStructureFactory.Create();
         removeEventsContainer(tmpSpatialStructure);
         return tmpSpatialStructure;
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

      public void Save(IContainer container)
      {
         var fileName = _interactionTaskContext.InteractionTask.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, container.Name);
         if (fileName.IsNullOrEmpty())
            return;

         exportContainer(container, fileName);
      }

      private void removeEventsContainer(MoBiSpatialStructure spatialStructure)
      {
         var eventsContainer = spatialStructure.TopContainers.FirstOrDefault(x => x.IsNamed(Constants.EVENTS));
         if (eventsContainer != null)
            spatialStructure.Remove(eventsContainer);
      }

      private void exportSpatialStructureTransfer(MoBiSpatialStructure tmpSpatialStructure, string fileName, IReadOnlyList<ExpressionParameter> expressionParametersToExport, IReadOnlyList<InitialCondition> initialConditionsToExport, string containerName)
      {
         var spatialStructureTransfer = new SpatialStructureTransfer
         {
            ParameterValues = _interactionTaskContext.Context.Create<ParameterValuesBuildingBlock>().WithName(containerName),
            InitialConditions = _interactionTaskContext.Context.Create<InitialConditionsBuildingBlock>().WithName(containerName),
            SpatialStructure = tmpSpatialStructure
         };
         expressionParametersToExport.MapAllUsing(_pathAndValueEntityToParameterValueMapper).Each(x => spatialStructureTransfer.ParameterValues.Add(x));
         initialConditionsToExport.Select(x => _cloneManager.Clone(x, spatialStructureTransfer.InitialConditions.FormulaCache)).Each(x => spatialStructureTransfer.InitialConditions.Add(x));

         _interactionTaskContext.InteractionTask.Save(spatialStructureTransfer, fileName);
      }

      private ObjectPath parentPathFrom(IContainer container)
      {
         return container.ParentPath ?? (container.ParentContainer == null ? new ObjectPath() : _objectPathFactory.CreateAbsoluteObjectPath(container.ParentContainer));
      }

      private IEnumerable<TPathAndValueEntity> pathAndValueEntitiesForContainer<TPathAndValueEntity>(IReadOnlyList<TPathAndValueEntity> buildingBlock, IContainer container, string moleculeName = null) where TPathAndValueEntity : PathAndValueEntity
      {
         if (buildingBlock == null || !buildingBlock.Any())
            return Enumerable.Empty<TPathAndValueEntity>();

         var containerPath = _objectPathFactory.CreateAbsoluteObjectPath(container);
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

      private void addIndividualParameterToContainerByPath(IndividualParameter individualParameter, IContainer container, string containerPath)
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

         // The mapper does not create a formula for the parameter. Caller must clone or create the formula based on how it will be used 
         if (individualParameter.Value.HasValue)
            parameterToAdd.Formula = _formulaFactory.ConstantFormula(individualParameter.Value.Value, individualParameter.Dimension);
         else if (individualParameter.Formula != null)
            parameterToAdd.Formula = _cloneManager.Clone(individualParameter.Formula);

         return parameterToAdd;
      }

      private IContainer findTargetContainer(string individualParameterContainerPath, IContainer container, string containerPath)
      {
         if (individualParameterContainerPath == containerPath)
            return container;

         // if the individualParameterContainerPath is pointing to a sub-container, then search for a child container with the relative path
         var relativePath = individualParameterContainerPath.Substring(containerPath.Length + 1).ToPathArray();
         return container.EntityAt<IContainer>(relativePath);
      }
   }
}
