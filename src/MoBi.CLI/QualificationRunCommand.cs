using CommandLine;
using MoBi.CLI.Commands;
using MoBi.CLI.Core.RunOptions;
using System.Text;

namespace MoBi.CLI
{
   [Verb("qualification", HelpText = "Start qualification run workflow")]
   public class QualificationRunCommand : CLICommand<MoBiQualificationRunOptions>
   {
      public override string Name { get; } = "Qualification";
      public override bool LogCommandName { get; } = false;

      [Option('i', "input", Required = true, HelpText = "Json configuration file used to start the qualification workflow.")]
      public string ConfigurationFile { get; set; }

      [Option('v', "validate", Required = false, HelpText = "Specifies a validation run. Default is false")]
      public bool Validate { get; set; }

      [Option('r', "run", Required = false, HelpText = "Should the qualification runner also run the simulation or simply export the qualification report for further processing. Default is false")]
      public bool Run { get; set; } = false;

      [Option('e', "exp", Required = false, HelpText = "Should the qualification runner also export the project files (snapshot and MoBi project file). Default is false")]
      public bool ExportProjectFiles { get; set; } = false;

      [Option('p', "pksim", Required = false, HelpText = "The file path where PK-Sim can be found for qualifications that use PK-Sim modules. Default is to use the value from user settings in MoBi")]
      public string PKSimPath { get; set; }

      public override MoBiQualificationRunOptions ToRunOptions()
      {
         return new MoBiQualificationRunOptions
         {
            ConfigurationFile = ConfigurationFile,
            Validate = Validate,
            Run = Run,
            ExportProjectFiles = ExportProjectFiles,
            PKSimPath = PKSimPath
         };
      }

      public override string ToString()
      {
         var sb = new StringBuilder();
         LogDefaultOptions(sb);
         sb.AppendLine($"Validate: {Validate}");
         sb.AppendLine($"Configuration file: {ConfigurationFile}");
         sb.AppendLine($"Run simulations: {Run}");
         sb.AppendLine($"Export project files: {ExportProjectFiles}");
         sb.AppendLine($"Path to PK-Sim: {PKSimPath}");
         return sb.ToString();
      }
   }
}