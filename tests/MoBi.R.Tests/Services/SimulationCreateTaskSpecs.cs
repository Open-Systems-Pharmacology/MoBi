using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
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
      private IReadOnlyList<string> _moduleNames;
      private MoBiProject _project;
      private Simulation _simulation;
      private readonly string _simulationName = "Sim1";
      private IProjectTask _projectTask;

      protected override void Context()
      {
         base.Context();
         _projectTask = Api.GetProjectTask();
         var projectFile = DomainHelperForSpecs.DataTestFileFullPath("SampleProject.mbp3");
         _project = _projectTask.GetProject(projectFile);
      }

      protected override void Because()
      {
         _moduleNames = _projectTask.GetModuleNames(_project);

         var expressionProfileNames = _projectTask.GetExpressionProfileNames(_project);
         var simulationConfig = new SimulationConfiguration();
         simulationConfig.ModuleConfigurations.AddRange(_moduleNames.Select(x => new ModuleConfiguration { ModuleName = x, SelectedInitialConditionsName = x }));
         simulationConfig.ExpressionProfileNames = expressionProfileNames.ToList();
         simulationConfig.SimulationName = _simulationName;
         _simulation = sut.CreateSimulationFrom(simulationConfig, _project);
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
         foreach (var moduleName in _moduleNames)
         {
            var module = _simulation.Configuration.ModuleConfigurations
               .FirstOrDefault(x => x.Module.Name == moduleName)?.Module;
            module.ShouldNotBeNull();
         }
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
         var projectFile = DomainHelperForSpecs.DataTestFileFullPath("SampleProject.mbp3");
         _project = _projectTask.GetProject(projectFile);
      }

      protected override void Because()
      {
         _simulationConfig = CreateInvalidConfiguration();
      }

      protected abstract SimulationConfiguration CreateInvalidConfiguration();

      [Test]
      public void should_throw_expected_exception()
      {
         The.Action(() => sut.CreateSimulationFrom(_simulationConfig, _project)).ShouldThrowAn<InvalidOperationException>();
      }
   }

   internal class when_creating_simulation_with_invalid_module_name : concern_for_invalid_simulation_configuration
   {
      protected override SimulationConfiguration CreateInvalidConfiguration()
      {
         return new SimulationConfiguration
         {
            SimulationName = "Sim1",
            ModuleConfigurations = new List<ModuleConfiguration>
            {
               new ModuleConfiguration { ModuleName = "non_existing_module" }
            }
         };
      }
   }

   internal class when_creating_simulation_with_invalid_expression_profile : concern_for_invalid_simulation_configuration
   {
      protected override SimulationConfiguration CreateInvalidConfiguration()
      {
         return new SimulationConfiguration
         {
            SimulationName = "Sim1",
            ExpressionProfileNames = new List<string> { "non_existing_expression" }
         };
      }
   }

   internal class when_creating_simulation_with_invalid_individual : concern_for_invalid_simulation_configuration
   {
      protected override SimulationConfiguration CreateInvalidConfiguration()
      {
         return new SimulationConfiguration
         {
            SimulationName = "Sim1",
            IndividualName = "non_existing_individual"
         };
      }
   }
}