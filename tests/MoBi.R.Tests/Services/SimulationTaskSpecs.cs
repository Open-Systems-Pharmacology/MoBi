using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.CLI.Core.Services;
using MoBi.Core.Domain.Model;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;
using static MoBi.R.Tests.HelperForSpecs;
using SimulationConfiguration = MoBi.R.Domain.SimulationConfiguration;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_SimulationTask : ContextForIntegration<ISimulationTask>
{
   protected Simulation _simulation;
   protected string _simulationName = "Sim1";
   protected IProjectTask _projectTask;
   protected IModuleTask _moduleTask;
   protected IndividualBuildingBlock _individualForSimulation;
   protected List<ExpressionProfileBuildingBlock> _expressionProfilesForSimulation;
   protected InitialConditionsBuildingBlock _initialConditionForModule;
   protected Module _moduleForSimulation;
   protected ParameterValuesBuildingBlock _parameterValuesForModule;
   protected ModuleConfiguration _moduleConfiguration;
   protected List<ModuleConfiguration> _moduleConfigurations;
   protected MoBiProject _project;
   protected SimulationConfiguration _simulationConfig;

   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetSimulationTask();
      _projectTask = Api.GetProjectTask();
      _moduleTask = Api.GetModuleTask();
      var projectFile = DataTestFileFullPath("SampleProject.mbp3");
      _project = _projectTask.LoadProject(projectFile);

      _moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");
      _individualForSimulation = _projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)");
      _expressionProfilesForSimulation = _projectTask.ExpressionProfileBuildingBlocksByName(_project, "UDPGT1|Human|Healthy");
      _initialConditionForModule = _moduleTask.InitialConditionBuildingBlockByName(_moduleForSimulation, "Initial Conditions");
      _parameterValuesForModule = _moduleTask.ParameterValueBuildingBlockByName(_moduleForSimulation, "Parameter Values");
      _moduleConfiguration = sut.CreateModuleConfiguration(_moduleForSimulation, _parameterValuesForModule, _initialConditionForModule);
      _moduleConfigurations = [_moduleConfiguration];

      _simulationConfig = sut.CreateConfiguration(_moduleConfigurations, _expressionProfilesForSimulation, _individualForSimulation);
   }
}

internal class when_creating_simulation : concern_for_SimulationTask
{
   protected override void Because()
   {
      _simulation = sut.CreateSimulationFrom(_simulationConfig, _simulationName);
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

internal abstract class when_creating_an_invalid_configuration : concern_for_SimulationTask
{
   [Observation]
   public void should_throw_expected_exception()
   {
      The.Action(() => sut.CreateSimulationFrom(_simulationConfig, _simulationName)).ShouldThrowAn<InvalidOperationException>();
   }
}

internal class when_creating_simulation_with_special_characters_in_name : when_creating_an_invalid_configuration
{
   protected override void Context()
   {
      base.Context();
      _simulationName = $"{_simulationName}{Constants.ILLEGAL_CHARACTERS.First()}";
   }
}

internal class when_creating_simulation_without_a_name : when_creating_an_invalid_configuration
{
   protected override void Context()
   {
      base.Context();
      _simulationName = null;
   }
}