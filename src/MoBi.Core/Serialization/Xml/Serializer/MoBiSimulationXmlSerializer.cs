using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer;

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
         //for compatibility with older versions before renaming of "Results" to "ResultsDataRepository"
         Map(x => x.ResultsDataRepository).WithMappingName(SerializationConstants.MoBiResults);
         Map(x => x.ParameterIdentificationWorkingDirectory);
         Map(x => x.HasUpToDateResults);
         Map(x => x.OutputMappings);
      }

      public override MoBiSimulation CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new MoBiSimulation {DiagramManager = serializationContext.Resolve<ISimulationDiagramManager>()};
      }

      protected override void TypedDeserialize(MoBiSimulation simulation, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(simulation, outputToDeserialize, serializationContext);

         if (simulation.ResultsDataRepository != null)
            serializationContext.AddRepository(simulation.ResultsDataRepository);

         var chartSerializer = SerializerRepository.SerializerFor<CurveChart>();
         var chartElement = outputToDeserialize.Element(chartSerializer.ElementName);
         if (chartElement != null)
            simulation.Chart = chartSerializer.Deserialize<CurveChart>(outputToDeserialize.Element(chartSerializer.ElementName), serializationContext);

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