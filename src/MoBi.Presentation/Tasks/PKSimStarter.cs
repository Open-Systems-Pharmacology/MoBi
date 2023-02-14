using System.IO;
using System.Reflection;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public class PKSimStarter : IPKSimStarter
   {
      private const string CREATE_INDIVIDUAL_ENZYME_EXPRESSION_PROFILE = "CreateIndividualEnzymeExpressionProfile";
      private const string CREATE_BINDING_PARTNER_EXPRESSION_PROFILE = "CreateBindingPartnerExpressionProfile";
      private const string CREATE_TRANSPORTER_EXPRESSION_PROFILE = "CreateTransporterExpressionProfile";
      private const string CREATE_INDIVIDUAL = "CreateIndividual";
      private const string PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR = "PKSim.UI.Starter.ExpressionProfileCreator";
      private const string PKSIM_UI_STARTER_INDIVIDUAL_CREATOR = "PKSim.UI.Starter.IndividualCreator";
      private const string PKSIM_UI_STARTER_DLL = "PKSim.UI.Starter.dll";
      private readonly IMoBiConfiguration _configuration;
      private readonly IApplicationSettings _applicationSettings;
      private readonly IStartableProcessFactory _startableProcessFactory;
      private Assembly _externalAssembly;
      private readonly IShell _shell;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public PKSimStarter(IMoBiConfiguration configuration, IApplicationSettings applicationSettings,
         IStartableProcessFactory startableProcessFactory, IShell shell, ICloneManagerForBuildingBlock cloneManager)
      {
         _configuration = configuration;
         _applicationSettings = applicationSettings;
         _startableProcessFactory = startableProcessFactory;
         _shell = shell;
         _cloneManager = cloneManager;
      }

      public void StartPopulationSimulationWithSimulationFile(string simulationFilePath)
      {
         startPKSimWithFile(simulationFilePath, AppConstants.PKSim.PopulationSimulationArgument);
      }

      public void StartWithWorkingJournalFile(string journalFilePath)
      {
         startPKSimWithFile(journalFilePath, AppConstants.PKSim.JournalFileArgument);
      }

      public IBuildingBlock CreateProfileExpression(ExpressionType expressionType)
      {
         var methodName = string.Empty;

         if (expressionType == ExpressionTypes.MetabolizingEnzyme)
            methodName = CREATE_INDIVIDUAL_ENZYME_EXPRESSION_PROFILE;
         else if (expressionType == ExpressionTypes.ProteinBindingPartner)
            methodName = CREATE_BINDING_PARTNER_EXPRESSION_PROFILE;
         else if (expressionType == ExpressionTypes.TransportProtein)
            methodName = CREATE_TRANSPORTER_EXPRESSION_PROFILE;

         if (string.IsNullOrEmpty(methodName))
            return null;

         loadPKSimAssembly();
         var expressionProfileBuildingBlock =
            executeMethodWithShell(getMethod(PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR, methodName)) as ExpressionProfileBuildingBlock;

         //in case of cancelling
         if (expressionProfileBuildingBlock == null)
            return null;

         return _cloneManager.CloneBuildingBlock(expressionProfileBuildingBlock);
      }

      public IBuildingBlock CreateIndividual()
      {
         loadPKSimAssembly();
         var individualBuildingBlock =
            executeMethodWithShell(getMethod(PKSIM_UI_STARTER_INDIVIDUAL_CREATOR, CREATE_INDIVIDUAL)) as IndividualBuildingBlock;

         //in case of cancelling
         if (individualBuildingBlock == null)
            return null;

         return _cloneManager.CloneBuildingBlock(individualBuildingBlock);
      }

      private object executeMethodWithShell(MethodInfo method)
      {
         return executeMethod(method, new object[] { _shell });
      }

      private object executeMethod(MethodInfo method, object[] parameters)
      {
         return method.Invoke(null, parameters);
      }

      private void loadPKSimAssembly()
      {
         var assemblyFile = retrievePKSimUIStarterPath();

         if (_externalAssembly == null)
         {
            _externalAssembly = Assembly.LoadFrom(assemblyFile);
         }
      }

      private MethodInfo getMethod(string type, string methodName)
      {
         return _externalAssembly.GetType(type).GetMethod(methodName);
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

         var dllName = PKSIM_UI_STARTER_DLL;

         if (directory != null)
         {
            var assemblyFile = Path.Combine(directory, dllName);
            if (FileHelper.FileExists(assemblyFile))
               return assemblyFile;
         }

         throw new MoBiException(AppConstants.PKSim.NotInstalled);
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