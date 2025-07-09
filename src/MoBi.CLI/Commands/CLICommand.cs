using System.Text;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace MoBi.CLI.Commands
{
   public abstract class CLICommand
   {
      public abstract string Name { get; }

      public virtual bool LogCommandName { get; } = true;

      [Option('l', "log", Required = false, HelpText = "Optional. Full path of log files where log output will be written. A log file will not be created if this value is not provided.")]
      public IEnumerable<string> LogFilesFullPath { get; set; } = new string[] { };

      [Option("logLevel", Required = false, HelpText = "Optional. Log verbosity (Debug, Information, Warning, Error). Default is Information.")]
      public LogLevel LogLevel { get; set; } = LogLevel.Information;

      protected virtual void LogDefaultOptions(StringBuilder sb)
      {
         sb.AppendLine($"Log level: {LogLevel}");
      }
   }

   public abstract class CLICommand<TRunOptions> : CLICommand
   {
      public abstract TRunOptions ToRunOptions();
   }
}