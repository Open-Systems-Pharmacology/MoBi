using System.Text;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Logging;
using MoBi.CLI.Commands;
using MoBi.CLI.Core.RunOptions;
using OSPSuite.CLI.Core.RunOptions;
using OSPSuite.CLI.Core.Services;
using System.Collections.Generic;

namespace MoBi.CLI
{
   [Verb("snap", HelpText = "Start snapshot workflow by loading a set of project (or snapshot) files and creating the corresponding snapshot (or project) file automatically.")]
   public class SnapshotRunCommand : CLICommand<MoBiSnapshotRunOptions>, IWithInputAndOutputFolders
   {
      public override string Name { get; } = "Snapshot";

      public SnapshotExportMode ExportMode { get; set; } = SnapshotExportMode.Snapshot;

      [Option('i', "input", Required = true, HelpText = "Input folder containing all project or snapshot files to load.")]
      public string InputFolder { get; set; }

      [Option('o', "output", Required = true, HelpText = "Output folder where project or snapshot files will be exported.")]
      public string OutputFolder { get; set; }

      [Option("pksim", Required = false, HelpText = "The file path where PK-Sim can be found, required when loading snapshots that use PK-Sim modules. Default is to use the value from user settings in MoBi.")]
      public string PKSimPath { get; set; }

      [Option('p', "project", HelpText = "Create project files from snapshot files.")]
      public bool ExportProject
      {
         set => ExportMode = SnapshotExportMode.Project;
         get => ExportMode == SnapshotExportMode.Project;
      }

      [Option('s', "snapshot", HelpText = "Create snapshot files from project files.")]
      public bool ExportSnapshot
      {
         set => ExportMode = SnapshotExportMode.Snapshot;
         get => ExportMode == SnapshotExportMode.Snapshot;
      }

      [Usage(ApplicationAlias = "MoBi.CLI")]
      public static IEnumerable<Example> Examples
      {
         get
         {
            yield return new Example("Create snapshot files from project files", new SnapshotRunCommand { InputFolder = "<SnapshotFolder>", OutputFolder = "<ProjectFolder>", ExportSnapshot = true });
            yield return new Example("Create project files from snapshot files using short notation", UnParserSettings.WithGroupSwitchesOnly(), new SnapshotRunCommand { InputFolder = "<ProjectFolder>", OutputFolder = "<SnapshotFolder>", ExportProject = true });
            yield return new Example("Create project files from snapshot files and only log errors", new SnapshotRunCommand
            {
               InputFolder = "<ProjectFolder>",
               OutputFolder = "<SnapshotFolder>",
               ExportProject = true,
               LogLevel = LogLevel.Error
            });
         }
      }

      public override string ToString()
      {
         var sb = new StringBuilder();
         this.LogFolders(sb);
         LogDefaultOptions(sb);
         sb.AppendLine($"Export mode: {ExportMode}");
         return sb.ToString();
      }

      public override MoBiSnapshotRunOptions ToRunOptions()
      {
         return new MoBiSnapshotRunOptions
         {
            OutputFolder = OutputFolder,
            InputFolder = InputFolder,
            ExportMode = ExportMode,
            PKSimPath = PKSimPath
         };
      }
   }
}