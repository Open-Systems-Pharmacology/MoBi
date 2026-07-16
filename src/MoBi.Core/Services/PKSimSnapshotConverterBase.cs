using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Qualification;

namespace MoBi.Core.Services;

public interface IPKSimSnapshotConverter
{
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

/// <summary>
///    Shared implementation of the PK-Sim snapshot exchange. All exchanges are performed by
///    reflecting into a PK-Sim assembly via <see cref="IPKSimAssemblyLoader" /> and recreating the
///    returned objects in MoBi through serialization. Concrete subclasses differ only in:
///    <list type="bullet">
///       <item>which assembly the loader is pointed at (initialized in their constructor), and</item>
///       <item><see cref="SnapshotExchangeType" /> - the fully-qualified name of the snapshot-exchange
///       type that exposes the <c>Create...</c> methods.</item>
///    </list>
///    The desktop app uses <c>PKSim.Starter.SnapshotExchange</c> (in PKSim.Starter.dll); MoBi.R uses
///    <c>PKSim.R.Exchange.SnapshotExchange</c> (in the shippable PKSim.R.dll).
/// </summary>
public abstract class PKSimSnapshotConverterBase : IPKSimSnapshotConverter
{
   protected const string CREATE_MODULE = "CreateModule";
   protected const string CREATE_MODULE_AND_EXPORT_INPUTS = "CreateModuleAndExportInputs";
   protected const string CREATE_INDIVIDUAL_BUILDING_BLOCK = "CreateIndividualBuildingBlock";
   protected const string CREATE_EXPRESSION_PROFILE_BUILDING_BLOCK = "CreateExpressionProfileBuildingBlock";

   protected readonly IPKSimAssemblyLoader _pkSimLoader;
   private readonly IXmlSerializationService _serializationService;
   private readonly IMoBiProjectRetriever _projectRetriever;

   protected PKSimSnapshotConverterBase(IPKSimAssemblyLoader pkSimLoader, IXmlSerializationService serializationService, IMoBiProjectRetriever projectRetriever)
   {
      _pkSimLoader = pkSimLoader;
      _serializationService = serializationService;
      _projectRetriever = projectRetriever;
   }

   /// <summary>
   ///    Fully-qualified name of the PK-Sim type exposing the <c>Create...</c> exchange methods
   ///    (e.g. <c>PKSim.Starter.SnapshotExchange</c> or <c>PKSim.R.Exchange.SnapshotExchange</c>).
   /// </summary>
   protected abstract string SnapshotExchangeType { get; }

   public Module LoadModuleFromSnapshot(string serializedSnapshot)
   {
      var element = _pkSimLoader.ExecuteMethod(SnapshotExchangeType, CREATE_MODULE, [serializedSnapshot]) as string;

      // all object exchanges should be done using serialization to ensure that objects are recreated in MoBi.
      return _serializationService.Deserialize<Module>(element, _projectRetriever.Current);
   }

   public (Module module, InputMapping[] inputMappings) LoadModuleFromSnapshotAndExportInputs(string serializedSnapshot, QualificationConfiguration qualificationConfiguration)
   {
      var tuple = _pkSimLoader.ExecuteMethod(SnapshotExchangeType, CREATE_MODULE_AND_EXPORT_INPUTS, [serializedSnapshot, qualificationConfiguration]);

      var (element, mappings) = tuple as (string element, InputMapping[] mappings)? ?? (null, null);

      // all object exchanges should be done using serialization to ensure that objects are recreated in MoBi.
      var module = _serializationService.Deserialize<Module>(element, _projectRetriever.Current);

      return (module, mappings);
   }

   public ExpressionProfileBuildingBlock LoadExpressionProfileFromSnapshot(string serializedSnapshot)
   {
      var pkml = _pkSimLoader.ExecuteMethod(SnapshotExchangeType, CREATE_EXPRESSION_PROFILE_BUILDING_BLOCK, [serializedSnapshot]) as string;

      // all object exchanges should be done using serialization to ensure that objects are recreated in MoBi.
      return _serializationService.Deserialize<ExpressionProfileBuildingBlock>(pkml, _projectRetriever.Current);
   }

   public IndividualBuildingBlock LoadIndividualFromSnapshot(string serializedSnapshot)
   {
      var pkml = _pkSimLoader.ExecuteMethod(SnapshotExchangeType, CREATE_INDIVIDUAL_BUILDING_BLOCK, [serializedSnapshot]) as string;

      // all object exchanges should be done using serialization to ensure that objects are recreated in MoBi.
      return _serializationService.Deserialize<IndividualBuildingBlock>(pkml, _projectRetriever.Current);
   }
}