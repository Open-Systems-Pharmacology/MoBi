using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using MoBi.Assets;
using MoBi.CLI.Core.RunOptions;
using MoBi.CLI.Core.Services;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Snapshots.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace MoBi.CLI
{
   public abstract class concern_for_SnapshotRunner : ContextSpecificationAsync<SnapshotRunner>
   {
      protected ISnapshotTask _snapshotTask;
      protected IOSPSuiteLogger _logger;
      protected SnapshotRunOptions _runOptions;
      protected string _createdDirectory;
      private Func<string, string> _oldCreateDirectory;
      protected readonly string _inputFolder = @"C:\Input\";
      protected readonly string _outputFolder = @"C:\Output\";
      protected IProjectTask _projectTask;
      protected IMoBiContext _moBiContext;
      protected IContextPersistor _contextPersistor;

      public override async Task GlobalContext()
      {
         await base.GlobalContext();
         _oldCreateDirectory = DirectoryHelper.CreateDirectory;
         DirectoryHelper.CreateDirectory = s => _createdDirectory = s;
      }

      protected override Task Context()
      {
         _projectTask = A.Fake<IProjectTask>();
         _moBiContext = A.Fake<IMoBiContext>();
         _snapshotTask = A.Fake<ISnapshotTask>();
         _logger = A.Fake<IOSPSuiteLogger>();
         _contextPersistor = A.Fake<IContextPersistor>();
         sut = new SnapshotRunner(_snapshotTask, _logger, _moBiContext, _projectTask, _contextPersistor);

         _runOptions = new SnapshotRunOptions();
         return _completed;
      }

      public override async Task GlobalCleanup()
      {
         await base.GlobalCleanup();
         DirectoryHelper.CreateDirectory = _oldCreateDirectory;
      }
   }

   public class When_running_the_snapshot_runner_for_a_single_folder_option_generating_project : concern_for_SnapshotRunner
   {
      private readonly string _fileName = "snapshotFile";
      private string _snapshotFile;
      private string _projectFile;

      protected override async Task Context()
      {
         await base.Context();
         _runOptions.ExportMode = SnapshotExportMode.Project;
         _runOptions.InputFolder = _inputFolder;
         _runOptions.OutputFolder = _outputFolder;
         _snapshotFile = Path.Combine(_inputFolder, $"{_fileName}{Constants.Filter.JSON_EXTENSION}");
         _projectFile = Path.Combine(_outputFolder, $"{_fileName}{AppConstants.Filter.MOBI_PROJECT_EXTENSION}");
         sut.AllFilesFrom = (folder, filter) => new[] { new FileInfo(_snapshotFile) };
      }

      protected override Task Because()
      {
         return sut.RunBatchAsync(_runOptions);
      }

      [Observation]
      public void should_load_the_snapshot_from_file()
      {
         A.CallTo(() => _snapshotTask.LoadProjectFromSnapshotFileAsync(_snapshotFile, true)).MustHaveHappened();
      }

      [Observation]
      public void should_generate_the_project_from_snapshot()
      {
         A.CallTo(() => _contextPersistor.Save(_moBiContext)).MustHaveHappened();
      }

      [Observation]
      public void should_generate_the_output_folder_in_case_in_does_not_exist()
      {
         _createdDirectory.ShouldBeEqualTo(_outputFolder);
      }
   }

   public class When_running_the_snapshot_runner_for_a_single_folder_option_generating_snapshot : concern_for_SnapshotRunner
   {
      private readonly string _fileName = "project";
      private string _snapshotFile;
      private string _projectFile;

      protected override async Task Context()
      {
         await base.Context();
         _runOptions.ExportMode = SnapshotExportMode.Snapshot;
         _runOptions.InputFolder = _inputFolder;
         _runOptions.OutputFolder = _outputFolder;
         _snapshotFile = Path.Combine(_outputFolder, $"{_fileName}{Constants.Filter.JSON_EXTENSION}");
         _projectFile = Path.Combine(_inputFolder, $"{_fileName}{Constants.Filter.PKML_EXTENSION}");

         sut.AllFilesFrom = (folder, filter) => new[] { new FileInfo(_projectFile) };
      }

      protected override Task Because()
      {
         return sut.RunBatchAsync(_runOptions);
      }

      [Observation]
      public void should_load_the_project_into_workspace()
      {
         A.CallTo(() => _projectTask.LoadProject(_projectFile)).MustHaveHappened();
      }

      [Observation]
      public void should_save_the_project_to_snapshot()
      {
         A.CallTo(() => _snapshotTask.ExportModelToSnapshotAsync(A<IProject>._, _snapshotFile)).MustHaveHappened();
      }

      [Observation]
      public void should_generate_the_output_folder_in_case_in_does_not_exist()
      {
         _createdDirectory.ShouldBeEqualTo(_outputFolder);
      }
   }

   public class When_running_the_snapshot_runner_for_a_multi_folder_option_generating_project : concern_for_SnapshotRunner
   {
      private readonly string _fileName = "snapshotFile";
      private string _snapshotFile;
      private string _projectFile;

      protected override async Task Context()
      {
         await base.Context();
         _runOptions.ExportMode = SnapshotExportMode.Project;
         _runOptions.Folders = new[] { _inputFolder };
         _snapshotFile = Path.Combine(_inputFolder, $"{_fileName}{Constants.Filter.JSON_EXTENSION}");
         _projectFile = Path.Combine(_inputFolder, $"{_fileName}{AppConstants.Filter.MOBI_PROJECT_EXTENSION}");
         sut.AllFilesFrom = (folder, filter) => new[] { new FileInfo(_snapshotFile) };
      }

      protected override Task Because()
      {
         return sut.RunBatchAsync(_runOptions);
      }

      [Observation]
      public void should_load_the_snapshot_from_file()
      {
         A.CallTo(() => _snapshotTask.LoadProjectFromSnapshotFileAsync(_snapshotFile, true)).MustHaveHappened();
      }

      [Observation]
      public void should_generate_the_project_from_snapshot()
      {
         A.CallTo(() => _contextPersistor.Save(_moBiContext)).MustHaveHappened();
      }
   }
}