using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   public class DirectoryMapSettingsXmlSerializer : XmlSerializer<DirectoryMapSettings, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.UsedDirectories, x => x.UsedDirectories.Add);
      }
   }
}
