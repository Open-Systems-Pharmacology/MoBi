using System;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.R.Domain;
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
      private readonly IMoBiContext _context;
      private readonly ISimulationConfigurationFactory _configurationFactory;
      private readonly Core.Domain.Services.ISimulationFactory _simulationFactory;

      public SimulationFactory(ISimulationConfigurationFactory configurationFactory, Core.Domain.Services.ISimulationFactory simulationFactory, IMoBiContext context)
      {
         _configurationFactory = configurationFactory;
         _simulationFactory = simulationFactory;
         _context = context;
      }

      public Simulation CreateSimulation(SimulationConfiguration configuration)
      {
         if (string.IsNullOrWhiteSpace(configuration.SimulationName))
            throw new InvalidOperationException("Simulation name is required");

         var simulationConfiguration = _configurationFactory.Create();

         configuration.ModuleConfigurations.Each(x =>
         {
            simulationConfiguration.AddModuleConfiguration(
               new ModuleConfiguration(x.Module, x.SelectedInitialCondition, x.SelectedParameterValue));
         });

         configuration.ExpressionProfiles.AddRange(configuration.ExpressionProfiles);

         simulationConfiguration.Individual = configuration.Individual;

         simulationConfiguration.ShouldValidate = true;

         var simulation = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, configuration.SimulationName);

         return new Simulation(simulation);
      }
   }
}