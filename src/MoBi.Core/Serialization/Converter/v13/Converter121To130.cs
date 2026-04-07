using System.Xml.Linq;
using MoBi.Core.Chart;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using CoreConverter121To130 = OSPSuite.Core.Converters.v13.Converter121To130;

namespace MoBi.Core.Serialization.Converter.v13;

public class Converter121To130 : IMoBiObjectConverter, IVisitor<MoBiProject>
{
   private readonly CoreConverter121To130 _coreConverter;
   private readonly IObjectTypeResolver _objectTypeResolver;

   public Converter121To130(CoreConverter121To130 coreConverter, IObjectTypeResolver objectTypeResolver)
   {
      _coreConverter = coreConverter;
      _objectTypeResolver = objectTypeResolver;
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
         converted |= convertLegacyChartsToAnalyses(simulationNode);
      });

      return (ProjectVersions.V13_0, converted);
   }

   public void Visit(MoBiProject moBiProject)
   {
      if(moBiProject.SimulationSettings != null)
         _coreConverter.Visit(moBiProject.SimulationSettings);
   }

   private bool convertLegacyChartsToAnalyses(XElement simulationNode)
   {
      if (simulationNode.Element("Analyses") != null)
         return false;

      var analysesElement = new XElement("Analyses");

      var curveChart = simulationNode.Element("CurveChart");
      if (curveChart != null)
      {
         curveChart.Remove();
         curveChart.Name = "MoBiSimulationTimeProfileChart";
         setDefaultNameIfMissing(curveChart);
         analysesElement.Add(curveChart);
      }

      moveElementIfExists(simulationNode, analysesElement, "SimulationPredictedVsObservedChart");
      moveElementIfExists(simulationNode, analysesElement, "SimulationResidualVsTimeChart");

      simulationNode.Add(analysesElement);
      return true;
   }

   private void moveElementIfExists(XElement source, XElement target, string elementName)
   {
      var element = source.Element(elementName);
      if (element != null)
      {
         element.Remove();
         setDefaultNameIfMissing(element);
         target.Add(element);
      }
   }

   private void setDefaultNameIfMissing(XElement element)
   {
      if (!string.IsNullOrEmpty(element.Attribute("name")?.Value))
         return;

      // No need to check for unique names because before now a simulation could only have one of each type
      var defaultName = defaultNameFor(element.Name.LocalName);
      if (defaultName != null)
         element.AddAttribute("name", defaultName);
   }

   private string defaultNameFor(string elementName) => elementName switch
   {
      nameof(MoBiSimulationTimeProfileChart) => _objectTypeResolver.TypeFor<MoBiSimulationTimeProfileChart>(),
      nameof(SimulationPredictedVsObservedChart) => _objectTypeResolver.TypeFor<SimulationPredictedVsObservedChart>(),
      nameof(SimulationResidualVsTimeChart) => _objectTypeResolver.TypeFor<SimulationResidualVsTimeChart>(),
      _ => null
   };
}