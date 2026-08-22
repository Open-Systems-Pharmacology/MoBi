using System;
using System.Linq;
using CommandLine;
using Microsoft.Extensions.Logging;
using MoBi.Assets;
using MoBi.CLI.Commands;
using MoBi.CLI.Core;
using MoBi.CLI.Core.RunOptions;
using MoBi.Core;
using OSPSuite.CLI.Core.Services;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Utility.Container;

namespace MoBi.CLI
{
   [Flags]
   enum ExitCodes
   {
      Success = 0,
      Error = 1 << 0,
   }

   class Program
   {
      static bool _valid = true;

      static int Main(string[] args)
      {
         ApplicationStartup.Initialize();

         Parser.Default.ParseArguments<SnapshotRunCommand, QualificationRunCommand>(args)
            .WithParsed<SnapshotRunCommand>(startCommand)
            .WithParsed<QualificationRunCommand>(startCommand)
            .WithNotParsed(err => _valid = false);

         if (!_valid)
            return (int)ExitCodes.Error;

         return (int)ExitCodes.Success;
      }

      private static void startCommand<TRunOptions>(CLICommand<TRunOptions> command) where TRunOptions : IProvidePKSimPath
      {
         ApplicationStartup.Start();
         var runOptions = command.ToRunOptions();

         if (!string.IsNullOrEmpty(runOptions.PKSimPath))
            IoC.Resolve<IApplicationSettings>().PKSimPath = runOptions.PKSimPath;

         // If a qualification workflow logs errors, the return code should be non-zero
         var errorCounter = command is QualificationRunCommand ? new ErrorCountingLoggerProvider() : null;

         var logger = initializeLogger(command, errorCounter);
         if (command.LogCommandName)
            logger.AddInfo($"Starting {command.Name.ToLower()} run");

         logger.AddDebug($"Arguments:\n{command}");

         var runner = IoC.Resolve<IBatchRunner<TRunOptions>>();
         try
         {
            runner.RunBatchAsync(runOptions).Wait();
         }
         catch (Exception e)
         {
            logger.AddException(e);
            _valid = false;
         }

         if (errorCounter != null && errorCounter.HasErrors)
         {
            logger.AddError("Qualification run finished with errors. See the log for details.");
            _valid = false;
         }

         if (command.LogCommandName)
            logger.AddInfo($"{command.Name} run finished");
      }

      private static IOSPSuiteLogger initializeLogger(CLICommand runCommand, ErrorCountingLoggerProvider errorCounter)
      {
         var loggerCreator = IoC.Resolve<ILoggerCreator>();

         var logger = IoC.Resolve<IOSPSuiteLogger>();
         logger.DefaultCategoryName = AppConstants.PRODUCT_NAME;

         loggerCreator.AddLoggingBuilderConfiguration(builder =>
            builder
               .SetMinimumLevel(runCommand.LogLevel)
               .AddConsole()
         );

         if (runCommand.LogFilesFullPath.Any())
            loggerCreator.AddLoggingBuilderConfiguration(builder =>
               builder
                  .SetMinimumLevel(runCommand.LogLevel)
                  .AddFile(runCommand.LogFilesFullPath.ToArray(), runCommand.LogLevel, true));

         if (errorCounter != null)
            loggerCreator.AddLoggingBuilderConfiguration(builder => builder.AddProvider(errorCounter));

         return logger;
      }
   }
}