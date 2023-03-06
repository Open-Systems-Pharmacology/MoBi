using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
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
      private const string GET_EXPRESSION_DATABASE_QUERY = "GetExpressionDatabaseQuery";
      private const string PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR = "PKSim.UI.Starter.ExpressionProfileCreator";
      private const string PKSIM_UI_STARTER_INDIVIDUAL_CREATOR = "PKSim.UI.Starter.IndividualCreator";
      private const string PKSIM_UI_STARTER_DLL = "PKSim.UI.Starter.dll";

      private readonly IMoBiConfiguration _configuration;
      private readonly IApplicationSettings _applicationSettings;
      private readonly IStartableProcessFactory _startableProcessFactory;
      private Assembly _externalAssembly;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public PKSimStarter(IMoBiConfiguration configuration, IApplicationSettings applicationSettings,
         IStartableProcessFactory startableProcessFactory, ICloneManagerForBuildingBlock cloneManager)
      {
         _configuration = configuration;
         _applicationSettings = applicationSettings;
         _startableProcessFactory = startableProcessFactory;
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

         return executeAndCloneBuildingBlock<ExpressionProfileBuildingBlock>(getMethod(PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR, methodName));
      }

      public IBuildingBlock CreateIndividual()
      {
         return executeAndCloneBuildingBlock<IndividualBuildingBlock>(getMethod(PKSIM_UI_STARTER_INDIVIDUAL_CREATOR, CREATE_INDIVIDUAL));
      }

      private TBuildingBlock executeAndCloneBuildingBlock<TBuildingBlock>(MethodInfo method) where TBuildingBlock : class, IBuildingBlock
      {
         loadPKSimAssembly();
         var buildingBlock = executeMethod(method) as TBuildingBlock;

         //in case of cancelling
         if (buildingBlock == null)
            return null;

         return _cloneManager.CloneBuildingBlock(buildingBlock) as TBuildingBlock;
      }

      public IReadOnlyList<ExpressionParameterValueUpdate> UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock expressionProfile)
      {
         loadPKSimAssembly();
         return executeMethod(getMethod(PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR, GET_EXPRESSION_DATABASE_QUERY), new object[] { expressionProfile }) as List<ExpressionParameterValueUpdate>;
      }

      private object executeMethod(MethodInfo method, object[] parameters = null)
      {
         return method.Invoke(null, parameters);
      }

      private void loadPKSimAssembly()
      {
         if (_externalAssembly != null)
            return;

         _externalAssembly = Assembly.LoadFrom(retrievePKSimUIStarterPath());
      }

      private MethodInfo getMethod(string type, string methodName)
      {
         loadPKSimAssembly();

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

         throw new MoBiException(AppConstants.PKSim.IncompatibleVersionInstalled);
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