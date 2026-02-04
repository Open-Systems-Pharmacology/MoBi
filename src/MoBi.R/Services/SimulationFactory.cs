using System;
using System.Collections.Generic;
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
      SimulationCreationResult CreateSimulationFrom(string simulationName, RModuleConfiguration[] moduleConfigurations,
         ExpressionProfileBuildingBlock[] expressionProfiles,
         IndividualBuildingBlock individual,
         bool createAllProcessRateParameters = false,
         SimulationSettings simulationSettings = null);
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

      public SimulationCreationResult CreateSimulationFrom(string simulationName, RModuleConfiguration[] moduleConfigurations,
         ExpressionProfileBuildingBlock[] expressionProfiles,
         IndividualBuildingBlock individual,
         bool createAllProcessRateParameters = false,
         SimulationSettings simulationSettings = null)
      {
         if (string.IsNullOrWhiteSpace(simulationName))
            throw new InvalidOperationException("Simulation name is required");

         if (Constants.ILLEGAL_CHARACTERS.Any(simulationName.Contains))
            throw new InvalidOperationException("Simulation name contains illegal characters");

         var simulationConfiguration = _configurationFactory.Create(simulationSettings ?? _simulationSettingsFactory.CreateDefault());
         simulationConfiguration.CreateAllProcessRateParameters = createAllProcessRateParameters;
         
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
            var (simulation, validationResult) = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, simulationName);
            var messages = validationResult?.Messages ?? Enumerable.Empty<ValidationMessage>();
            var warnings = messages.Where(x => x.NotificationType == NotificationType.Warning).Select(x => x.Text);
            return new SimulationCreationResult(new Simulation(simulation), warnings);
         }
         catch (ValidationFailedMoBiException e)
         {
            return createResultWithMessages(e);
         }
         catch (Exception e)
         {
            return new SimulationCreationResult(null, Enumerable.Empty<string>(), new[] { e.Message });
         }
      }

      private static SimulationCreationResult createResultWithMessages(ValidationFailedMoBiException e)
      {
         var messages = e.ValidationResult?.Messages.ToList() ?? new List<ValidationMessage>();

         var warnings = messages
            .Where(m => m.NotificationType == NotificationType.Warning)
            .Select(m => m.Text);

         var errors = messages
            .Where(m => m.NotificationType == NotificationType.Error)
            .Select(m => m.Text);

         return new SimulationCreationResult(null, warnings, errors);
      }
   }
}