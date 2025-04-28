using System.Xml;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Serialization.Diagram;

namespace MoBi.R.MinimalImplementations
{
   public class DiagramModelToXmlMapper : IDiagramModelToXmlMapper
   {
      public string ElementName => null;

      public void AddElementBaseNodeBindingFor<T>(T node)
      {
      }

      public void Deserialize(IDiagramModel model, XmlDocument xmlDoc)
      {
      }

      public XmlDocument DiagramModelToXmlDocument(IDiagramModel diagramModel)
      {
         return new XmlDocument();
      }

      public IDiagramModel XmlDocumentToDiagramModel(XmlDocument xmlDoc)
      {
         return new DiagramModelMinimalImplementation();
      }
   }
}