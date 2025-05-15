using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace MoBi.R.Domain
{
   public class SimulationFactory
   {
      private readonly IMoBiContext _context;
      private readonly ISimulationConfigurationFactory _configurationFactory;
      private readonly ISimulationFactory _simulationFactory;

      public SimulationFactory(ISimulationConfigurationFactory configurationFactory, ISimulationFactory simulationFactory)
      {
         _configurationFactory = configurationFactory;
         _simulationFactory = simulationFactory;
      }

      public OSPSuite.R.Domain.Simulation CreateSimulation(Services.SimulationConfiguration configuration, MoBiProject moBiProject)
      {
         if (string.IsNullOrWhiteSpace(configuration.SimulationName))
            throw new InvalidOperationException("Simulation name is required");

         var simulationConfiguration = _configurationFactory.Create();
         //What happens when not found? 
         configuration.ModuleConfigurations.Each(x =>
         {
            var module = _context.CurrentProject.ModuleByName(x.ModuleName);
            simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module, module.InitialConditionsCollection.FindByName(x.SelectedInitialConditionsName), 
               module.ParameterValuesCollection.FindByName(x.SelectedParameterValueName)));
         });

         configuration.ExpressionProfileNames.Each(x =>
         {
            var expressionProfile = _context.CurrentProject.ExpressionProfileCollection.Single(y => y.Name == x);
            simulationConfiguration.AddExpressionProfile(expressionProfile);
         });

         if (!string.IsNullOrWhiteSpace(configuration.IndividualName))
         {
            var individual = moBiProject.IndividualsCollection.FirstOrDefault(x => x.Name == configuration.IndividualName);
            if (individual != null)
               simulationConfiguration.Individual = individual;
         }

         simulationConfiguration.ShouldValidate = true;

         var simulation = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, configuration.SimulationName);

         return simulation.;
      }
   }
}