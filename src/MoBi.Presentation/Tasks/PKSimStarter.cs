using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public class PKSimStarter : IPKSimStarter
   {
      private readonly IMoBiConfiguration _configuration;
      private readonly IApplicationSettings _applicationSettings;
      private readonly IStartableProcessFactory _startableProcessFactory;

      public PKSimStarter(IMoBiConfiguration configuration, IApplicationSettings applicationSettings, IStartableProcessFactory startableProcessFactory)
      {
         _configuration = configuration;
         _applicationSettings = applicationSettings;
         _startableProcessFactory = startableProcessFactory;
      }

      public void StartPopulationSimulationWithSimulationFile(string simulationFilePath)
      {
         startPKSimWithFile(simulationFilePath, AppConstants.PKSim.PopulationSimulationArgument);
      }

      public void StartWithWorkingJournalFile(string journalFilePath)
      {
         startPKSimWithFile(journalFilePath, AppConstants.PKSim.JournalFileArgument);
      }

      private void startPKSimWithFile(string filePathToStart, string option)
      {
         var moBiPath = retrievePKSimExecutablePath();

         //now start PK-Sim
         var args = new[]
         {
            option,
            $"\"{filePathToStart}\""
         };

         this.DoWithinExceptionHandler(() => { _startableProcessFactory.CreateStartableProcess(moBiPath, args).Start(); });
      }

      private string retrievePKSimExecutablePath()
      {
         //Installed properly via Setup? return standard path
         if (FileHelper.FileExists(_configuration.PKSimPath))
            return _configuration.PKSimPath;

         if (FileHelper.FileExists(_applicationSettings.PKSimPath))
            return _applicationSettings.PKSimPath;

         throw new MoBiException(AppConstants.PKSim.NotInstalled);
      }
   }
}