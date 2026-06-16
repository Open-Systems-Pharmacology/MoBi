using System.Collections.Generic;
using System.IO;
using MoBi.Assets;
using MoBi.Core.Exceptions;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public interface IPKSimStarter : IPKSimSnapshotConverter
   {
      void StartPopulationSimulationWithSimulationFile(string simulationFilePath);
      void StartWithWorkingJournalFile(string journalFilePath);
      IBuildingBlock CreateProfileExpression(ExpressionType expressionType);
      IBuildingBlock CreateIndividual();
      IReadOnlyList<ExpressionParameterValueUpdate> UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock expressionProfile);
   }

   /// <summary>
   ///    Desktop PK-Sim integration. In addition to the snapshot exchange (reflecting into
   ///    <c>PKSim.Starter.SnapshotExchange</c> in the installed PK-Sim's PKSim.Starter.dll) it can launch
   ///    the PK-Sim executable and create building blocks through the PK-Sim UI starter.
   /// </summary>
   public class PKSimStarter : PKSimSnapshotConverterBase, IPKSimStarter
   {
      private const string CREATE_INDIVIDUAL_ENZYME_EXPRESSION_PROFILE = "CreateIndividualEnzymeExpressionProfile";
      private const string CREATE_BINDING_PARTNER_EXPRESSION_PROFILE = "CreateBindingPartnerExpressionProfile";
      private const string CREATE_TRANSPORTER_EXPRESSION_PROFILE = "CreateTransporterExpressionProfile";
      private const string CREATE_INDIVIDUAL = "CreateIndividual";
      private const string GET_EXPRESSION_DATABASE_QUERY = "GetExpressionDatabaseQuery";
      private const string PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR = "PKSim.Starter.ExpressionProfileCreator";
      private const string PKSIM_UI_STARTER_INDIVIDUAL_CREATOR = "PKSim.Starter.IndividualCreator";
      private const string PKSIM_UI_STARTER_DLL = "PKSim.Starter.dll";
      private const string PKSIM_UI_STARTER_SNAPSHOT_EXCHANGE = "PKSim.Starter.SnapshotExchange";

      private readonly IMoBiConfiguration _configuration;
      private readonly IApplicationSettings _applicationSettings;
      private readonly IStartableProcessFactory _startableProcessFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public PKSimStarter(IMoBiConfiguration configuration,
         IApplicationSettings applicationSettings,
         IStartableProcessFactory startableProcessFactory,
         ICloneManagerForBuildingBlock cloneManager,
         IXmlSerializationService serializationService,
         IMoBiProjectRetriever projectRetriever,
         IPKSimAssemblyLoader pkSimLoader)
         : base(pkSimLoader, serializationService, projectRetriever)
      {
         _configuration = configuration;
         _applicationSettings = applicationSettings;
         _startableProcessFactory = startableProcessFactory;
         _cloneManager = cloneManager;
         _pkSimLoader.InitializePath(retrievePKSimAssemblyPath());
      }

      protected override string SnapshotExchangeType => PKSIM_UI_STARTER_SNAPSHOT_EXCHANGE;

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

         return executeAndCloneBuildingBlock<ExpressionProfileBuildingBlock>(PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR, methodName);
      }

      public IBuildingBlock CreateIndividual()
      {
         return executeAndCloneBuildingBlock<IndividualBuildingBlock>(PKSIM_UI_STARTER_INDIVIDUAL_CREATOR, CREATE_INDIVIDUAL);
      }

      private TBuildingBlock executeAndCloneBuildingBlock<TBuildingBlock>(string type, string methodName) where TBuildingBlock : class, IBuildingBlock
      {
         var buildingBlock = _pkSimLoader.ExecuteMethod(type, methodName) as TBuildingBlock;

         //in case of cancelling
         if (buildingBlock == null)
            return null;

         return _cloneManager.Clone(buildingBlock);
      }

      public IReadOnlyList<ExpressionParameterValueUpdate> UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock expressionProfile)
      {
         return _pkSimLoader.ExecuteMethod(PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR, GET_EXPRESSION_DATABASE_QUERY, [expressionProfile]) as List<ExpressionParameterValueUpdate>;
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

      private string retrievePKSimAssemblyPath()
      {
         var pkSimPath = prioritizedPKSimPath();
         return Path.Combine(Path.GetDirectoryName(pkSimPath) ?? string.Empty, PKSIM_UI_STARTER_DLL);
      }

      private string retrievePKSimExecutablePath()
      {
         var prioritizedPath = prioritizedPKSimPath();

         if (!FileHelper.FileExists(prioritizedPath))
            throw new MoBiException(AppConstants.PKSim.NotInstalled);

         return prioritizedPath;
      }

      private string prioritizedPKSimPath()
      {
         // Specified explicitly via user settings? Use overridden path
         if (FileHelper.FileExists(_applicationSettings.PKSimPath))
            return _applicationSettings.PKSimPath;

         return _configuration.PKSimPath;
      }
   }
}