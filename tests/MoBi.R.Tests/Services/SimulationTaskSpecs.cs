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
using OSPSuite.Utility.Extensions;
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

      _request = new SimulationRequest { SimulationName = _simulationName };
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
      _creationResult = sut.CreateSimulationsAndValidateFrom(_request).Single();
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

internal abstract class when_creating_multiple_simulations_from_requests : when_creating_from_mobi_project
{
   protected SimulationCreationResult[] _results;
   protected string[] _simulationNames;
   protected SimulationRequest[] _requests;

   protected override void Context()
   {
      base.Context();
      _requests = _simulationNames.Select(createRequest).ToArray();
   }

   protected override void Because()
   {
      _results = sut.CreateSimulationsAndValidateFrom(_requests);
   }

   private SimulationRequest createRequest(string simulationName)
   {
      var request = new SimulationRequest { SimulationName = simulationName };
      request.AddModuleConfiguration(_moduleConfiguration);
      (_expressionProfilesForSimulation ?? Array.Empty<ExpressionProfileBuildingBlock>()).Each(request.AddExpressionProfile);
      request.SetIndividual(_individualForSimulation);
      return request;
   }
}

internal class when_creating_multiple_simulations : when_creating_multiple_simulations_from_requests
{
   protected override void Context()
   {
      _simulationNames = new[] { "ParallelSim1", "ParallelSim2", "ParallelSim3", "ParallelSim4" };
      base.Context();
   }

   [Observation]
   public void should_return_one_result_per_request_in_the_request_order()
   {
      _results.Length.ShouldBeEqualTo(_simulationNames.Length);
      _results.Select(x => x.Simulation.Name).ToArray().ShouldBeEqualTo(_simulationNames);
   }

   [Observation]
   public void should_create_all_simulations_from_the_shared_module()
   {
      _results.Each(result =>
      {
         result.Simulation.ShouldNotBeNull();
         result.Simulation.Configuration.ModuleConfigurations
            .FirstOrDefault(x => x.Module.Name == "Module1")?.Module.ShouldNotBeNull();
      });
   }
}

internal class when_creating_multiple_simulations_and_one_cannot_be_created : when_creating_multiple_simulations_from_requests
{
   protected override void Context()
   {
      _simulationNames = new[] { "ParallelSim1", $"Invalid{Constants.ILLEGAL_CHARACTERS.First()}Name", "ParallelSim3" };
      base.Context();
   }

   [Observation]
   public void should_return_the_errors_for_the_failing_simulation()
   {
      _results[1].Simulation.ShouldBeNull();
      _results[1].Errors.Any().ShouldBeTrue();
   }

   [Observation]
   public void should_create_the_other_simulations_in_the_request_order()
   {
      _results[0].Simulation.Name.ShouldBeEqualTo(_simulationNames[0]);
      _results[2].Simulation.Name.ShouldBeEqualTo(_simulationNames[2]);
   }
}

internal class when_creating_multiple_simulations_without_requests : when_creating_from_mobi_project
{
   [Observation]
   public void should_throw_expected_exception_when_the_requests_are_null()
   {
      The.Action(() => sut.CreateSimulationsAndValidateFrom(null))
         .ShouldThrowAn<InvalidArgumentException>();
   }

   [Observation]
   public void should_return_no_results_when_there_are_no_requests()
   {
      sut.CreateSimulationsAndValidateFrom().ShouldBeEmpty();
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

      _request = new SimulationRequest { SimulationName = _simulationName };
      _request.AddModuleConfiguration(moduleConfig);
   }

   protected override void Because()
   {
      _creationResult = sut.CreateSimulationsAndValidateFrom(_request).Single();
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

      _request = new SimulationRequest { SimulationName = _simulationName };
      _request.AddModuleConfiguration(moduleConfig);
      _request.SetIndividual(_projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)"));

      _projectTask.CloseProject();
   }

   protected override void Because()
   {
      _creationResult = sut.CreateSimulationsAndValidateFrom(_request).Single();
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

      _request = new SimulationRequest { SimulationName = _simulationName };
      _request.AddModuleConfiguration(moduleConfig);
      _request.SetIndividual(_projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)"));

      _projectTask.CloseProject();
   }

