using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using MoBi.CLI.Core.Services;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Snapshots.Services;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.CLI.Core.RunOptions;
using OSPSuite.CLI.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Qualification;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using SnapshotProject = MoBi.Core.Snapshots.Project;
using Simulation = MoBi.Core.Snapshots.Simulation;

namespace MoBi.CLI
{
   public abstract class concern_for_QualificationRunner : ContextSpecificationAsync<QualificationRunner>
   {
      private IMoBiContext _context;
      private IProjectPersistor _projectPersistor;
      private IOSPSuiteLogger _logger;
      protected IDataRepositoryExportTask _dataRepositoryTask;
      protected IJsonSerializer _jsonSerializer;
      protected ISnapshotTask _snapshotTask;
      protected ISimulationPersistor _simulationPersistor;
      private Func<string, string> _oldCreateDirectory;
      private Func<string, bool> _oldDirectoryExists;
      private Action<string, bool> _oldDeleteDirectory;
      private Func<string, bool> _oldFileExists;
      protected List<string> _createdDirectories = new List<string>();
      protected QualificationRunOptions _runOptions;
      protected QualificationConfiguration _qualificationConfiguration;

      public override async Task GlobalContext()
      {
         await base.GlobalContext();
         _oldCreateDirectory = DirectoryHelper.CreateDirectory;
         _oldDirectoryExists = DirectoryHelper.DirectoryExists;
         _oldDeleteDirectory = DirectoryHelper.DeleteDirectory;
         _oldFileExists = FileHelper.FileExists;
         DirectoryHelper.CreateDirectory = s =>
         {
            _createdDirectories.Add(s);
            return s;
         };
      }

      protected override Task Context()
      {
         _context = A.Fake<IMoBiContext>();
         _projectPersistor = A.Fake<IProjectPersistor>();
         _logger = A.Fake<IOSPSuiteLogger>();
         _dataRepositoryTask = A.Fake<IDataRepositoryExportTask>();
         _jsonSerializer = A.Fake<IJsonSerializer>();
         _snapshotTask = A.Fake<ISnapshotTask>();
         _simulationPersistor = A.Fake<ISimulationPersistor>();

         _runOptions = new QualificationRunOptions();
         _qualificationConfiguration = new QualificationConfiguration();

         sut = new QualificationRunner(_context, _projectPersistor, _logger, _dataRepositoryTask, _jsonSerializer, _snapshotTask, _simulationPersistor);

         return _completed;
      }

      public override async Task GlobalCleanup()
      {
         await base.GlobalCleanup();
         DirectoryHelper.CreateDirectory = _oldCreateDirectory;
         FileHelper.FileExists = _oldFileExists;
         DirectoryHelper.DirectoryExists = _oldDirectoryExists;
         DirectoryHelper.DeleteDirectory = _oldDeleteDirectory;
      }
   }

   public class When_running_the_qualification_runner_with_an_invalid_configuration : concern_for_QualificationRunner
   {
      [Observation]
      public void should_log_the_error()
      {
         The.Action(() => sut.RunBatchAsync(_runOptions)).ShouldThrowAn<QualificationRunException>();
      }
   }

   public abstract class concern_for_QualificationRunnerWithValidConfiguration : concern_for_QualificationRunner
   {
      protected SnapshotProject _projectSnapshot;
      protected MoBiProject _project;
      protected const string PROJECT_NAME = "toto";
      protected const string PROJECT_SNAPSHOT_NAME = "toto_model";

      protected override async Task Context()
      {
         await base.Context();
         _runOptions.ConfigurationFile = "XXX";
         _runOptions.Run = true;
         A.CallTo(() => _jsonSerializer.Deserialize<QualificationConfiguration>(_runOptions.ConfigurationFile)).Returns(_qualificationConfiguration);
         _qualificationConfiguration.Project = PROJECT_NAME;
         _qualificationConfiguration.OutputFolder = "c:/tests/outputs/";
         _qualificationConfiguration.InputsFolder = "c:/tests/outputs/INPUTS";
         _qualificationConfiguration.SnapshotFile = $"c:/tests/inputs/{PROJECT_SNAPSHOT_NAME}.json";
         _qualificationConfiguration.MappingFile = $"c:/tests/temp/{PROJECT_NAME}_Mapping.json";
         _qualificationConfiguration.TempFolder = $"c:/tests/temp";
         _qualificationConfiguration.ReportConfigurationFile = "c:/tests/outputs/report_config.json";
         _qualificationConfiguration.ObservedDataFolder = "c:/tests/outputs/OBS_DATA_FOLDER";

         _projectSnapshot = new SnapshotProject().WithName(PROJECT_SNAPSHOT_NAME);
         _project = new MoBiProject().WithName(PROJECT_NAME);
         A.CallTo(() => _snapshotTask.LoadSnapshotFromFileAsync<SnapshotProject>(_qualificationConfiguration.SnapshotFile)).Returns(_projectSnapshot);
         A.CallTo(() => _snapshotTask.LoadProjectFromSnapshotAsync(_projectSnapshot, _runOptions.Run)).Returns(_project);
         FileHelper.FileExists = s => s.IsOneOf(_qualificationConfiguration.SnapshotFile, _runOptions.ConfigurationFile);
      }
   }

