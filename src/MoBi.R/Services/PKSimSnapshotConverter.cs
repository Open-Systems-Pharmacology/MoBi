using System.IO;
using System.Reflection;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;

namespace MoBi.R.Services
{
   /// <summary>
   ///    Headless PK-Sim snapshot converter used by MoBi.R. It reflects into
   ///    <c>PKSim.R.Exchange.SnapshotExchange</c> shipped in the co-located <c>PKSim.R.dll</c>, so it
   ///    works without a PK-Sim desktop installation and without the UI/Presentation assemblies that
   ///    <c>PKSim.Starter.dll</c> depends on.
   /// </summary>
   public class PKSimSnapshotConverter : PKSimSnapshotConverterBase
   {
      private const string PKSIM_R_DLL = "PKSim.R.dll";
      private const string PKSIM_R_SNAPSHOT_EXCHANGE = "PKSim.R.Exchange.SnapshotExchange";

      public PKSimSnapshotConverter(IXmlSerializationService serializationService, IMoBiProjectRetriever projectRetriever, IPKSimAssemblyLoader pkSimLoader)
         : base(pkSimLoader, serializationService, projectRetriever)
      {
         _pkSimLoader.InitializePath(pkSimAssemblyPath());
      }

      protected override string SnapshotExchangeType => PKSIM_R_SNAPSHOT_EXCHANGE;

      // PKSim.R.dll is shipped next to the MoBi.R assembly (e.g. the R package's inst/lib folder).
      private static string pkSimAssemblyPath() =>
         Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, PKSIM_R_DLL);
   }
}
