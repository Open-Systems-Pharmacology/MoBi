using MoBi.Assets;
using MoBi.Core.Exceptions;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public interface IPKSimStarter
   {
      void StartPopulationSimulationWithSimulationFile(string simulationFilePath);
      void StartWithWorkingJournalFile(string journalFilePath);
      IBuildingBlock CreateProfileExpression(ExpressionType expressionType);
      IBuildingBlock CreateIndividual();
      IReadOnlyList<ExpressionParameterValueUpdate> UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock expressionProfile);
      SimulationTransfer LoadSimulationTransferFromSnapshot(string serializedSnapshot);
   }

   public class PKSimStarter : IPKSimStarter
   {
      private const string CREATE_INDIVIDUAL_ENZYME_EXPRESSION_PROFILE = "CreateIndividualEnzymeExpressionProfile";
      private const string CREATE_BINDING_PARTNER_EXPRESSION_PROFILE = "CreateBindingPartnerExpressionProfile";
      private const string CREATE_TRANSPORTER_EXPRESSION_PROFILE = "CreateTransporterExpressionProfile";
      private const string CREATE_INDIVIDUAL = "CreateIndividual";
      private const string CREATE_SIMULATION_TRANSFER = "CreateSimulationTransfer";
      private const string GET_EXPRESSION_DATABASE_QUERY = "GetExpressionDatabaseQuery";
      private const string PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR = "PKSim.UI.Starter.ExpressionProfileCreator";
      private const string PKSIM_UI_STARTER_INDIVIDUAL_CREATOR = "PKSim.UI.Starter.IndividualCreator";
      private const string PKSIM_UI_STARTER_DLL = "PKSim.UI.Starter.dll";
      private const string PKSIM_UI_STARTER_SIMULATION_TRANSFER_CONSTRUCTOR = "PKSim.UI.Starter.SimulationTransferConstructor";

      private readonly IMoBiConfiguration _configuration;
      private readonly IApplicationSettings _applicationSettings;
      private readonly IStartableProcessFactory _startableProcessFactory;
      private Assembly _externalAssembly;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IXmlSerializationService _serializationService;

      public PKSimStarter(IMoBiConfiguration configuration,
         IApplicationSettings applicationSettings,
         IStartableProcessFactory startableProcessFactory,
         ICloneManagerForBuildingBlock cloneManager,
         IXmlSerializationService serializationService,
         IMoBiProjectRetriever projectRetriever)
      {
         _configuration = configuration;
         _applicationSettings = applicationSettings;
         _startableProcessFactory = startableProcessFactory;
         _cloneManager = cloneManager;
         _serializationService = serializationService;
         _projectRetriever = projectRetriever;
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

         return _cloneManager.Clone(buildingBlock);
      }

      public IReadOnlyList<ExpressionParameterValueUpdate> UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock expressionProfile)
      {
         loadPKSimAssembly();
         return executeMethod(getMethod(PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR, GET_EXPRESSION_DATABASE_QUERY), [expressionProfile]) as List<ExpressionParameterValueUpdate>;
      }

      public SimulationTransfer LoadSimulationTransferFromSnapshot(string serializedSnapshot)
      {
         var element = executeMethod(getMethod(PKSIM_UI_STARTER_SIMULATION_TRANSFER_CONSTRUCTOR, CREATE_SIMULATION_TRANSFER), [serializedSnapshot]) as string;
         var transfer = _serializationService.Deserialize<SimulationTransfer>(element, _projectRetriever.Current);
         var simulationConfiguration = transfer.Simulation.Configuration;

         var module = simulationConfiguration.ModuleConfigurations.Single().Module;
         simulationConfiguration.ExpressionProfiles.Each(x => x.SnapshotOriginModuleId = module.Id);
         simulationConfiguration.Individual.SnapshotOriginModuleId = module.Id;
         return transfer;
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