using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class MoBiSimulationXmlSerializer : SimulationXmlSerializer<MoBiSimulation>
   {
      public MoBiSimulationXmlSerializer() : base(SerializationConstants.MoBiSimulation)
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Results);
         Map(x => x.ParameterIdentificationWorkingDirectory);
         Map(x => x.HasUpToDateResults);
      }

      public override MoBiSimulation CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new MoBiSimulation {DiagramManager = serializationContext.Resolve<ISimulationDiagramManager>()};
      }

      protected override void TypedDeserialize(MoBiSimulation simulation, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(simulation, outputToDeserialize, serializationContext);

         if (simulation.Results != null)
            serializationContext.AddRepository(simulation.Results);

         var chartSerializer = SerializerRepository.SerializerFor<CurveChart>();
         var chartElement = outputToDeserialize.Element(chartSerializer.ElementName);
         if (chartElement != null)
            simulation.Chart = chartSerializer.Deserialize<ICurveChart>(outputToDeserialize.Element(chartSerializer.ElementName), serializationContext);

         var diagramSerializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();

         var diagramElement = outputToDeserialize.Element(diagramSerializer.ElementName);
         if (diagramElement != null)
            simulation.DiagramModel = diagramSerializer.XmlDocumentToDiagramModel(diagramElement.ToXmlDocument());
      }

      protected override XElement TypedSerialize(MoBiSimulation simulation, SerializationContext serializationContext)
      {
         var xElement = base.TypedSerialize(simulation, serializationContext);
         var chartSerializer = SerializerRepository.SerializerFor<CurveChart>();

         var diagramSerializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();

         if (simulation.DiagramModel != null)
            xElement.Add(diagramSerializer.DiagramModelToXmlDocument(simulation.DiagramModel).ToXElement());

         if (simulation.Chart != null)
            xElement.Add(chartSerializer.Serialize(simulation.Chart, serializationContext));

         return xElement;
      }
   }
}