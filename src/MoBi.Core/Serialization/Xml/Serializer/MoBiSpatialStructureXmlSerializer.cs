using System.Xml.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   internal class MoBiSpatialStructureXmlSerializer : SpatialStructureXmlSerializerBase<MoBiSpatialStructure>
   {
      public MoBiSpatialStructureXmlSerializer() : base(AppConstants.XmlNames.SpatialStructure)
      {
      }

      public override MoBiSpatialStructure CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new MoBiSpatialStructure {DiagramManager = serializationContext.Resolve<ISpatialStructureDiagramManager>()};
      }

      protected override void TypedDeserialize(MoBiSpatialStructure spatialStructure, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(spatialStructure, outputToDeserialize, serializationContext);
         var serializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();

         var diagramElement = outputToDeserialize.Element(serializer.ElementName);
         if (diagramElement == null) return;

         var xmlDoc = diagramElement.ToXmlDocument();
         spatialStructure.DiagramModel = serializer.XmlDocumentToDiagramModel(xmlDoc);
      }

      protected override XElement TypedSerialize(MoBiSpatialStructure spatialStructure, SerializationContext serializationContext)
      {
         var xElement = base.TypedSerialize(spatialStructure, serializationContext);
         var serializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();
         if (spatialStructure.DiagramModel != null)
         {
            var xmlDoc = serializer.DiagramModelToXmlDocument(spatialStructure.DiagramModel);
            xElement.Add(xmlDoc.ToXElement());
         }
         return xElement;
      }
   }
}