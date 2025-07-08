using CommandLine;
using Microsoft.Extensions.Logging;
using MoBi.Assets;
using MoBi.CLI.Commands;
using MoBi.CLI.Core.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Services;

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

         Parser.Default.ParseArguments<SnapshotRunCommand>(args)
            .WithParsed<SnapshotRunCommand>(startCommand)
            .WithNotParsed(err => _valid = false);

         if (!_valid)
            return (int)ExitCodes.Error;

         return (int)ExitCodes.Success;

      }

      private static void startCommand<TRunOptions>(CLICommand<TRunOptions> command)
      {
         var logger = initializeLogger(command);
         if (command.LogCommandName)
            logger.AddInfo($"Starting {command.Name.ToLower()} run");

         logger.AddDebug($"Arguments:\n{command}");
         ApplicationStartup.Start();
         var runner = IoC.Resolve<IBatchRunner<TRunOptions>>();
         try
         {
            runner.RunBatchAsync(command.ToRunOptions()).Wait();
         }
         catch (Exception e)
         {
            logger.AddException(e);
            _valid = false;
         }

         if (command.LogCommandName)
            logger.AddInfo($"{command.Name} run finished");
      }

      private static IOSPSuiteLogger initializeLogger(CLICommand runCommand)
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

         return logger;
      }
   }
}
