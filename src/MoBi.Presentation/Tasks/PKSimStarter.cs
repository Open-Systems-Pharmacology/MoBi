using System;
using System.IO;
using System.Reflection;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public class PKSimStarter : IPKSimStarter
   {
      private readonly IMoBiConfiguration _configuration;
      private readonly IApplicationSettings _applicationSettings;
      private readonly IStartableProcessFactory _startableProcessFactory;
      private Assembly _externalAssembly;
      private readonly IShell _shell;
      private Type _expressionCreatorType;

      public PKSimStarter(IMoBiConfiguration configuration, IApplicationSettings applicationSettings, IStartableProcessFactory startableProcessFactory, IShell shell)
      {
         _configuration = configuration;
         _applicationSettings = applicationSettings;
         _startableProcessFactory = startableProcessFactory;
         _shell = shell;
      }

      public void StartPopulationSimulationWithSimulationFile(string simulationFilePath)
      {
         startPKSimWithFile(simulationFilePath, AppConstants.PKSim.PopulationSimulationArgument);
      }

      public void StartWithWorkingJournalFile(string journalFilePath)
      {
         startPKSimWithFile(journalFilePath, AppConstants.PKSim.JournalFileArgument);
      }

      public void CreateMetabolizingEnzymeExpression()
      {
         loadPKSimAssembly();
         var result = executeMethod(_expressionCreatorType.GetMethod("CreateIndividualEnzymeExpressionProfile"));
      }

      private object executeMethod(MethodInfo method)
      {
         return method.Invoke(null, new object[] { _shell });
      }

      private void loadPKSimAssembly()
      {
         var assemblyFile = retrievePKSimUIStarterPath();

         if (_externalAssembly == null)
         {
            _externalAssembly = Assembly.LoadFrom(assemblyFile);
         }

         if (_expressionCreatorType == null)
            _expressionCreatorType = _externalAssembly.GetType("PKSim.UI.Starter.ExpressionProfileCreator");
      }

      public void CreateBindingPartnerExpression()
      {
         loadPKSimAssembly();
         var result = executeMethod(_expressionCreatorType.GetMethod("CreateBindingPartnerExpressionProfile"));
      }

      public void CreateTransporterExpression()
      {
         loadPKSimAssembly();
         var result = executeMethod(_expressionCreatorType.GetMethod("CreateTransporterExpressionProfile"));
      }

      private void startPKSimWithFile(string filePathToStart, string option)
      {
         var pkSimPath = retrievePKSimExecutablePath();

         //now start PK-Sim
         var args = new[]
         {
            option,
            $"\"{filePathToStart}\""
         };

         this.DoWithinExceptionHandler(() => { _startableProcessFactory.CreateStartableProcess(pkSimPath, args).Start(); });
      }

      private string retrievePKSimUIStarterPath()
      {
         var pkSimPath = retrievePKSimExecutablePath();
         var directory = Path.GetDirectoryName(pkSimPath);

         var dllName = "PKSim.UI.Starter.dll";

         if (directory != null)
         {
            var assemblyFile = Path.Combine(directory, dllName);
            if (FileHelper.FileExists(assemblyFile))
               return assemblyFile;
         }

         throw new MoBiException(AppConstants.PKSim.EntryPointNotFound(dllName));
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