   public class When_running_the_qualification_runner_with_a_valid_configuration_for_a_valid_snapshot_file : concern_for_QualificationRunnerWithValidConfiguration
   {
      private string _expectedOutputPath;
      private string _deletedDirectory;
      private DataRepository _observedData;
      private SimulationMapping[] _simulationExports;
      private SimulationMapping _simulationExport;
      private string _expectedSimulationPath;
      private QualificationMapping _mapping;
      private string _simulationName;
      private string _expectedObservedDataXlsFullPath;
      private string _expectedObservedDataCsvFullPath;
      private Simulation _simulation;
      private MoBiSimulation _moBiSimulation;

      protected override async Task Context()
      {
         await base.Context();

         _expectedOutputPath = Path.Combine(_qualificationConfiguration.OutputFolder, PROJECT_NAME);
         DirectoryHelper.DirectoryExists = s => string.Equals(s, _expectedOutputPath);
         DirectoryHelper.DeleteDirectory = (s, b) => _deletedDirectory = s;

         _simulationName = "S1";
         _simulation = new Simulation { Name = _simulationName };
         _moBiSimulation = new MoBiSimulation { Name = _simulationName };

         _expectedSimulationPath = Path.ChangeExtension(Path.Combine(_expectedOutputPath, _simulationName, _simulationName), Constants.Filter.PKML_EXTENSION);
         _simulationExport = new SimulationMapping { Project = PROJECT_NAME, Simulation = _simulationName, Path = _expectedSimulationPath };
         _simulationExports = new[] { _simulationExport };

         _observedData = DomainHelperForSpecs.ObservedData().WithName("OBS");
         _project.AddObservedData(_observedData);
         _projectSnapshot.Simulations = new[] { _simulation };
         _expectedObservedDataXlsFullPath = Path.Combine(_qualificationConfiguration.ObservedDataFolder, $"{_observedData.Name}{Constants.Filter.XLSX_EXTENSION}");
         _expectedObservedDataCsvFullPath = Path.Combine(_qualificationConfiguration.ObservedDataFolder, $"{_observedData.Name}{Constants.Filter.CSV_EXTENSION}");

         A.CallTo(() => _jsonSerializer.Serialize(A<QualificationMapping>._, _qualificationConfiguration.MappingFile))
            .Invokes(x => _mapping = x.GetArgument<QualificationMapping>(0));

         _project.AddSimulation(_moBiSimulation);
         _qualificationConfiguration.Simulations = new[] { _simulationName, };
         _runOptions.Run = true;
      }

      protected override Task Because()
      {
         return sut.RunBatchAsync(_runOptions);
      }

      [Observation]
      public void should_delete_the_project_output_folder_under_the_output_folder_if_available()
      {
         _deletedDirectory.ShouldBeEqualTo(_expectedOutputPath);
      }

      [Observation]
      public void should_create_the_output_directory_for_the_project()
      {
         _createdDirectories.ShouldContain(_expectedOutputPath);
      }

      [Observation]
      public void should_create_the_output_directory_for_the_observed_data()
      {
         _createdDirectories.ShouldContain(_qualificationConfiguration.ObservedDataFolder);
      }

      [Observation]
      public void should_load_the_project_from_snapshot_and_export_its_simulations_to_the_output_folder()
      {
         A.CallTo(() => _simulationPersistor.Save(A<SimulationTransfer>._, _expectedSimulationPath)).MustHaveHappened();
      }

      [Observation]
      public void should_only_export_the_simulation_required_for_the_qualification()
      {
         A.CallTo(() => _simulationPersistor.Save(A<SimulationTransfer>._, A<string>._)).MustHaveHappened(1, Times.Exactly);
      }

      [Observation]
      public void should_export_the_mapping_to_the_specified_mapping_file()
      {
         _mapping.ShouldNotBeNull();
      }

      [Observation]
      public void should_export_the_simulation_configuration_with_mapping_relative_to_the_report_output_folder()
      {
         _mapping.SimulationMappings.Length.ShouldBeEqualTo(1);
         _mapping.SimulationMappings[0].Simulation.ShouldBeEqualTo(_simulationName);
         _mapping.SimulationMappings[0].Project.ShouldBeEqualTo(PROJECT_NAME);
         _mapping.SimulationMappings[0].Path.ShouldBeEqualTo($"{PROJECT_NAME}/{_simulationName}/");
      }

      [Observation]
      public void should_export_the_observed_data_defined_in_the_project_to_excel_into_the_observed_data_folder()
      {
         A.CallTo(() => _dataRepositoryTask.ExportToExcelAsync(_observedData, _expectedObservedDataXlsFullPath, false, null)).MustHaveHappened();
      }

      [Observation]
      public void should_export_the_observed_data_defined_in_the_project_to_csv_into_the_observed_data_folder()
      {
         A.CallTo(() => _dataRepositoryTask.ExportToCsvAsync(_observedData, _expectedObservedDataCsvFullPath, null)).MustHaveHappened();
      }

      [Observation]
      public void should_export_the_observed_data_mapping_relative_to_the_report_output_folder()
      {
         _mapping.ObservedDataMappings.Length.ShouldBeEqualTo(1);
         _mapping.ObservedDataMappings[0].Id.ShouldBeEqualTo(_observedData.Name);
         _mapping.ObservedDataMappings[0].Path.ShouldBeEqualTo($"OBS_DATA_FOLDER/{_observedData.Name}.csv");
      }
   }
}