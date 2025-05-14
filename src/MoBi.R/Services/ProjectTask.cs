using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.R.Services
{
   public interface IProjectTask
   {
      MoBiProject GetProject(string fileName);
      IReadOnlyList<string> GetModuleNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetExpressionProfileNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetSimulationNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetBuildingBlocksNamesFromModuleName(string moduleName);
      IReadOnlyList<IMoBiSimulation> GetSimulations();
      string CreateSimulation(IEnumerable<string> moduleNames, IEnumerable<string> expressionProfileNames, string individualName, string simulationName);
   }

   public class ProjectTask : IProjectTask
   {
      private readonly IMoBiContext _moBiContext;
      private readonly IContextPersistor _contextPersistor;
      private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;
      private readonly ISimulationFactory _simulationFactory;
      public ProjectTask(IMoBiContext moBiContext, IContextPersistor contextPersistor,
         ISimulationConfigurationFactory simulationConfigurationFactory, 
         ISimulationFactory simulationFactory)
      {
         _moBiContext = moBiContext;
         _contextPersistor = contextPersistor;
         _simulationConfigurationFactory = simulationConfigurationFactory;
         _simulationFactory = simulationFactory;
      }

      private ExpressionProfileBuildingBlock getExpressionProfileFromProject(MoBiProject moBiProject, string expressionProfileName)
      {
         var expressionProfiles = moBiProject.ExpressionProfileCollection;
         var selectedExpressionProfile = expressionProfiles.FirstOrDefault(x => x.Name.Equals(expressionProfileName));
         if (selectedExpressionProfile != null)
            return selectedExpressionProfile;

         return null;
      }

      public IReadOnlyList<string> GetExpressionProfileNames(MoBiProject moBiProject)
      {
         var expressionProfiles= moBiProject.ExpressionProfileCollection;
         if (expressionProfiles != null)
            return expressionProfiles.AllNames();

         return new List<string>();
      }

      public IReadOnlyList<string> GetModuleNames(MoBiProject moBiProject) =>
         moBiProject.Modules.AllNames();

      public IReadOnlyList<string> GetBuildingBlocksNamesFromModuleName(string moduleName)
      {
         var module = _moBiContext.CurrentProject.ModuleByName(moduleName);
         if (module != null)
            return module.BuildingBlocks.AllNames();

         return new List<string>();
      }

      public MoBiProject GetProject(string fileName)
      {
         _contextPersistor.CloseProject(_moBiContext);
         _contextPersistor.Load(_moBiContext, fileName);
         return _moBiContext.CurrentProject;
      }

      public IReadOnlyList<string> GetSimulationNames(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x => x.Name).ToList();

      public IReadOnlyList<IMoBiSimulation> GetSimulations() =>
         _moBiContext.CurrentProject.Simulations;

      public string CreateSimulation(IEnumerable<string> moduleNames, IEnumerable<string> expressionProfileNames, string individualName, string simulationName)
      {
         var builder = new Domain.SimulationBuilder(_moBiContext, _simulationConfigurationFactory, _simulationFactory);
         var simulation = builder
            .WithModules(moduleNames)
            .WithExpressionProfiles(expressionProfileNames)
            .WithIndividual(individualName)
            .WithName(simulationName)
            .Build();

         return simulation.Name;
      }

      private SimulationConfiguration addIndividualToSimulationConfiguration(string individualName, SimulationConfiguration simulationConfiguration)
      {
         var individual = _moBiContext.CurrentProject.IndividualsCollection.FirstOrDefault(x => x.Name.Equals(individualName));
         if(individual!=null)
            simulationConfiguration.Individual = individual;

         return simulationConfiguration;
      }

      private SimulationConfiguration addExpressionProfilesToSimulationConfiguration(IEnumerable<string> expressionProfileNames, SimulationConfiguration simulationConfiguration)
      {
         var expressionProfileBuildingBlocks = new List<ExpressionProfileBuildingBlock>();
         foreach (var expressionProfileName in expressionProfileNames)
         {
            var expressionProfile = getExpressionProfileFromProject(_moBiContext.CurrentProject, expressionProfileName);
            if(expressionProfile!=null)
               expressionProfileBuildingBlocks.Add(expressionProfile);
         }
         
         expressionProfileBuildingBlocks.Each(simulationConfiguration.AddExpressionProfile);
         return simulationConfiguration;
      }


      private IMoBiSimulation createSimulationFromSimulationConfiguration(SimulationConfiguration simulationConfiguration, string simulationName)
      {
         var simulation = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, simulationName);
         if (_moBiContext.CurrentProject.Simulations.Any(x => x.Name == simulation.Name))
            throw new Exception("Simulation already exists on this project");

         _moBiContext.CurrentProject.AddSimulation(simulation);

         return simulation;
      }

      private SimulationConfiguration createSimulationConfigurationFromModuleNames(IEnumerable<string> moduleNames)
      {
         var simulationConfiguration = _simulationConfigurationFactory.Create();
         var moduleConfigurationDTOs = createModuleConfigurationFromModuleNames(moduleNames);
         moduleConfigurationDTOs.Each(simulationConfiguration.AddModuleConfiguration);
         return simulationConfiguration;
      }

      private List<ModuleConfiguration> createModuleConfigurationFromModuleNames(IEnumerable<string> moduleNames)
      {
         var modules = moduleNames
            .Select(name => _moBiContext.CurrentProject.ModuleByName(name))
            .Where(module => module != null)
            .ToList();

         if (!modules.Any())
            throw new ArgumentException("No valid modules found for given names");

         var moduleConfigurationDTOs = modules
            .Select(module => new ModuleConfiguration(module))
            .ToList();

         return moduleConfigurationDTOs;
      }
   }
}