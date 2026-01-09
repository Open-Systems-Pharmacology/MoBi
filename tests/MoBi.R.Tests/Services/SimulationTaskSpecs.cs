using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;
using static MoBi.R.Tests.HelperForSpecs;
using IProjectTask = MoBi.R.Services.IProjectTask;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_SimulationTask : ContextForIntegration<ISimulationTask>
{
   protected IModuleTask _moduleTask;
   protected string _simulationName = "Sim1";
   protected IProjectTask _projectTask;
   protected MoBiProject _project;
   protected IndividualBuildingBlock _individualForSimulation;
   protected IReadOnlyList<ExpressionProfileBuildingBlock> _expressionProfilesForSimulation;
   protected Module _moduleForSimulation;
   protected ModuleConfiguration _moduleConfiguration;
   protected List<ModuleConfiguration> _moduleConfigurations;

   public override void GlobalContext()
   {
      base.GlobalContext();
      _moduleTask = Api.GetModuleTask();

      _projectTask = Api.GetProjectTask();

      var projectFile = DataTestFileFullPath("SampleProject.mbp3");
      _project = _projectTask.LoadProject(projectFile);

      sut = Api.GetSimulationTask();
   }
}

internal class when_creating_from_mobi_project : concern_for_SimulationTask
{
   protected Simulation _simulation;

   protected override void Context()
   {
      base.Context();

      _moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");
      _individualForSimulation = _projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)");
      _expressionProfilesForSimulation = _projectTask.ExpressionProfileBuildingBlocksByName(_project, new string[] { "UDPGT1|Human|Healthy" });

      _moduleConfiguration = sut.CreateModuleConfiguration(_moduleForSimulation, "Parameter Values", "Initial Conditions");

      // Add items one by one
      sut.AddModuleConfiguration(_moduleConfiguration);
      foreach (var ep in _expressionProfilesForSimulation ?? Array.Empty<ExpressionProfileBuildingBlock>())
         sut.AddExpressionProfile(ep);

      _projectTask.CloseProject();
   }
}

internal class when_creating_simulation : when_creating_from_mobi_project
{
   protected override void Because()
   {
      _simulation = sut.CreateSimulationFrom(_simulationName, _individualForSimulation);
   }

   [Observation]
   public void should_return_simulation_name()
   {
      _simulation.ShouldNotBeNull();
      _simulation.Name.ShouldBeEqualTo(_simulationName);
   }

   [Observation]
   public void should_contain_module()
   {
      var module = _simulation.Configuration.ModuleConfigurations
         .FirstOrDefault(x => x.Module.Name == "Module1")?.Module;
      module.ShouldNotBeNull();
   }
}

internal abstract class when_creating_an_invalid_configuration : when_creating_from_mobi_project
{
   [Observation]
   public void should_throw_expected_exception()
   {
      The.Action(() => sut.CreateSimulationFrom(_simulationName, _individualForSimulation))
         .ShouldThrowAn<InvalidOperationException>();
   }
}

internal class when_creating_simulation_from_pkml_module : concern_for_SimulationTask
{
   private Simulation _simulation;
   private List<ModuleConfiguration> _moduleConfigurations;

   protected override void Context()
   {
      base.Context();
      var module = _moduleTask.LoadModulesFromFile(DataTestFileFullPath("Second module.pkml")).First();
      _simulationName = "SimFromPKML";
      var moduleConfig = sut.CreateModuleConfiguration(module);
      _moduleConfigurations = [moduleConfig];

      // Add one by one
      sut.AddModuleConfiguration(moduleConfig);
   }

   protected override void Because()
   {
      _simulation = sut.CreateSimulationFrom(_simulationName, _individualForSimulation);
   }

   [Observation]
   public void should_return_simulation_name() =>
      _simulation.Name.ShouldBeEqualTo(_simulationName);

   [Observation]
   public void should_contain_loaded_module() =>
      _simulation.Configuration.ModuleConfigurations.Any().ShouldBeTrue();
}