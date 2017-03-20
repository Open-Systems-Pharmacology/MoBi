using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Diagram.Elements;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   public class DiagramOptionsXmlSerializer : XmlSerializer<IDiagramOptions, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(options => options.SnapGridVisible);
         Map(options => options.MoleculePropertiesVisible);
         Map(options => options.ObserverLinksVisible);
         Map(options => options.UnusedMoleculesVisibleInModelDiagram);
         Map(options => options.DefaultNodeSizeReaction);
         Map(options => options.DefaultNodeSizeMolecule);
         Map(options => options.DefaultNodeSizeObserver);
         Map(options => options.DiagramColors);
      }

      public override IDiagramOptions CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new DiagramOptions();
      }
   }
}