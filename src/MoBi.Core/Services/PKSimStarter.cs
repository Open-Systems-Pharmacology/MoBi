using System.Diagnostics;
using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Exceptions;

namespace MoBi.Core.Services
{
   public interface IPKSimStarter
   {
      /// <summary>
      ///    Checks if pk sim is installed. Throws an exeption if not.
      /// </summary>
      /// <exception cref="MoBiException"></exception>
      void CheckPKSimInstallation();

      void StartWithSimulationFile(string simulationFilePath);
      void StartWithWorkingJournalFile(string journalFilePath);
   }

   public class PKSimStarter : IPKSimStarter
   {
      private readonly IMoBiConfiguration _moBiConfiguration;

      public PKSimStarter(IMoBiConfiguration moBiConfiguration)
      {
         _moBiConfiguration = moBiConfiguration;
      }

      /// <summary>
      ///    Checks if pk sim is installed. Throws an exeption if not.
      /// </summary>
      /// <exception cref="MoBiException"></exception>
      public void CheckPKSimInstallation()
      {
         if (!FileHelper.FileExists(_moBiConfiguration.PKSimPath))
            throw new MoBiException(AppConstants.PKSim.NotInstalled);
      }

      public void StartWithSimulationFile(string simulationFilePath)
      {
         this.DoWithinExceptionHandler(() => Process.Start(_moBiConfiguration.PKSimPath, string.Format("{0} \"{1}\"", AppConstants.PKSim.PopulationSimulationArgument, simulationFilePath)));
      }

      public void StartWithWorkingJournalFile(string journalFilePath)
      {
         this.DoWithinExceptionHandler(() => Process.Start(_moBiConfiguration.PKSimPath, string.Format("{0} \"{1}\"", AppConstants.PKSim.JournalFileArgument, journalFilePath)));
      }
   }
}