using System.IO;
using System.Linq;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;

namespace MoBi.Presentation
{
   public enum StartOptionsMode
   {
      Unspecified,
      Project,
      Simulation,
      Journal
   }

   public class StartOptions : IStartOptions
   {
      /// <summary>
      ///    File that should be loaded automatically
      /// </summary>
      public string FileToLoad { get; private set; }

      /// <summary>
      ///    Loading a project or a simulation file
      /// </summary>
      public StartOptionsMode StartOptionsMode { get; private set; }

      public bool IsDeveloperMode { get; private set; } = false;

      private readonly string[] _developerFlags = {"/dev", "--dev"};

      public void InitializeFrom(string[] args)
      {
         if (args.Length == 0)
            return;

         updateDeveloperMode(args);

         var arguments = stripDeveloperFlagsFrom(args);
         if (arguments.Length == 0)
            return;

         //only one argument=>should be a mobi project file or journal
         if (arguments.Length == 1)
         {
            FileToLoad = arguments[0];
            StartOptionsMode = isJournalFile(FileToLoad) ? StartOptionsMode.Journal : StartOptionsMode.Project;
            return;
         }

         //at least 2 arguments
         var command = arguments[0];
         FileToLoad = arguments[1];

         if (isSimulation(command))
         {
            StartOptionsMode = StartOptionsMode.Simulation;
            return;
         }

         if (isProject(command))
         {
            StartOptionsMode = StartOptionsMode.Project;
            return;
         }

         if (isJournal(command) && isJournalFile(FileToLoad))
         {
            StartOptionsMode = StartOptionsMode.Journal;
            return;
         }
      }

      private void updateDeveloperMode(string[] args)
      {
         IsDeveloperMode = args.ContainsAny(_developerFlags);
      }

      private string[] stripDeveloperFlagsFrom(string[] args)
      {
         return args.Except(_developerFlags).ToArray();
      }

      private static bool isJournalFile(string journalFilePath)
      {
         if (!FileHelper.FileExists(journalFilePath))
            return false;

         var extension = new FileInfo(journalFilePath).Extension;
         return string.Equals(extension, Constants.Filter.JOURNAL_EXTENSION);
      }

      private static bool isProject(string firstArgument)
      {
         return firstArgumentIsOneOf(firstArgument, "/p", "/project", "-p", "--project");
      }

      private static bool isSimulation(string firstArgument)
      {
         return firstArgumentIsOneOf(firstArgument, "/s", "/simulation", "-s", "--simulation");
      }

      private static bool isJournal(string firstArgument)
      {
         return firstArgumentIsOneOf(firstArgument, "/j", "/journal", "-j", "--journal");
      }

      private static bool firstArgumentIsOneOf(string firstArgument, params string[] listOfValues)
      {
         if (string.IsNullOrEmpty(firstArgument))
            return false;
         var lowerCaseArgument = firstArgument.ToLower();
         return lowerCaseArgument.IsOneOf(listOfValues);
      }

      public bool IsValid()
      {
         return FileHelper.FileExists(FileToLoad) && StartOptionsMode != StartOptionsMode.Unspecified;
      }
   }
}