   [Observation]
   public void should_return_the_errors_instead_of_a_simulation()
   {
      var result = sut.CreateSimulationsAndValidateFrom(_request).Single();
      result.Simulation.ShouldBeNull();
      result.Errors.Any().ShouldBeTrue();
   }
}

internal class when_creating_simulation_with_create_all_process_rate_parameters_flag : concern_for_SimulationTask
{
   private SimulationCreationResult _creationResult;

   protected override void Context()
   {
      base.Context();

      _moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");
      var moduleConfig = sut.CreateModuleConfiguration(_moduleForSimulation, "Parameter Values", "Initial Conditions");

      _request = new SimulationRequest { SimulationName = _simulationName };
      _request.AddModuleConfiguration(moduleConfig);
      _request.SetIndividual(_projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)"));
      _request.CreateAllProcessRateParameters = true;

      _projectTask.CloseProject();
   }

   protected override void Because()
   {
      _creationResult = sut.CreateSimulationsAndValidateFrom(_request).Single();
   }

   [Observation]
   public void should_propagate_create_all_process_rate_parameters_to_simulation_configuration()
   {
      _creationResult.ShouldNotBeNull();
      _creationResult.Simulation.ShouldNotBeNull();
      _creationResult.Simulation.Configuration.CreateAllProcessRateParameters.ShouldBeTrue();
   }
}

internal class when_creating_simulation_with_custom_simulation_settings : concern_for_SimulationTask
{
   private SimulationCreationResult _creationResult;

   protected override void Context()
   {
      base.Context();

      _moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");
      var moduleConfig = sut.CreateModuleConfiguration(_moduleForSimulation, "Parameter Values", "Initial Conditions");

      var customSettings = new SimulationSettings();
      customSettings.Name = "CustomSettings";

      _request = new SimulationRequest { SimulationName = _simulationName };
      _request.AddModuleConfiguration(moduleConfig);
      _request.SetIndividual(_projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)"));
      _request.SimulationSettings = customSettings;

      _projectTask.CloseProject();
   }

   protected override void Because()
   {
      _creationResult = sut.CreateSimulationsAndValidateFrom(_request).Single();
   }

   [Observation]
   public void should_use_the_provided_simulation_settings()
   {
      _creationResult.ShouldNotBeNull();
      _creationResult.Simulation.ShouldNotBeNull();
      _creationResult.Simulation.Settings.ShouldNotBeNull();
      _creationResult.Simulation.Settings.Name.ShouldBeEqualTo("CustomSettings");
   }
}

internal class when_creating_simulation_with_calculation_method_override : concern_for_SimulationTask
{
   private SimulationCreationResult _creationResult;
   private string _category;
   private const string _overriddenCalculationMethod = "OverriddenMethod";

   protected override void Context()
   {
      base.Context();

      _moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");
      _moduleForSimulation.Molecules.Add(new MoleculeBuilder().WithName("Molecule name"));

      var molecule = _moduleForSimulation.Molecules.First();
      molecule.AddUsedCalculationMethod(new UsedCalculationMethod("someCategory", "someName"));
      var usedCalculationMethod = molecule.UsedCalculationMethods.First();
      _category = usedCalculationMethod.Category;
      
      var moduleConfiguration = sut.CreateModuleConfiguration(_moduleForSimulation, "Parameter Values", "Initial Conditions");

      _request = new SimulationRequest { SimulationName = _simulationName };
      _request.AddModuleConfiguration(moduleConfiguration);
      _request.SetIndividual(_projectTask.IndividualBuildingBlockByName(_project, "European (P-gp modified, CYP3A4 36 h)"));
      _request.AddMoleculeUsedCalculationMethod("Molecule name", _category, _overriddenCalculationMethod);

      _projectTask.CloseProject();
   }

   protected override void Because()
   {
      _creationResult = sut.CreateSimulationsAndValidateFrom(_request).Single();
   }

   [Observation]
   public void should_contain_the_override_in_the_simulation_configuration()
   {
      _creationResult.ShouldNotBeNull();
      _creationResult.Simulation.ShouldNotBeNull();
      var overrideForMolecule = _creationResult.Simulation.Configuration.CalculationMethodOverridesFor("Molecule name");
      overrideForMolecule.ShouldNotBeNull();
      overrideForMolecule.UsedCalculationMethods
         .Single(ucm => ucm.Category == _category)
         .CalculationMethod.ShouldBeEqualTo(_overriddenCalculationMethod);
   }
}