using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.R.Domain;
using MoBi.R.Services;
using NUnit.Framework;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.R.Domain;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;

namespace MoBi.R.Tests.Services
{
   internal abstract class concern_for_SimulationCreateTask : ContextForIntegration<ISimulationCreateTask>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = Api.GetSimulationCreateTask;
      }
   }

   internal class when_creating_simulation : concern_for_SimulationCreateTask
   {
      private MoBiProject _project;
      private Simulation _simulation;
      private readonly string _simulationName = "Sim1";
      private IProjectTask _projectTask;
      private IReadOnlyList<string> _moduleNames;

      protected override void Context()
      {
         base.Context();
         _projectTask = Api.GetProjectTask();
         var projectFile = DomainHelperForSpecs.TestFileFullPath("SampleProject.mbp3");
         _project = _projectTask.LoadProject(projectFile);
      }

      protected override void Because()
      {
         var moduleForSimulation = _projectTask.ModuleByName(_project, "Module1");

         var individualForSimulation = _projectTask.IndividualByName(_project, "European (P-gp modified, CYP3A4 36 h)");

         var expressionProfilesForSimulation = _projectTask.ExpressionProfilesByName(_project, "UDPGT1|Human|Healthy");

         var initialConditionForModule = _projectTask.InitialConditionByName(moduleForSimulation, "Initial Conditions");

         var parameterValuesForModule = _projectTask.ParameterValueByName(moduleForSimulation, "Parameter Values");

         var moduleConfiguration = sut.CreateModuleConfiguration(moduleForSimulation, parameterValuesForModule, initialConditionForModule);

         var moduleConfigurations = new List<ModuleConfiguration> { moduleConfiguration };

         var simulationConfig = sut.CreateConfiguration(_simulationName, moduleConfigurations, expressionProfilesForSimulation, individualForSimulation);

         _simulation = sut.CreateSimulationFrom(simulationConfig);
      }

      [Test]
      public void should_return_simulation_name()
      {
         _simulation.ShouldNotBeNull();
         _simulation.Name.ShouldBeEqualTo(_simulationName);
      }

      [Test]
      public void should_contain_module()
      {
         var module = _simulation.Configuration.ModuleConfigurations
            .FirstOrDefault(x => x.Module.Name == "Module1")?.Module;
         module.ShouldNotBeNull();
      }
   }

   internal abstract class concern_for_invalid_simulation_configuration : concern_for_SimulationCreateTask
   {
      protected MoBiProject _project;
      protected IProjectTask _projectTask;
      protected SimulationConfiguration _simulationConfig;

      protected override void Context()
      {
         base.Context();
         _projectTask = Api.GetProjectTask();
         var projectFile = DomainHelperForSpecs.TestFileFullPath("SampleProject.mbp3");
         _project = _projectTask.LoadProject(projectFile);
      }

      protected override void Because()
      {
         _simulationConfig = CreateInvalidConfiguration();
      }

      protected abstract SimulationConfiguration CreateInvalidConfiguration();

      [Test]
      public void should_throw_expected_exception()
      {
         The.Action(() => sut.CreateSimulationFrom(_simulationConfig)).ShouldThrowAn<InvalidOperationException>();
      }
   }
}