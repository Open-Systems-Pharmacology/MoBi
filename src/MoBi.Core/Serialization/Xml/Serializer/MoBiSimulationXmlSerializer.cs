using System.Xml.Linq;
using MoBi.Core.Chart;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class MoBiSimulationXmlSerializer : SimulationXmlSerializer<MoBiSimulation>
   {
      private const string _analysesElement = "Analyses";

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
         Map(x => x.HasUntraceableChanges);
         MapEnumerable(x => x.OriginalQuantityValues, x => x.AddOriginalQuantityValue);
      }

      public override MoBiSimulation CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new MoBiSimulation { DiagramManager = serializationContext.Resolve<ISimulationDiagramManager>() };
      }

      protected override void TypedDeserialize(MoBiSimulation simulation, XElement simulationElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(simulation, simulationElement, serializationContext);

         if (simulation.ResultsDataRepository != null)
            serializationContext.AddRepository(simulation.ResultsDataRepository);

         var analysesElement = simulationElement.Element(_analysesElement);
         if (analysesElement != null)
            deserializeAnalysesFrom(simulation, analysesElement, serializationContext);

         var diagramSerializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();

         var diagramElement = simulationElement.Element(diagramSerializer.ElementName);
         if (diagramElement != null)
            simulation.DiagramModel = diagramSerializer.XmlDocumentToDiagramModel(diagramElement.ToXmlDocument());
      }

      private void deserializeAnalysesFrom(MoBiSimulation simulation, XElement analysesElement, SerializationContext serializationContext)
      {
         foreach (var childElement in analysesElement.Elements())
         {
            var analysis = deserializeAnalysis(childElement, serializationContext);
            if (analysis != null)
               simulation.AddAnalysis(analysis);
         }
      }

      private ISimulationAnalysis deserializeAnalysis(XElement element, SerializationContext serializationContext)
      {
         return tryDeserializeChart<MoBiSimulationTimeProfileChart>(element, serializationContext)
                ?? tryDeserializeChart<SimulationPredictedVsObservedChart>(element, serializationContext)
                ?? tryDeserializeChart<SimulationResidualVsTimeChart>(element, serializationContext);
      }

      private ISimulationAnalysis tryDeserializeChart<T>(XElement element, SerializationContext serializationContext) where T : class, ISimulationAnalysis
      {
         var serializer = SerializerRepository.SerializerFor<T>();
         if (!string.Equals(element.Name.LocalName, serializer.ElementName))
            return null;

         return serializer.Deserialize<T>(element, serializationContext);
      }

      protected override XElement TypedSerialize(MoBiSimulation simulation, SerializationContext serializationContext)
      {
         var simulationElement = base.TypedSerialize(simulation, serializationContext);

         var diagramSerializer = serializationContext.Resolve<IDiagramModelToXmlMapper>();

         if (simulation.DiagramModel != null)
            simulationElement.Add(diagramSerializer.DiagramModelToXmlDocument(simulation.DiagramModel).ToXElement());

         // Serialize all analyses into an <Analyses> wrapper element
         var analysesElement = new XElement(_analysesElement);
         foreach (var analysis in simulation.Analyses)
         {
            addSerializedAnalysis(analysesElement, analysis, serializationContext);
         }

         simulationElement.Add(analysesElement);

         return simulationElement;
      }

      private void addSerializedAnalysis(XElement parentElement, ISimulationAnalysis analysis, SerializationContext serializationContext)
      {
         switch (analysis)
         {
            case MoBiSimulationTimeProfileChart chart:
               addSerializedChart(parentElement, chart, serializationContext);
               break;
            case SimulationPredictedVsObservedChart chart:
               addSerializedChart(parentElement, chart, serializationContext);
               break;
            case SimulationResidualVsTimeChart chart:
               addSerializedChart(parentElement, chart, serializationContext);
               break;
         }
      }

      private void addSerializedChart<T>(XElement parentElement, T chart, SerializationContext serializationContext) where T : class
      {
         if (chart == null)
            return;

         var chartSerializer = SerializerRepository.SerializerFor<T>();
         parentElement.Add(chartSerializer.Serialize(chart, serializationContext));
      }
   }
}
