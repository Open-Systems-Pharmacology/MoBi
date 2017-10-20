using System.Xml.Linq;
using MoBi.Core;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   public class ApplicationSettingsXmlSerializer : XmlSerializer<ApplicationSettings, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.PKSimPath);
         Map(x => x.UseWatermark);
         Map(x => x.WatermarkText);
      }

      public override ApplicationSettings CreateObject(XElement element, SerializationContext serializationContext)
      {
         return serializationContext.Resolve<IApplicationSettings>().DowncastTo<ApplicationSettings>();
      }
   }
}