using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;

namespace MoBi.Core.Domain.Services
{
   public interface ISimulationFactory
   {
      /// <summary>
      ///    Creates and returns a new <see cref="IMoBiSimulation" /> using the <paramref name="simulationConfiguration" /> and
      ///    <paramref name="model" />
      /// </summary>
      IMoBiSimulation CreateFrom(SimulationConfiguration simulationConfiguration, IModel model);

      /// <summary>
      ///    Creates and returns a new <see cref="IMoBiSimulation" />
      /// </summary>
      IMoBiSimulation Create();

      IMoBiSimulation CreateSimulationAndValidate(SimulationConfiguration configurationReferencingBuildingBlocks, string simulationName);

      IModel CreateModelAndValidate(SimulationConfiguration simulationConfiguration, string modelName, string message = AppConstants.Captions.ConfiguringSimulation);
   }

   public class SimulationFactory : ISimulationFactory
   {
      private readonly IIdGenerator _idGenerator;
      private readonly ICreationMetaDataFactory _creationMetaDataFactory;
      private readonly ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;
      private readonly IDiagramManagerFactory _diagramManagerFactory;
      private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;
      private readonly IDimensionValidator _dimensionValidator;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IModelConstructor _modelConstructor;
      private readonly IMoBiContext _context;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public SimulationFactory(IIdGenerator idGenerator,
         ICreationMetaDataFactory creationMetaDataFactory,
         ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater,
         IDiagramManagerFactory diagramManagerFactory,
         ISimulationConfigurationFactory simulationConfigurationFactory,
         IDimensionValidator dimensionValidator,
         IHeavyWorkManager heavyWorkManager,
         IModelConstructor modelConstructor,
         IMoBiContext context,
         ICloneManagerForBuildingBlock cloneManager)
      {
         _idGenerator = idGenerator;
         _creationMetaDataFactory = creationMetaDataFactory;
         _simulationParameterOriginIdUpdater = simulationParameterOriginIdUpdater;
         _diagramManagerFactory = diagramManagerFactory;
         _simulationConfigurationFactory = simulationConfigurationFactory;
         _dimensionValidator = dimensionValidator;
         _heavyWorkManager = heavyWorkManager;
         _modelConstructor = modelConstructor;
         _context = context;
         _cloneManager = cloneManager;
      }

      public IMoBiSimulation CreateFrom(SimulationConfiguration simulationConfiguration, IModel model)
      {
         var moBiSimulation = new MoBiSimulation
         {
            DiagramManager = _diagramManagerFactory.Create<ISimulationDiagramManager>(),
            Configuration = simulationConfiguration,
            Model = model,
            Creation = _creationMetaDataFactory.Create(),
            HasChanged = true,
            Id = _idGenerator.NewId(),
         };

         _simulationParameterOriginIdUpdater.UpdateSimulationId(moBiSimulation);

         return moBiSimulation;
      }

      public IMoBiSimulation Create()
      {
         return CreateFrom(_simulationConfigurationFactory.Create(), null);
      }

      private void validateDimensions(IModel model, SimulationBuilder simulationBuilder)
      {
         _dimensionValidator.Validate(model, simulationBuilder)
            .SecureContinueWith(t => showWarnings(t.Result));
      }

      public IModel CreateModelAndValidate(SimulationConfiguration simulationConfiguration, string modelName, string message = AppConstants.Captions.ConfiguringSimulation)
      {
         CreationResult results = null;

         _heavyWorkManager.Start(() => { results = createModel(simulationConfiguration, modelName); }, message);

         if (results == null || results.IsInvalid)
            throw new MoBiException(AppConstants.Exceptions.CouldNotCreateSimulation);

         validateDimensions(results.Model, results.SimulationBuilder);

         return results.Model;
      }

      public IMoBiSimulation CreateSimulationAndValidate(SimulationConfiguration configurationReferencingBuildingBlocks, string simulationName)
      {
         var model = CreateModelAndValidate(configurationReferencingBuildingBlocks, simulationName, AppConstants.Captions.CreatingSimulation);
         return CreateFrom(cloneOf(configurationReferencingBuildingBlocks), model).WithName(simulationName);
      }

      private SimulationConfiguration cloneOf(SimulationConfiguration configurationReferencingBuildingBlocks)
      {
         return _cloneManager.Clone(configurationReferencingBuildingBlocks);
      }

      private CreationResult createModel(SimulationConfiguration simulationConfiguration, string name)
      {
         var result = _modelConstructor.CreateModelFrom(simulationConfiguration, name);
         if (result == null)
            return null;

         showWarnings(result.ValidationResult);

         return result;
      }

      private void showWarnings(ValidationResult validationResult)
      {
         _context.PublishEvent(new ShowValidationResultsEvent(validationResult));
      }
   }
}