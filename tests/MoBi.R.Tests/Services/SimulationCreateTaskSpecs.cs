using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.R.Domain;
using MoBi.R.Services;
using NUnit.Framework;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.R.Domain;
using static MoBi.R.Tests.DomainHelperForSpecs;
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
         var projectFile = TestFileFullPath("SampleProject.mbp3");
         _project = _projectTask.GetProject(projectFile);
      }

      protected override void Because()
      {
         _moduleNames = _projectTask.GetModuleNames(_project);

         var expressionProfileNames = _projectTask.GetExpressionProfileNames(_project);
         var simulationConfig = new SimulationConfiguration();
         simulationConfig.ModuleConfigurations.AddRange(_moduleNames.Select(x => new ModuleConfiguration { ModuleName = x, SelectedInitialConditionsName = x }));
         simulationConfig.IndividualName = "";
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
   }
}