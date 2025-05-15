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
using OSPSuite.R.Domain;

namespace MoBi.R.Services
{
   public class SimulationConfiguration
   {
      public string IndividualName { get; set; }
      public List<ModuleConfiguration> ModuleConfigurations { get; set; } = new List<ModuleConfiguration>();
      public List<string> ExpressionProfileNames { get; set; } = new List<string>();
      public string SimulationName { get; set; }
   }
   public class ModuleConfiguration
   {
      public string ModuleName { get; set; }
      public string SelectedParameterValueName { get; set; }
      public string SelectedInitialConditionsName { get; set; }


   }
   public interface IProjectTask
   {
      MoBiProject GetProject(string fileName);
      IReadOnlyList<string> GetModuleNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetExpressionProfileNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetSimulationNames(MoBiProject moBiProject);
      IReadOnlyList<string> GetBuildingBlocksNamesFromModuleName(string moduleName);
      IReadOnlyList<IMoBiSimulation> GetSimulations();
      Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration, MoBiProject moBiProject);
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

      Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration, MoBiProject moBiProject)
      {
         var builder = new Domain.SimulationFactory(_moBiContext, _simulationConfigurationFactory, _simulationFactory);
         
      }
   }
}