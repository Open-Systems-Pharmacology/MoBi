using System.IO;
using MoBi.CLI.Core.RunOptions;
using MoBi.CLI.Core.Services;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using MoBi.HelpersForTests;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;

namespace MoBi.IntegrationTests
{
   public abstract class concern_for_SnapshotRunner : concern_for_BatchRunnerSpecs<SnapshotRunOptions>
   {
   }

   public class when_exporting_a_snapshot_from_project : concern_for_SnapshotRunner
   {
      private SnapshotRunOptions _snapshotRunOptions;
      private string _projectDirectory;
      private string _jsonDirectory;
      private readonly string _jsonFileName = "snapshot_no_pksim_modules.json";
      private readonly string _projectFileName = "snapshot_no_pksim_modules.mbp3";
      private string _jsonPath;
      private string _projectPath;

      protected override void Context()
      {
         base.Context();
         _projectDirectory = Path.Combine(Path.GetTempPath(), "projects");
         _jsonDirectory = Path.Combine(Path.GetTempPath(), "json");

         _jsonPath = Path.Combine(_jsonDirectory, _jsonFileName);
         _projectPath = Path.Combine(_projectDirectory, _projectFileName);

         createProjectFromSnapshot();

         File.Delete(_jsonPath);
         _snapshotRunOptions = new SnapshotRunOptions
         {
            ExportMode = SnapshotExportMode.Snapshot,
            InputFolder = _projectDirectory,
            OutputFolder = _jsonDirectory
         };

         loadProject(_projectPath);
      }

      private void createProjectFromSnapshot()
      {
         _snapshotRunOptions = new SnapshotRunOptions
         {
            ExportMode = SnapshotExportMode.Project,
            InputFolder = _jsonDirectory,
            OutputFolder = _projectDirectory
         };

         if (!Directory.Exists(_projectDirectory))
            Directory.CreateDirectory(_projectDirectory);

         if (!Directory.Exists(_jsonDirectory))
            Directory.CreateDirectory(_jsonDirectory);

         File.Copy(Path.Combine(DomainHelperForSpecs.TestFileDirectory, _jsonFileName), _jsonPath, overwrite: true);

         sut.RunBatchAsync(_snapshotRunOptions).Wait();
      }

      private void loadProject(string projectFile)
      {
         var contextPersistor = IoC.Resolve<IContextPersistor>();
         var context = IoC.Resolve<IMoBiContext>();
         contextPersistor.Load(context, projectFile);
      }

      protected override void Because()
      {
         sut.RunBatchAsync(_snapshotRunOptions).Wait();
      }

      [Observation]
      public void the_project_file_should_be_created()
      {
         File.Exists(_jsonPath).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         if (Directory.Exists(_projectDirectory))
            Directory.Delete(_projectDirectory, true);

         if (Directory.Exists(_jsonDirectory))
            Directory.Delete(_jsonDirectory, true);
      }
   }

   public class when_loading_a_project_from_snapshot : concern_for_SnapshotRunner
   {
      private SnapshotRunOptions _snapshotRunOptions;
      private string _projectDirectory;
      private string _jsonDirectory;
      private readonly string _jsonFileName = "snapshot_no_pksim_modules.json";
      private readonly string _projectFileName = "snapshot_no_pksim_modules.mbp3";
      private string _inputPath;
      private string _outputPath;

      protected override void Context()
      {
         base.Context();
         _projectDirectory = Path.Combine(Path.GetTempPath(), "snapshots_outputs");
         _jsonDirectory = Path.Combine(Path.GetTempPath(), "snapshots_inputs");

         _inputPath = Path.Combine(_jsonDirectory, _jsonFileName);
         _outputPath = Path.Combine(_projectDirectory, _projectFileName);

         _snapshotRunOptions = new SnapshotRunOptions
         {
            ExportMode = SnapshotExportMode.Project,
            InputFolder = _jsonDirectory,
            OutputFolder = _projectDirectory
         };

         if (!Directory.Exists(_projectDirectory))
            Directory.CreateDirectory(_projectDirectory);

         if (!Directory.Exists(_jsonDirectory))
            Directory.CreateDirectory(_jsonDirectory);

         File.Copy(Path.Combine(DomainHelperForSpecs.TestFileDirectory, _jsonFileName), _inputPath, overwrite: true);
      }

      protected override void Because()
      {
         sut.RunBatchAsync(_snapshotRunOptions).Wait();
      }

      [Observation]
      public void the_project_file_should_be_created()
      {
         File.Exists(_outputPath).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         if (Directory.Exists(_projectDirectory))
            Directory.Delete(_projectDirectory, true);

         if (Directory.Exists(_jsonDirectory))
            Directory.Delete(_jsonDirectory, true);
      }
   }
}