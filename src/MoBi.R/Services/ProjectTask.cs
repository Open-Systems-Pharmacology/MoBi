using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;

namespace MoBi.R.Services
{
   public interface IProjectTask
   {
      MoBiProject LoadProject(string fileName);
      IReadOnlyList<string> AllModuleNames(MoBiProject moBiProject);
      IReadOnlyList<string> AllExpressionProfileNames(MoBiProject moBiProject);
      IReadOnlyList<string> AllIndividualNames(MoBiProject moBiProject);
      IReadOnlyList<string> AllSimulationNames(MoBiProject moBiProject);
      IReadOnlyList<string> AllBuildingBlocksNamesFromModuleName(MoBiProject moBiProject, string moduleName);
      IReadOnlyList<Simulation> AllSimulations(MoBiProject moBiProject);
      IReadOnlyList<IndividualBuildingBlock> AllIndividuals(MoBiProject moBiProject);
      IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditions(MoBiProject moBiProject);
      IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValues(MoBiProject moBiProject);
      IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditionsFromModule(Module module);
      IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValuesFromModule(Module module);
      Module ModuleByName(MoBiProject moBiProject, string name);
      IndividualBuildingBlock IndividualBuildingBlockByName(MoBiProject moBiProject, string name);
      InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name);
      ParameterValuesBuildingBlock ParameterValueBuildingBlockByName(Module module, string name);
      List<ExpressionProfileBuildingBlock> ExpressionProfileBuildingBlocksByName(MoBiProject moBiProject, params string[] names);
   }

   public class ProjectTask : IProjectTask
   {
      private readonly IMoBiContext _moBiContext;
      private readonly IContextPersistor _contextPersistor;

      public ProjectTask(IMoBiContext moBiContext, IContextPersistor contextPersistor)
      {
         _moBiContext = moBiContext;
         _contextPersistor = contextPersistor;
      }

      public IReadOnlyList<string> AllExpressionProfileNames(MoBiProject moBiProject) =>
         moBiProject.ExpressionProfileCollection.AllNames();

      public List<ExpressionProfileBuildingBlock> ExpressionProfileBuildingBlocksByName(MoBiProject moBiProject, params string[] names) =>
         moBiProject.ExpressionProfileCollection
            .Where(p => names.Contains(p.Name))
            .ToList();
      



      public IReadOnlyList<string> AllModuleNames(MoBiProject moBiProject) =>
         moBiProject.Modules.AllNames();

      public IReadOnlyList<string> AllBuildingBlocksNamesFromModuleName(MoBiProject moBiProject, string moduleName) =>
         moBiProject.ModuleByName(moduleName).BuildingBlocks?.AllNames();

      public MoBiProject LoadProject(string fileName)
      {
         // Load the project from the file 
         _contextPersistor.CloseProject(_moBiContext);
         _contextPersistor.Load(_moBiContext, fileName);
         return _moBiContext.CurrentProject;
      }

      public IReadOnlyList<string> AllSimulationNames(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x => x.Name).ToList();

      public IReadOnlyList<Simulation> AllSimulations(MoBiProject moBiProject) =>
         moBiProject.Simulations.Select(x => new Simulation(x)).ToList();

      public Module ModuleByName(MoBiProject moBiProject, string name) =>
         moBiProject.Modules.FindByName(name);

      public InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name) =>
         module.InitialConditionsCollection.FindByName(name);

      public IndividualBuildingBlock IndividualBuildingBlockByName(MoBiProject moBiProject, string name) =>
         moBiProject.IndividualsCollection.FindByName(name);

      public IReadOnlyList<string> AllIndividualNames(MoBiProject moBiProject) =>
         moBiProject.IndividualsCollection.AllNames();

      public IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditionsFromModule(Module module) =>
         module.InitialConditionsCollection.ToList();

      public ParameterValuesBuildingBlock ParameterValueBuildingBlockByName(Module module, string name) =>
         module.ParameterValuesCollection.FindByName(name);

      public IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValuesFromModule(Module module) =>
         module.ParameterValuesCollection;

      public IReadOnlyList<IndividualBuildingBlock> AllIndividuals(MoBiProject moBiProject) =>
         moBiProject.IndividualsCollection;

      public IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditions(MoBiProject moBiProject) =>
         moBiProject.Modules.SelectMany(x => x.InitialConditionsCollection).ToList();

      public IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValues(MoBiProject moBiProject) =>
         moBiProject.Modules.SelectMany(x => x.ParameterValuesCollection).ToList();
   }
}