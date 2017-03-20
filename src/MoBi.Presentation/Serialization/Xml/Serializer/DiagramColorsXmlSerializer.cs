using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Diagram.Elements;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   public class DiagramColorsXmlSerializer : XmlSerializer<IDiagramColors, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(colors => colors.DiagramBackground);
         Map(colors => colors.BorderFixed);
         Map(colors => colors.BorderUnfixed);
         Map(colors => colors.ContainerLogical);
         Map(colors => colors.ContainerPhysical);
         Map(colors => colors.ContainerOpacity);
         Map(colors => colors.ContainerBorder);
         Map(colors => colors.ContainerHandle);
         Map(colors => colors.NodeSizeOpacity);
         Map(colors => colors.PortOpacity);
         Map(colors => colors.NeighborhoodNode);
         Map(colors => colors.NeighborhoodLink);
         Map(colors => colors.NeighborhoodPort);
         Map(colors => colors.TransportLink);
         Map(colors => colors.MoleculeNode);
         Map(colors => colors.ObserverNode);
         Map(colors => colors.ObserverLink);
         Map(colors => colors.ReactionNode);
         Map(colors => colors.ReactionPortEduct);
         Map(colors => colors.ReactionLinkEduct);
         Map(colors => colors.ReactionPortProduct);
         Map(colors => colors.ReactionLinkProduct);
         Map(colors => colors.ReactionPortModifier);
         Map(colors => colors.ReactionLinkModifier);
      }

      public override IDiagramColors CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new DiagramColors();
      }
   }
}