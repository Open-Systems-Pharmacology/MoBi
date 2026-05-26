using System;
using System.IO;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.CLI.Core.Services;
using OSPSuite.Utility;
using static MoBi.R.Tests.HelperForSpecs;
using MoBiSimulation = MoBi.R.Domain.MoBiSimulation;
using ISnapshotTask = MoBi.R.Services.ISnapshotTask;

namespace MoBi.R.Tests.Services
{
   internal abstract class concern_for_SnapshotTask : ContextForIntegration<ISnapshotTask>
   {
      protected string _snapshotFile;

      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = Api.GetSnapshotTask();
      }

      protected override void Context()
      {
         base.Context();
         _snapshotFile = DataTestFileFullPath("snapshot_no_pksim_modules.json");
      }
   }

   internal class When_loading_all_simulations_from_a_snapshot : concern_for_SnapshotTask
   {
      private MoBiSimulation[] _simulations;

      protected override void Because()
      {
         _simulations = sut.LoadSimulationsFromSnapshot(_snapshotFile);
      }

      [Observation]
      public void should_return_every_simulation_defined_in_the_snapshot()
      {
         _simulations.ShouldNotBeNull();
         _simulations.Length.ShouldBeGreaterThan(0);
      }
   }

   internal class When_loading_simulations_filtered_by_an_existing_simulation_name : concern_for_SnapshotTask
   {
      private MoBiSimulation[] _simulations;
      private string _existingSimulationName;

      protected override void Context()
      {
         base.Context();
         _existingSimulationName = sut.LoadSimulationsFromSnapshot(_snapshotFile).First().Name;
      }

      protected override void Because()
      {
         _simulations = sut.LoadSimulationsFromSnapshot(_snapshotFile, _existingSimulationName);
      }

      [Observation]
      public void should_return_only_simulations_whose_name_matches()
      {
         _simulations.Length.ShouldBeEqualTo(1);
         _simulations[0].Name.ShouldBeEqualTo(_existingSimulationName);
      }
   }

   internal class When_loading_simulations_filtered_by_a_name_that_does_not_exist : concern_for_SnapshotTask
   {
      private MoBiSimulation[] _simulations;

      protected override void Because()
      {
         _simulations = sut.LoadSimulationsFromSnapshot(_snapshotFile, "ThisSimulationDoesNotExist");
      }

      [Observation]
      public void should_return_an_empty_array()
      {
         _simulations.Length.ShouldBeEqualTo(0);
      }
   }

   internal class When_running_a_snapshot_export_using_the_convenient_method : concern_for_SnapshotTask
   {
      private readonly string _inputFolder = Path.Combine(Path.GetTempPath(), $"MoBi_SnapshotTask_Input_{Guid.NewGuid():N}");
      private readonly string _outputFolder = Path.Combine(Path.GetTempPath(), $"MoBi_SnapshotTask_Output_{Guid.NewGuid():N}");
      private readonly string _snapshotFileName = "snapshot_no_pksim_modules.json";

      protected override void Context()
      {
         base.Context();
         DirectoryHelper.CreateDirectory(_inputFolder);
         FileHelper.Copy(_snapshotFile, Path.Combine(_inputFolder, _snapshotFileName));
      }

      protected override void Because()
      {
         sut.RunSnapshot(_inputFolder, _outputFolder, exportMode: SnapshotExportMode.Project);
      }

      [Observation]
      public void should_write_the_mbp3_project_file_to_the_output_folder()
      {
         Directory.Exists(_outputFolder).ShouldBeTrue();
         File.Exists(Path.Combine(_outputFolder, "snapshot_no_pksim_modules.mbp3")).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         DirectoryHelper.DeleteDirectory(_outputFolder, true);
         DirectoryHelper.DeleteDirectory(_inputFolder, true);
      }
   }
}