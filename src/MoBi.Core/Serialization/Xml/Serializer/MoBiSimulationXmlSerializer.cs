using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
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
         MapEnumerable(x => x.OriginalQuantityValues, simulation => simulation.AddOriginalQuantityValue);
      }

      public override MoBiSimulation CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new MoBiSimulation {DiagramManager = serializationContext.Resolve<ISimulationDiagramManager>()};
      }

      protected override void TypedDeserialize(MoBiSimulation simulation, XElement simulationElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(simulation, simulationElement, serializationContext);

         if (simulation.ResultsDataRepository != null)
            serializationContext.AddRepository(simulation.ResultsDataRepository);

         simulation.Chart = deserializeChart<CurveChart>(simulationElement, serializationContext);
         simulation.PredictedVsObservedChart = deserializeChart<SimulationPredictedVsObservedChart>(simulationElement, serializationContext);
         simulation.ResidualVsTimeChart = deserializeChart<SimulationResidualVsTimeChart>(simulationElement, serializationContext);

         var diagramSerializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();

         var diagramElement = simulationElement.Element(diagramSerializer.ElementName);
         if (diagramElement != null)
            simulation.DiagramModel = diagramSerializer.XmlDocumentToDiagramModel(diagramElement.ToXmlDocument());
      }

      protected override XElement TypedSerialize(MoBiSimulation simulation, SerializationContext serializationContext)
      {
         var simulationElement = base.TypedSerialize(simulation, serializationContext);

         var diagramSerializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();

         if (simulation.DiagramModel != null)
            simulationElement.Add(diagramSerializer.DiagramModelToXmlDocument(simulation.DiagramModel).ToXElement());

         addSerializedChart(simulationElement, simulation.Chart, serializationContext);
         addSerializedChart(simulationElement, simulation.PredictedVsObservedChart, serializationContext);
         addSerializedChart(simulationElement, simulation.ResidualVsTimeChart, serializationContext);

         return simulationElement;
      }

      private T deserializeChart<T>(XElement simulationElement, SerializationContext serializationContext) where T : class
      {
         var chartSerializer = SerializerRepository.SerializerFor<T>();
         var chartElement = simulationElement.Element(chartSerializer.ElementName);
         if (chartElement == null)
            return null;

         return chartSerializer.Deserialize<T>(simulationElement.Element(chartSerializer.ElementName), serializationContext);
      }

      private void addSerializedChart<T>(XElement simulationElement, T chart, SerializationContext serializationContext) where T : class
      {
         if (chart == null)
            return;

         var chartSerializer = SerializerRepository.SerializerFor<T>();
         simulationElement.Add(chartSerializer.Serialize(chart, serializationContext));
      }
   }
}