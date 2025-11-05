using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MoBi.Assets;
using MoBi.Core.Exceptions;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Qualification;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using Module = OSPSuite.Core.Domain.Module;

namespace MoBi.Core.Services
{
   public interface IPKSimStarter
   {
      void StartPopulationSimulationWithSimulationFile(string simulationFilePath);
      void StartWithWorkingJournalFile(string journalFilePath);
      IBuildingBlock CreateProfileExpression(ExpressionType expressionType);
      IBuildingBlock CreateIndividual();
      IReadOnlyList<ExpressionParameterValueUpdate> UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock expressionProfile);
      Module LoadModuleFromSnapshot(string serializedSnapshot);

      /// <summary>
      ///    Recreates the PKSim module from the snapshot.
      ///    If any of the PK-Sim building blocks should have markdown exported it will be done during the module rebuild.
      /// </summary>
      /// <param name="serializedSnapshot">The PK-Sim project snapshot</param>
      /// <param name="qualificationConfiguration">
      ///    The configuration containing any building block inputs that should have report
      ///    markdown exported while the module is rebuilt in PK-Sim
      /// </param>
      /// <returns>A module and any input mappings that were exported</returns>
      (Module module, InputMapping[] inputMappings) LoadModuleFromSnapshotAndExportInputs(string serializedSnapshot, QualificationConfiguration qualificationConfiguration);

      /// <summary>
      ///    Recreates a PK-Sim expression profile from the base64 encoded <paramref name="serializedSnapshot" />.
      /// </summary>
      /// <returns>The expression profile building block, as created in Pk-Sim</returns>
      ExpressionProfileBuildingBlock LoadExpressionProfileFromSnapshot(string serializedSnapshot);

      /// <summary>
      ///    Recreates a PK-Sim individual from the base64 encoded <paramref name="serializedSnapshot" />.
      /// </summary>
      /// <returns>The individual building block, as created in Pk-Sim</returns>
      IndividualBuildingBlock LoadIndividualFromSnapshot(string serializedSnapshot);
   }

   public class PKSimStarter : IPKSimStarter
   {
      private const string CREATE_INDIVIDUAL_ENZYME_EXPRESSION_PROFILE = "CreateIndividualEnzymeExpressionProfile";
      private const string CREATE_BINDING_PARTNER_EXPRESSION_PROFILE = "CreateBindingPartnerExpressionProfile";
      private const string CREATE_TRANSPORTER_EXPRESSION_PROFILE = "CreateTransporterExpressionProfile";
      private const string CREATE_INDIVIDUAL = "CreateIndividual";
      private const string CREATE_MODULE = "CreateModule";
      private const string CREATE_MODULE_AND_EXPORT_INPUTS = "CreateModuleAndExportInputs";
      private const string CREATE_INDIVIDUAL_BUILDING_BLOCK = "CreateIndividualBuildingBlock";
      private const string CREATE_EXPRESSION_PROFILE_BUILDING_BLOCK = "CreateExpressionProfileBuildingBlock";
      private const string GET_EXPRESSION_DATABASE_QUERY = "GetExpressionDatabaseQuery";
      private const string PKSIM_UI_STARTER_EXPRESSION_PROFILE_CREATOR = "PKSim.Starter.ExpressionProfileCreator";
      private const string PKSIM_UI_STARTER_INDIVIDUAL_CREATOR = "PKSim.Starter.IndividualCreator";
      private const string PKSIM_UI_STARTER_DLL = "PKSim.Starter.dll";
      private const string PKSIM_UI_STARTER_SNAPSHOT_EXCHANGE = "PKSim.Starter.SnapshotExchange";

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

      public Module LoadModuleFromSnapshot(string serializedSnapshot)
      {
         var element = executeMethod(getMethod(PKSIM_UI_STARTER_SNAPSHOT_EXCHANGE, CREATE_MODULE), [serializedSnapshot]) as string;

         // all object exchanges should be done using serialization to ensure that objects are recreated in MoBi.
         return _serializationService.Deserialize<Module>(element, _projectRetriever.Current);
      }

      public (Module module, InputMapping[] inputMappings) LoadModuleFromSnapshotAndExportInputs(string serializedSnapshot, QualificationConfiguration qualificationConfiguration)
      {
         var tuple = executeMethod(getMethod(PKSIM_UI_STARTER_SNAPSHOT_EXCHANGE, CREATE_MODULE_AND_EXPORT_INPUTS), [serializedSnapshot, qualificationConfiguration]);

         var (element, mappings) = tuple as (string element, InputMapping[] mappings)? ?? (null, null);

         // all object exchanges should be done using serialization to ensure that objects are recreated in MoBi.
         var module = _serializationService.Deserialize<Module>(element, _projectRetriever.Current);

         return (module, mappings);
      }

      public ExpressionProfileBuildingBlock LoadExpressionProfileFromSnapshot(string serializedSnapshot)
      {
         var pkml = executeMethod(getMethod(PKSIM_UI_STARTER_SNAPSHOT_EXCHANGE, CREATE_EXPRESSION_PROFILE_BUILDING_BLOCK), [serializedSnapshot]) as string;

         // all object exchanges should be done using serialization to ensure that objects are recreated in MoBi.
         return _serializationService.Deserialize<ExpressionProfileBuildingBlock>(pkml, _projectRetriever.Current);
      }

      public IndividualBuildingBlock LoadIndividualFromSnapshot(string serializedSnapshot)
      {
         var pkml = executeMethod(getMethod(PKSIM_UI_STARTER_SNAPSHOT_EXCHANGE, CREATE_INDIVIDUAL_BUILDING_BLOCK), [serializedSnapshot]) as string;

         // all object exchanges should be done using serialization to ensure that objects are recreated in MoBi.
         return _serializationService.Deserialize<IndividualBuildingBlock>(pkml, _projectRetriever.Current);
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
         // Specified explicitly via user settings? Use overridden path
         if (FileHelper.FileExists(_applicationSettings.PKSimPath))
            return _applicationSettings.PKSimPath;

         // Installed properly via Setup? Use standard path
         if (FileHelper.FileExists(_configuration.PKSimPath))
            return _configuration.PKSimPath;

         throw new MoBiException(AppConstants.PKSim.NotInstalled);
      }
   }
}