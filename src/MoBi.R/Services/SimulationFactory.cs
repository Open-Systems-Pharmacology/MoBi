using System;
using System.Linq;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Services;
using MoBi.R.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using RModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;
using CoreModuleConfiguration = OSPSuite.Core.Domain.ModuleConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationFactory
   {
      MoBiSimulation CreateSimulation(string simulationName, object[] moduleConfigurations,
         object[] expressionProfiles,
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

      public MoBiSimulation CreateSimulation(string simulationName, object[] moduleConfigurations,
         object[] expressionProfiles,
         IndividualBuildingBlock individual)
      {
         if (string.IsNullOrWhiteSpace(simulationName))
            throw new InvalidOperationException("Simulation name is required");

         if (Constants.ILLEGAL_CHARACTERS.Any(simulationName.Contains))
            throw new InvalidOperationException("Simulation name contains illegal characters");

         var simulationSettings = _simulationSettingsFactory.CreateDefault();
         var simulationConfiguration = _configurationFactory.Create(simulationSettings);

         // Convert object[] to strongly typed lists
         var typedModuleConfigurations = (moduleConfigurations ?? Array.Empty<object>()).OfType<RModuleConfiguration>().ToList();

         var typedExpressionProfiles = (expressionProfiles ?? Array.Empty<object>()).OfType<ExpressionProfileBuildingBlock>().ToList();

         typedModuleConfigurations.Each(x =>
         {
            simulationConfiguration.AddModuleConfiguration(
               new CoreModuleConfiguration(x.Module, x.SelectedInitialCondition, x.SelectedParameterValue));
         });

         typedExpressionProfiles.Each(simulationConfiguration.AddExpressionProfile);

         simulationConfiguration.Individual = individual;
         simulationConfiguration.ShouldValidate = true;

         var simulation = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, simulationName);

         return new MoBiSimulation(simulation);
      }
   }
}