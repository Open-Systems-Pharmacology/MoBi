using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.R.Domain;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using static MoBi.R.Tests.HelperForSpecs;
using IProjectTask = MoBi.R.Services.IProjectTask;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;

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
   protected SimulationRequest _request;

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
   protected SimulationCreationResult _creationResult;

   protected override void Context()
   {
      base.Context();

      _moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");
      _individualForSimulation = _projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)");
      _expressionProfilesForSimulation = _projectTask.ExpressionProfileBuildingBlocksByName(_project, new[] { "UDPGT1|Human|Healthy" });

      _moduleConfiguration = sut.CreateModuleConfiguration(_moduleForSimulation, "Parameter Values", "Initial Conditions");

      _request = new SimulationRequest();
      _request.AddModuleConfiguration(_moduleConfiguration);
      foreach (var ep in _expressionProfilesForSimulation ?? Array.Empty<ExpressionProfileBuildingBlock>())
         _request.AddExpressionProfile(ep);
      _request.SetIndividual(_individualForSimulation);

      _projectTask.CloseProject();
   }
}

internal class when_creating_simulation : when_creating_from_mobi_project
{
   protected override void Because()
   {
      _creationResult = sut.CreateSimulationResultsFrom(_simulationName, _request);
   }

   [Observation]
   public void should_return_simulation_name()
   {
      _creationResult.ShouldNotBeNull();
      _creationResult.Simulation.ShouldNotBeNull();
      _creationResult.Simulation.Name.ShouldBeEqualTo(_simulationName);
   }

   [Observation]
   public void should_contain_module()
   {
      var module = _creationResult.Simulation.Configuration.ModuleConfigurations
         .FirstOrDefault(x => x.Module.Name == "Module1")?.Module;
      module.ShouldNotBeNull();
   }
}

internal abstract class when_creating_an_invalid_configuration : when_creating_from_mobi_project
{
   [Observation]
   public void should_throw_expected_exception()
   {
      The.Action(() => sut.CreateSimulationResultsFrom(_simulationName, _request))
         .ShouldThrowAn<InvalidOperationException>();
   }
}

internal class when_creating_simulation_from_pkml_module : concern_for_SimulationTask
{
   private SimulationCreationResult _creationResult;

   protected override void Context()
   {
      base.Context();
      var module = _moduleTask.LoadModulesFromFile(DataTestFileFullPath("Second module.pkml")).First();
      _simulationName = "SimFromPKML";
      var moduleConfig = sut.CreateModuleConfiguration(module);

      _request = new SimulationRequest();
      _request.AddModuleConfiguration(moduleConfig);
   }

   protected override void Because()
   {
      _creationResult = sut.CreateSimulationResultsFrom(_simulationName, _request);
   }

   [Observation]
   public void should_return_simulation_name() =>
      _creationResult.Simulation.Name.ShouldBeEqualTo(_simulationName);

   [Observation]
   public void should_contain_loaded_module() =>
      _creationResult.Simulation.Configuration.ModuleConfigurations.Any().ShouldBeTrue();
}

internal class when_creating_simulation_with_warnings_only : concern_for_SimulationTask
{
   private SimulationCreationResult _creationResult;

   protected override void Context()
   {
      base.Context();
      _moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");
      _simulationName = "SimWithWarningsOnly";
      var moduleConfig = sut.CreateModuleConfiguration(_moduleForSimulation, "Parameter Values", "Initial Conditions");

      _request = new SimulationRequest();
      _request.AddModuleConfiguration(moduleConfig);
      _request.SetIndividual(_projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)"));

      _projectTask.CloseProject();
   }

   protected override void Because()
   {
      _creationResult = sut.CreateSimulationResultsFrom(_simulationName, _request);
   }

   [Observation]
   public void should_create_simulation_despite_warnings()
   {
      _creationResult.ShouldNotBeNull();
      _creationResult.Simulation.ShouldNotBeNull();
      _creationResult.Simulation.Name.ShouldBeEqualTo(_simulationName);
      _creationResult.Simulation.Configuration.ModuleConfigurations.Any().ShouldBeTrue();
      _creationResult.Warnings.ShouldNotBeNull();
   }
}

internal class when_creating_simulation_with_errors : concern_for_SimulationTask
{
   protected override void Context()
   {
      base.Context();
      _simulationName = $"Invalid{Constants.ILLEGAL_CHARACTERS.First()}Name";

      var module = _projectTask.ModuleByName(_project, "Module1");
      var moduleConfig = sut.CreateModuleConfiguration(module);

      _request = new SimulationRequest();
      _request.AddModuleConfiguration(moduleConfig);
      _request.SetIndividual(_projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)"));

      _projectTask.CloseProject();
   }

   [Observation]
   public void should_throw_expected_exception()
   {
      The.Action(() => sut.CreateSimulationResultsFrom(_simulationName, _request))
         .ShouldThrowAn<InvalidOperationException>();
   }
}