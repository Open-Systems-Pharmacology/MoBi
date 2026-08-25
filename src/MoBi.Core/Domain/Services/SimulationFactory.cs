using System.Collections.Generic;
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
      ///    Creates and returns a new <see cref="IMoBiSimulation" /> using the <paramref name="simulationConfiguration" />
      ///    <paramref name="model" /> and <paramref name="entitySources" />
      /// </summary>
      IMoBiSimulation CreateFrom(SimulationConfiguration simulationConfiguration, IModel model, IEnumerable<SimulationEntitySource> entitySources);

      /// <summary>
      ///    Creates and returns a new <see cref="IMoBiSimulation" />
      /// </summary>
      IMoBiSimulation Create();

      SimulationAndValidationResult CreateSimulationAndValidate(SimulationConfiguration configurationReferencingBuildingBlocks, string simulationName);

      /// <summary>
      ///    Creates the model from <paramref name="simulationConfiguration" /> and validates its dimensions. With
      ///    <paramref name="throwOnInvalid" /> <c>true</c> (the default) an invalid model throws
      ///    <see cref="ValidationFailedMoBiException" />; pass <c>false</c> to get the result back instead — always
      ///    non-null, with <c>IsInvalid</c> set when the model could not be created.
      /// </summary>
      CreationResult CreateModelAndValidate(SimulationConfiguration simulationConfiguration, string modelName, bool throwOnInvalid = true);
   }

   public class SimulationFactory : ISimulationFactory
   {
      private readonly IIdGenerator _idGenerator;
      private readonly ICreationMetaDataFactory _creationMetaDataFactory;
      private readonly ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;
      private readonly IDiagramManagerFactory _diagramManagerFactory;
      private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;
      private readonly IDimensionValidator _dimensionValidator;
      private readonly IModelConstructor _modelConstructor;
      private readonly IMoBiContext _context;

      public SimulationFactory(IIdGenerator idGenerator,
         ICreationMetaDataFactory creationMetaDataFactory,
         ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater,
         IDiagramManagerFactory diagramManagerFactory,
         ISimulationConfigurationFactory simulationConfigurationFactory,
         IDimensionValidator dimensionValidator,
         IModelConstructor modelConstructor,
         IMoBiContext context)
      {
         _idGenerator = idGenerator;
         _creationMetaDataFactory = creationMetaDataFactory;
         _simulationParameterOriginIdUpdater = simulationParameterOriginIdUpdater;
         _diagramManagerFactory = diagramManagerFactory;
         _simulationConfigurationFactory = simulationConfigurationFactory;
         _dimensionValidator = dimensionValidator;
         _modelConstructor = modelConstructor;
         _context = context;
      }

      public IMoBiSimulation CreateFrom(SimulationConfiguration simulationConfiguration, IModel model, IEnumerable<SimulationEntitySource> entitySources)
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

         moBiSimulation.AddEntitySources(entitySources);

         _simulationParameterOriginIdUpdater.UpdateSimulationId(moBiSimulation);

         return moBiSimulation;
      }

      public IMoBiSimulation Create()
      {
         return CreateFrom(_simulationConfigurationFactory.Create(), null, null);
      }

      private void validateDimensions(IModel model, SimulationBuilder simulationBuilder)
      {
         _dimensionValidator.Validate(model, simulationBuilder)
            .SecureContinueWith(t => showWarnings(t.Result));
      }

      public CreationResult CreateModelAndValidate(SimulationConfiguration simulationConfiguration, string modelName, bool throwOnInvalid = true)
      {
         var results = createModel(simulationConfiguration, modelName);

         if (results.IsInvalid)
         {
            if (throwOnInvalid)
               throw new ValidationFailedMoBiException(AppConstants.Exceptions.CouldNotCreateSimulation, results.ValidationResult);

            return results;
         }

         validateDimensions(results.Model, results.SimulationBuilder);

         return results;
      }

      public SimulationAndValidationResult CreateSimulationAndValidate(SimulationConfiguration configurationReferencingBuildingBlocks, string simulationName)
      {
         var results = CreateModelAndValidate(configurationReferencingBuildingBlocks, simulationName);
         //resolved per call: the clone manager holds per-operation state and this method may run on parallel workers
         var cloneManager = _context.Resolve<ICloneManagerForBuildingBlock>();
         var clonedConfiguration = cloneManager.Clone(configurationReferencingBuildingBlocks);
         var simulation = CreateFrom(clonedConfiguration, results.Model, results.SimulationBuilder.EntitySources).WithName(simulationName);
         return new SimulationAndValidationResult(simulation, results.ValidationResult);
      }

      private CreationResult createModel(SimulationConfiguration simulationConfiguration, string name)
      {
         //CreateModelFrom always returns a result - an invalid one carrying the validation messages when the build fails - never null
         var result = _modelConstructor.CreateModelFrom(simulationConfiguration, name);
         showWarnings(result.ValidationResult);
         return result;
      }

      private void showWarnings(ValidationResult validationResult)
      {
         _context.PublishEvent(new ShowValidationResultsEvent(validationResult));
      }
   }
}