using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using CoreConverter121To130 = OSPSuite.Core.Converters.v13.Converter121To130;

namespace MoBi.Core.Serialization.Converter.v13;

public class Converter121To130 : IMoBiObjectConverter, IVisitor<MoBiProject>
{
   private readonly CoreConverter121To130 _coreConverter;

   public Converter121To130(CoreConverter121To130 coreConverter)
   {
      _coreConverter = coreConverter;
   }

   public bool IsSatisfiedBy(int version) => version == ProjectVersions.V12_1;

   public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project)
   {
      (_, bool converted) = _coreConverter.Convert(objectToUpdate);
      this.Visit(objectToUpdate);
      return (ProjectVersions.V13_0, converted);
   }

   public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project)
   {
      (_, bool converted) = _coreConverter.ConvertXml(element);

      element.DescendantsAndSelfNamed("MoBiSimulation").Each(simulationNode =>
      {
         convertLegacyChartsToAnalyses(simulationNode);
         converted = true;
      });

      return (ProjectVersions.V13_0, converted);
   }

   public void Visit(MoBiProject moBiProject)
   {
      if(moBiProject.SimulationSettings != null)
         _coreConverter.Visit(moBiProject.SimulationSettings);
   }

   private void convertLegacyChartsToAnalyses(XElement simulationNode)
   {
      if (simulationNode.Element("Analyses") != null)
         return;

      var analysesElement = new XElement("Analyses");

      var curveChart = simulationNode.Element("CurveChart");
      if (curveChart != null)
      {
         curveChart.Remove();
         curveChart.Name = "MoBiSimulationTimeProfileChart";
         analysesElement.Add(curveChart);
      }

      moveElementIfExists(simulationNode, analysesElement, "SimulationPredictedVsObservedChart");
      moveElementIfExists(simulationNode, analysesElement, "SimulationResidualVsTimeChart");

      simulationNode.Add(analysesElement);
   }

   private void moveElementIfExists(XElement source, XElement target, string elementName)
   {
      var element = source.Element(elementName);
      if (element != null)
      {
         element.Remove();
         target.Add(element);
      }
   }
}