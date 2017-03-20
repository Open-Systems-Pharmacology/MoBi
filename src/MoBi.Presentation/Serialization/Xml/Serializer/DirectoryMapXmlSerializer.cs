using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   public class DirectoryMapXmlSerializer : XmlSerializer<DirectoryMap, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.Key);
         Map(x => x.Path);
      }
   }
}