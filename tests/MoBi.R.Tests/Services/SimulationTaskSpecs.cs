using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.R.Domain;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;

namespace MoBi.R.Tests.Services
{
   internal abstract class concern_for_SimulationCreateTask : ContextForIntegration<ISimulationTask>
   {
      protected MoBiProject _project;
      protected IProjectTask _projectTask;
      protected IModuleTask _moduleTask;
      protected string _simulationName = "Sim1";

      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = Api.GetSimulationTask();
         _projectTask = Api.GetProjectTask();
         _moduleTask = Api.GetModuleTask();
      }

      protected void LoadSampleProject(string fileName)
      {
         var projectFile = DomainHelperForSpecs.DataTestFileFullPath(fileName);
         _project = _projectTask.LoadProject(projectFile);
      }

      protected Simulation CreateSimulationFromModule(Module module)
      {
         var individual = _projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)");
         var expressionProfiles = _projectTask.ExpressionProfileBuildingBlocksByName(_project, "UDPGT1|Human|Healthy");
         var initialConditions = _moduleTask.InitialConditionBuildingBlockByName(module, "Initial Conditions");
         var parameterValues = _moduleTask.ParameterValueBuildingBlockByName(module, "Parameter Values");
         var moduleConfig = sut.CreateModuleConfiguration(module, parameterValues, initialConditions);
         var moduleConfigurations = new List<ModuleConfiguration> { moduleConfig };

         var config = sut.CreateConfiguration(_simulationName, moduleConfigurations, expressionProfiles, individual);
         return sut.CreateSimulationFrom(config);
      }
   }

   internal class when_creating_simulation_from_project_module : concern_for_SimulationCreateTask
   {
      private Simulation _simulation;
      private readonly string _simulationName = "Sim1";

      protected override void Context()
      {
         base.Context();
         LoadSampleProject("SampleProject.mbp3");
      }

      protected override void Because()
      {
         var module = _projectTask.ModuleByName(_project, "Module1");
         _simulation = CreateSimulationFromModule(module);
      }

      [Observation]
      public void should_return_simulation_name() =>
         _simulation.Name.ShouldBeEqualTo(_simulationName);

      [Observation]
      public void should_contain_module() =>
         _simulation.Configuration.ModuleConfigurations.Any(x => x.Module.Name == "Module1").ShouldBeTrue();
   }

   internal class when_creating_simulation_from_pkml_module : concern_for_SimulationCreateTask
   {
      private Simulation _simulation;

      protected override void Context()
      {
         base.Context();
         LoadSampleProject("SampleProject.mbp3");
      }

      protected override void Because()
      {
         var module = _moduleTask.LoadModulesFromFile(DomainHelperForSpecs.DataTestFileFullPath("Second module.pkml")).First();
         _simulationName = "SimFromPKML";
         _simulation = CreateSimulationFromModule(module);
      }

      [Observation]
      public void should_return_simulation_name() =>
         _simulation.Name.ShouldBeEqualTo(_simulationName);

      [Observation]
      public void should_contain_loaded_module() =>
         _simulation.Configuration.ModuleConfigurations.Any().ShouldBeTrue();
   }
}