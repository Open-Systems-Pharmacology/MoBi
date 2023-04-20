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

      IMoBiSimulation CreateSimulationAndValidate(SimulationConfiguration configuration, string name);

      IModel CreateModelAndValidate(string modelName, SimulationConfiguration simulationConfiguration);
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

      public SimulationFactory(IIdGenerator idGenerator,
         ICreationMetaDataFactory creationMetaDataFactory,
         ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater,
         IDiagramManagerFactory diagramManagerFactory,
         ISimulationConfigurationFactory simulationConfigurationFactory,
         IDimensionValidator dimensionValidator,
         IHeavyWorkManager heavyWorkManager,
         IModelConstructor modelConstructor,
         IMoBiContext context)
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
      }

      public IMoBiSimulation CreateFrom(SimulationConfiguration simulationConfiguration, IModel model)
      {
         var moBiSimulation = new MoBiSimulation()
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

      public IModel CreateModelAndValidate(string modelName, SimulationConfiguration simulationConfiguration)
      {
         var results = createModel(simulationConfiguration, modelName);

         if (results == null || results.IsInvalid)
            throw new MoBiException(AppConstants.Exceptions.CouldNotCreateSimulation);

         validateDimensions(results.Model, results.SimulationBuilder);

         return results.Model;
      }

      public IMoBiSimulation CreateSimulationAndValidate(SimulationConfiguration configuration, string name)
      {
         CreationResult result = null;

         _heavyWorkManager.Start(() => { result = createModel(configuration, name); }, AppConstants.Captions.CreatingSimulation);

         if (result == null || result.IsInvalid)
            throw new MoBiException(AppConstants.Exceptions.CouldNotCreateSimulation);

         var simulation = createSimulation(result.Model, configuration, name);

         validateDimensions(simulation.Model, result.SimulationBuilder);

         return simulation;
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

      private IMoBiSimulation createSimulation(IModel model, SimulationConfiguration configuration, string name)
      {
         var simulation = CreateFrom(configuration, model).WithName(name);
         simulation.HasChanged = true;
         return simulation;
      }
   }
}