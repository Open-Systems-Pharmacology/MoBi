using System;
using System.Linq;
using MoBi.Core.Domain.Model;
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
      Simulation CreateSimulation(SimulationConfiguration configuration, MoBiProject moBiProject);
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

      public Simulation CreateSimulation(SimulationConfiguration configuration, MoBiProject moBiProject)
      {
         if (string.IsNullOrWhiteSpace(configuration.SimulationName))
            throw new InvalidOperationException("Simulation name is required");

         var simulationConfiguration = _configurationFactory.Create();

         configuration.ModuleConfigurations.Each(x =>
         {
            var module = _context.CurrentProject.ModuleByName(x.ModuleName);
            if (module == null)
               throw new InvalidOperationException($"Module {x.ModuleName} not found in the project");

            simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module, module.InitialConditionsCollection.FindByName(x.SelectedInitialConditionsName),
               module.ParameterValuesCollection.FindByName(x.SelectedParameterValueName)));
         });

         configuration.ExpressionProfileNames.Each(x =>
         {
            var expressionProfile = _context.CurrentProject.ExpressionProfileCollection.FirstOrDefault(y => y.Name == x);
            if (expressionProfile == null)
               throw new InvalidOperationException($"Expression profile {x} not found in the project");

            simulationConfiguration.AddExpressionProfile(expressionProfile);
         });

         if (!string.IsNullOrWhiteSpace(configuration.IndividualName))
         {
            var individual = moBiProject.IndividualsCollection.FirstOrDefault(x => x.Name == configuration.IndividualName);
            if (individual == null)
               throw new InvalidOperationException($"Individual {configuration.IndividualName} not found in the project");

            simulationConfiguration.Individual = individual;
         }

         simulationConfiguration.ShouldValidate = true;

         var simulation = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, configuration.SimulationName);
         return new Simulation(simulation);
      }
   }
}