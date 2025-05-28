using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using MoBi.R.Services;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.R.Services;
using static MoBi.HelpersForTests.DomainHelperForSpecs;

namespace MoBi.R.Tests.Services
{
   internal abstract class concern_for_ProjectTask : ContextForIntegration<IProjectTask>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = Api.GetProjectTask();
      }
   }

   internal class when_running_a_simulation_from_a_project : concern_for_ProjectTask
   {
      protected IModelCoreSimulation _simulation;
      protected ISimulationRunner _simulationRunnerTask;
      private SimulationResults _results;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _simulationRunnerTask = OSPSuite.R.Api.GetSimulationRunner();
         var projectFile = DataTestFileFullPath("SampleProjectWith2Simulations.mbp3");
         _project = sut.LoadProject(projectFile);
         _simulation = _project.Simulations.FirstOrDefault();
      }

      protected override void Because()
      {
         _results = _simulationRunnerTask.Run(new SimulationRunArgs { Simulation = _simulation });
      }

      [Observation]
      public void should_create_results()
      {
         _results.ShouldNotBeNull();
      }
   }

   internal class when_reading_module_names_from_project : concern_for_ProjectTask
   {
      private IReadOnlyList<string> _moduleNames;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();

         var projectFile = DataTestFileFullPath("SampleProject.mbp3");
         _project = sut.LoadProject(projectFile);
      }

      protected override void Because()
      {
         _moduleNames = sut.AllModuleNames(_project);
      }

      [Test]
      public void should_return_module_names()
      {
         _moduleNames.ShouldNotBeNull();
         _moduleNames.Count.ShouldBeEqualTo(2);
         _moduleNames.ShouldContain("Module1");
         _moduleNames.ShouldContain("Module2");
      }
   }

   internal class when_reading_building_blocks_names_from_module : concern_for_ProjectTask
   {
      private IReadOnlyList<string> _buildingBlockNames;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();

         var projectFile = DataTestFileFullPath("SampleProject.mbp3");
         _project = sut.LoadProject(projectFile);
      }

      protected override void Because()
      {
         var moduleNames = sut.AllModuleNames(_project);
         _buildingBlockNames = sut.AllBuildingBlocksNamesFromModuleName(_project, moduleNames.First());
      }

      [Observation]
      public void should_return_module_building_blocks_names()
      {
         _buildingBlockNames.ShouldNotBeNull();
         _buildingBlockNames.ShouldOnlyContain("Organism", "Molecules", "Initial Conditions", "Reactions");
      }
   }

   internal class when_reading_simulation_names_from_project : concern_for_ProjectTask
   {
      private IReadOnlyList<string> _simulationNames;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();

         var projectFile = DataTestFileFullPath("SampleProjectWith2Simulations.mbp3");
         _project = sut.LoadProject(projectFile);
      }

      protected override void Because()
      {
         _simulationNames = sut.AllSimulationNames(_project);
      }

      [Observation]
      public void should_return_simulation_names()
      {
         _simulationNames.ShouldNotBeNull();
         _simulationNames.Count.ShouldBeEqualTo(2);
         _simulationNames.ShouldOnlyContain("Simulation1", "Simulation2");
      }
   }
}