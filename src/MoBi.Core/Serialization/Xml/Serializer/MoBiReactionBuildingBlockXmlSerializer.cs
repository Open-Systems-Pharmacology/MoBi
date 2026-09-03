using System.Xml.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   internal class MoBiReactionBuildingBlockXmlSerializer : ReactionBuildingBlockXmlSerializerBase<MoBiReactionBuildingBlock>
   {
      public MoBiReactionBuildingBlockXmlSerializer() : base(AppConstants.XmlNames.ReactionBuildingBlock)
      {
      }

      public override MoBiReactionBuildingBlock CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new MoBiReactionBuildingBlock {DiagramManager = serializationContext.Resolve<IMoBiReactionDiagramManager>()};
      }

      protected override void TypedDeserialize(MoBiReactionBuildingBlock reactionBuildingBlock, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(reactionBuildingBlock, outputToDeserialize, serializationContext);
         var serializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();
         var diagramElement = outputToDeserialize.Element(serializer.ElementName);

         if (diagramElement == null) return;
         var xmlDoc = diagramElement.ToXmlDocument();
         var model = serializationContext.Resolve<IReactionDiagramModelFactory>().Create();
         serializer.Deserialize(model, xmlDoc);
         reactionBuildingBlock.DiagramModel = model;
      }

      protected override XElement TypedSerialize(MoBiReactionBuildingBlock reactionBuildingBlock, SerializationContext serializationContext)
      {
         var xElement = base.TypedSerialize(reactionBuildingBlock, serializationContext);
         var serializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();
         if (reactionBuildingBlock.DiagramModel != null)
         {
            var xmlDoc = serializer.DiagramModelToXmlDocument(reactionBuildingBlock.DiagramModel);
            xElement.Add(xmlDoc.ToXElement());
         }
         return xElement;
      }
   }
}