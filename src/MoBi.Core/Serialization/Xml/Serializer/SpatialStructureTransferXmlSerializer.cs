using MoBi.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class SpatialStructureTransferXmlSerializer : OSPSuiteXmlSerializer<SpatialStructureTransfer>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.SpatialStructure);
         Map(x => x.ParameterValues);
      }
   }
}