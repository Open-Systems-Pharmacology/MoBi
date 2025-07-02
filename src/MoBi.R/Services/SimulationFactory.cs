using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Services;
using MoBi.R.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Extensions;
using ModuleConfiguration = OSPSuite.Core.Domain.ModuleConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationFactory
   {
      Simulation CreateSimulation(SimulationConfiguration configuration);
   }

   public class SimulationFactory : ISimulationFactory
   {
      private readonly ISimulationConfigurationFactory _configurationFactory;
      private readonly Core.Domain.Services.ISimulationFactory _simulationFactory;
      private readonly IForbiddenNamesRetriever _forbiddenNamesRetriever;
      private readonly List<string> _usedNames = new List<string>();

      public SimulationFactory(ISimulationConfigurationFactory configurationFactory, Core.Domain.Services.ISimulationFactory simulationFactory,
         IForbiddenNamesRetriever forbiddenNamesRetriever)
      {
         _configurationFactory = configurationFactory;
         _simulationFactory = simulationFactory;
         _forbiddenNamesRetriever = forbiddenNamesRetriever;
      }

      public Simulation CreateSimulation(SimulationConfiguration configuration)
      {
         if (string.IsNullOrWhiteSpace(configuration.SimulationName))
            throw new InvalidOperationException("Simulation name is required");

         if (Constants.ILLEGAL_CHARACTERS.Any(configuration.SimulationName.Contains))
            throw new InvalidOperationException("Simulation name contains illegal characters");

         var simulationConfiguration = _configurationFactory.Create();

         configuration.ModuleConfigurations.Each(x =>
         {
            simulationConfiguration.AddModuleConfiguration(
               new ModuleConfiguration(x.Module, x.SelectedInitialCondition, x.SelectedParameterValue));
         });

         configuration.ExpressionProfiles.Each(simulationConfiguration.AddExpressionProfile);

         simulationConfiguration.Individual = configuration.Individual;

         simulationConfiguration.ShouldValidate = true;

         var simulation = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, configuration.SimulationName);

         if (_forbiddenNamesRetriever.For(simulation).Contains(configuration.SimulationName))
            throw new InvalidOperationException("Simulation name is forbidden");

         return new Simulation(simulation);
      }
   }
}