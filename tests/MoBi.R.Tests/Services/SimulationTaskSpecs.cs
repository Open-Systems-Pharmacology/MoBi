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
   protected IModuleTask _moduleTask; 
   protected string _simulationName = "Sim1";
   protected IProjectTask _projectTask;
   protected MoBiProject _project;

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

   protected IndividualBuildingBlock _individualForSimulation;
   protected IReadOnlyList<ExpressionProfileBuildingBlock> _expressionProfilesForSimulation;
   protected Module _moduleForSimulation;
   protected ModuleConfiguration _moduleConfiguration;
   protected List<ModuleConfiguration> _moduleConfigurations;
   protected SimulationConfiguration _simulationConfig;

   protected override void Context()
   {
      base.Context();


      _moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");
      _individualForSimulation = _projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)");
      _expressionProfilesForSimulation = _projectTask.ExpressionProfileBuildingBlocksByName(_project, new string[] {"UDPGT1|Human|Healthy"});

      _moduleConfiguration = sut.CreateModuleConfiguration(_moduleForSimulation, "Parameter Values", "Initial Conditions");
      _moduleConfigurations = [_moduleConfiguration];

      _simulationConfig = sut.CreateConfiguration(_moduleConfigurations, _expressionProfilesForSimulation, _individualForSimulation);
   }
}

internal class when_creating_simulation : when_creating_from_mobi_project
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

internal abstract class when_creating_an_invalid_configuration : when_creating_from_mobi_project
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

internal class when_creating_simulation_from_pkml_module_and_the_selected_parameter_values_are_not_found : concern_for_SimulationTask
{
   private Module _module;

   protected override void Because()
   {
      _module = _moduleTask.LoadModulesFromFile(DataTestFileFullPath("Second module.pkml")).First();
   }

   [Observation]
   public void the_exception_should_be_thrown() =>
      The.Action(() => sut.CreateModuleConfiguration(_module, "Parameter Values")).ShouldThrowAn<InvalidArgumentException>();
}

internal class when_creating_simulation_from_pkml_module_and_the_selected_initial_conditions_are_not_found : concern_for_SimulationTask
{
   private Module _module;

   protected override void Because()
   {
      _module = _moduleTask.LoadModulesFromFile(DataTestFileFullPath("Second module.pkml")).First();
      _module.Add(new ParameterValuesBuildingBlock().WithName("Parameter Values"));
   }

   [Observation]
   public void the_exception_should_be_thrown() =>
      The.Action(() => sut.CreateModuleConfiguration(_module, null, "Initial Conditions")).ShouldThrowAn<InvalidArgumentException>();
}

internal class when_creating_simulation_from_pkml_module : concern_for_SimulationTask
{
   private Simulation _simulation;
   private List<ModuleConfiguration> _moduleConfigurations;
   
   private IndividualBuildingBlock _individual;
   private ExpressionProfileBuildingBlock[] _expressionProfiles;

   protected override void Context()
   {
      base.Context();
      var module = _moduleTask.LoadModulesFromFile(DataTestFileFullPath("Second module.pkml")).First();
      _simulationName = "SimFromPKML";
      _individual = _projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)");
      _expressionProfiles = _projectTask.ExpressionProfileBuildingBlocksByName(_project, "UDPGT1|Human|Healthy");
      var moduleConfig = sut.CreateModuleConfiguration(module);
      _moduleConfigurations = [moduleConfig];
   }

   protected override void Because()
   {
      _simulation = sut.CreateSimulationFrom(sut.CreateConfiguration(_moduleConfigurations, _expressionProfiles, _individual), _simulationName);
   }

   [Observation]
   public void should_return_simulation_name() =>
      _simulation.Name.ShouldBeEqualTo(_simulationName);

   [Observation]
   public void should_contain_loaded_module() =>
      _simulation.Configuration.ModuleConfigurations.Any().ShouldBeTrue();
}

