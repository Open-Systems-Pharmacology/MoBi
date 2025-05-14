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
   public class SimulationBuilder
   {
      private readonly IMoBiContext _context;
      private readonly ISimulationConfigurationFactory _configurationFactory;
      private readonly ISimulationFactory _simulationFactory;

      private readonly List<string> _moduleNames = new List<string>();
      private readonly List<string> _expressionProfileNames = new List<string>();
      private string _individualName;
      private string _simulationName;

      public SimulationBuilder(IMoBiContext context, ISimulationConfigurationFactory configurationFactory, ISimulationFactory simulationFactory)
      {
         _context = context;
         _configurationFactory = configurationFactory;
         _simulationFactory = simulationFactory;
      }

      public SimulationBuilder WithModules(IEnumerable<string> moduleNames)
      {
         _moduleNames.AddRange(moduleNames);
         return this;
      }

      public SimulationBuilder WithExpressionProfiles(IEnumerable<string> expressionProfileNames)
      {
         _expressionProfileNames.AddRange(expressionProfileNames);
         return this;
      }

      public SimulationBuilder WithIndividual(string individualName)
      {
         _individualName = individualName;
         return this;
      }

      public SimulationBuilder WithName(string simulationName)
      {
         _simulationName = simulationName;
         return this;
      }

      public IMoBiSimulation Build()
      {
         if (string.IsNullOrWhiteSpace(_simulationName))
            throw new InvalidOperationException("Simulation name is required");

         var simulationConfiguration = _configurationFactory.Create();

         var modules = _moduleNames
            .Select(name => _context.CurrentProject.ModuleByName(name))
            .Where(m => m != null)
            .Select(m => new ModuleConfiguration(m))
            .ToList();
         modules.Each(simulationConfiguration.AddModuleConfiguration);

         foreach (var exprName in _expressionProfileNames)
         {
            var expr = _context.CurrentProject.ExpressionProfileCollection.FirstOrDefault(x => x.Name == exprName);
            if (expr != null)
               simulationConfiguration.AddExpressionProfile(expr);
         }

         if (!string.IsNullOrWhiteSpace(_individualName))
         {
            var individual = _context.CurrentProject.IndividualsCollection.FirstOrDefault(x => x.Name == _individualName);
            if (individual != null)
               simulationConfiguration.Individual = individual;
         }

         simulationConfiguration.ShouldValidate = true;

         var simulation = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, _simulationName);

         if (_context.CurrentProject.Simulations.Any(x => x.Name == simulation.Name))
            throw new InvalidOperationException($"Simulation '{simulation.Name}' already exists.");

         _context.CurrentProject.AddSimulation(simulation);

         return simulation;
      }
   }
}