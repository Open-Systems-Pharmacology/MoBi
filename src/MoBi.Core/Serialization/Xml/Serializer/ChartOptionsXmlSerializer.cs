using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Serialization.Xml;


namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class ChartOptionsXmlSerializer : XmlSerializer<ChartOptions, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(options => options.SimulationInCurveName);
         Map(options => options.TopContainerInCurveName);
         Map(options => options.DefaultChartYScaling);
         Map(options => options.DefaultChartBackColor);
         Map(options => options.DefaultChartDiagramBackColor);
      }

      public override ChartOptions CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new ChartOptions();
      }
   }
}