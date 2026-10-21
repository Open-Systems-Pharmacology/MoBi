using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
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

      /// <summary>
      ///    Creates and validates a new simulation using the <paramref name="validationOptions" />, which are then stored in
      ///    the simulation. Options that are not set, or all options when <paramref name="validationOptions" /> is
      ///    <c>null</c>, fall back to the user settings
      /// </summary>
      SimulationAndValidationResult CreateSimulationAndValidate(SimulationConfiguration configurationReferencingBuildingBlocks, string simulationName, SimulationValidationOptions validationOptions = null);

      /// <summary>
      ///    Creates the model from <paramref name="simulationConfiguration" /> and validates its dimensions. With
      ///    <paramref name="throwOnInvalid" /> <c>true</c> (the default) an invalid model throws
      ///    <see cref="ValidationFailedMoBiException" />; pass <c>false</c> to get the result back instead — always
      ///    non-null, with <c>IsInvalid</c> set when the model could not be created.
      ///    The model is created and validated using the <paramref name="validationOptions" /> of the simulation. Options
      ///    that are not set, or all options when <paramref name="validationOptions" /> is <c>null</c>, fall back to the
      ///    user settings
      /// </summary>
      CreationResult CreateModelAndValidate(SimulationConfiguration simulationConfiguration, string modelName, bool throwOnInvalid = true, SimulationValidationOptions validationOptions = null);
   }

   public class SimulationFactory : ISimulationFactory
   {
      private readonly IIdGenerator _idGenerator;
      private readonly ICreationMetaDataFactory _creationMetaDataFactory;
      private readonly ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;
      private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;
      private readonly IDimensionValidator _dimensionValidator;
      private readonly IModelConstructor _modelConstructor;
      private readonly IMoBiContext _context;
      private readonly ICoreUserSettings _userSettings;

      public SimulationFactory(IIdGenerator idGenerator,
         ICreationMetaDataFactory creationMetaDataFactory,
         ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater,
         ISimulationConfigurationFactory simulationConfigurationFactory,
         IDimensionValidator dimensionValidator,
         IModelConstructor modelConstructor,
         IMoBiContext context,
         ICoreUserSettings userSettings)
      {
         _idGenerator = idGenerator;
         _creationMetaDataFactory = creationMetaDataFactory;
         _simulationParameterOriginIdUpdater = simulationParameterOriginIdUpdater;
         _simulationConfigurationFactory = simulationConfigurationFactory;
         _dimensionValidator = dimensionValidator;
         _modelConstructor = modelConstructor;
         _context = context;
         _userSettings = userSettings;
      }

      public IMoBiSimulation CreateFrom(SimulationConfiguration simulationConfiguration, IModel model, IEnumerable<SimulationEntitySource> entitySources)
      {
         var moBiSimulation = new MoBiSimulation
         {
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

      private void validateDimensions(IModel model, SimulationBuilder simulationBuilder, ValidationSettings validationSettings)
      {
         _dimensionValidator.Validate(model, simulationBuilder, validationSettings)
            .SecureContinueWith(t => showWarnings(t.Result, validationSettings));
      }

      public CreationResult CreateModelAndValidate(SimulationConfiguration simulationConfiguration, string modelName, bool throwOnInvalid = true, SimulationValidationOptions validationOptions = null)
      {
         var options = validationOptions ?? new SimulationValidationOptions();
         var validationSettings = options.ValidationSettingsWith(_userSettings);
         simulationConfiguration.PerformCircularReferenceCheck = validationSettings.CheckCircularReference;
         simulationConfiguration.WarnForNonFiniteQuantities = options.WarnForNonFiniteQuantitiesWith(_userSettings);

         var results = createModel(simulationConfiguration, modelName, validationSettings);

         if (results.IsInvalid)
         {
            if (throwOnInvalid)
               throw new ValidationFailedMoBiException(AppConstants.Exceptions.CouldNotCreateSimulation, results.ValidationResult);

            return results;
         }

         validateDimensions(results.Model, results.SimulationBuilder, validationSettings);

         return results;
      }

      public SimulationAndValidationResult CreateSimulationAndValidate(SimulationConfiguration configurationReferencingBuildingBlocks, string simulationName, SimulationValidationOptions validationOptions = null)
      {
         var options = validationOptions ?? new SimulationValidationOptions();
         var results = CreateModelAndValidate(configurationReferencingBuildingBlocks, simulationName, validationOptions: options);
         var cloneManager = _context.Resolve<ICloneManagerForBuildingBlock>();
         var clonedConfiguration = cloneManager.Clone(configurationReferencingBuildingBlocks);
         var simulation = CreateFrom(clonedConfiguration, results.Model, results.SimulationBuilder.EntitySources).WithName(simulationName);
         simulation.ValidationOptions = options.Clone();
         return new SimulationAndValidationResult(simulation, results.ValidationResult);
      }

      private CreationResult createModel(SimulationConfiguration simulationConfiguration, string name, ValidationSettings validationSettings)
      {
         //CreateModelFrom always returns a result - an invalid one carrying the validation messages when the build fails - never null
         var result = _modelConstructor.CreateModelFrom(simulationConfiguration, name);
         showWarnings(result.ValidationResult, validationSettings);
         return result;
      }

      private void showWarnings(ValidationResult validationResult, ValidationSettings validationSettings)
      {
         _context.PublishEvent(new ShowValidationResultsEvent(validationResult, validationSettings));
      }
   }
}