using System;
using System.Linq;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.R.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Extensions;
using CoreModuleConfiguration = OSPSuite.Core.Domain.ModuleConfiguration;
using RModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationFactory
   {
      CreateSimulationResult CreateSimulation(string simulationName, RModuleConfiguration[] moduleConfigurations,
         ExpressionProfileBuildingBlock[] expressionProfiles,
         IndividualBuildingBlock individual);
   }

   public class SimulationFactory : ISimulationFactory
   {
      private readonly ISimulationSettingsFactory _simulationSettingsFactory;
      private readonly ISimulationConfigurationFactory _configurationFactory;
      private readonly Core.Domain.Services.ISimulationFactory _simulationFactory;

      public SimulationFactory(
         ISimulationConfigurationFactory configurationFactory,
         Core.Domain.Services.ISimulationFactory simulationFactory,
         ISimulationSettingsFactory simulationSettingsFactory)
      {
         _configurationFactory = configurationFactory;
         _simulationFactory = simulationFactory;
         _simulationSettingsFactory = simulationSettingsFactory;
      }

      public CreateSimulationResult CreateSimulation(string simulationName, RModuleConfiguration[] moduleConfigurations,
         ExpressionProfileBuildingBlock[] expressionProfiles,
         IndividualBuildingBlock individual)
      {
         if (string.IsNullOrWhiteSpace(simulationName))
            throw new InvalidOperationException("Simulation name is required");

         if (Constants.ILLEGAL_CHARACTERS.Any(simulationName.Contains))
            throw new InvalidOperationException("Simulation name contains illegal characters");

         var simulationSettings = _simulationSettingsFactory.CreateDefault();
         var simulationConfiguration = _configurationFactory.Create(simulationSettings);

         // Handle null parameters by providing empty arrays
         var typedModuleConfigurations = (moduleConfigurations ?? Array.Empty<RModuleConfiguration>()).ToList();

         var typedExpressionProfiles = (expressionProfiles ?? Array.Empty<ExpressionProfileBuildingBlock>()).ToList();

         typedModuleConfigurations.Each(x =>
         {
            simulationConfiguration.AddModuleConfiguration(
               new CoreModuleConfiguration(x.Module, x.SelectedInitialCondition, x.SelectedParameterValue));
         });

         typedExpressionProfiles.Each(simulationConfiguration.AddExpressionProfile);

         simulationConfiguration.Individual = individual;
         simulationConfiguration.ShouldValidate = true;
         try
         {
            var simulationAndCreationResult = _simulationFactory.CreateSimulationAndValidationResult(simulationConfiguration, simulationName);
            var warnings = simulationAndCreationResult.ValidationResult.Messages.Where(x => x.NotificationType == NotificationType.Warning).Select(x => x.Text);
            return new CreateSimulationResult(new Simulation(simulationAndCreationResult.Simulation), warnings);
         }
         catch (ValidationFailedMoBiException e)
         {
            return createResultWithMessages(e);
         }
         catch(Exception e)
         {
            return new CreateSimulationResult(null, Enumerable.Empty<string>(), new[] { e.Message });
         }
      }

      private static CreateSimulationResult createResultWithMessages(ValidationFailedMoBiException e)
      {
         var messages = e.ValidationResult?.Messages ?? Enumerable.Empty<ValidationMessage>();

         var warnings = messages
            .Where(m => m.NotificationType == NotificationType.Warning)
            .Select(m => m.Text)
            .ToList();

         var errors = messages
            .Where(m => m.NotificationType == NotificationType.Error)
            .Select(m => m.Text)
            .ToList();

         return new CreateSimulationResult(null, warnings, errors);
      }
   }
}