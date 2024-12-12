using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Converters.v12;
using OSPSuite.Core.Serialization.Xml.Extensions;
using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Serialization.Converter.v12
{
   public class Converter11To12 : IMoBiObjectConverter
   {
      private readonly Converter110To120 _coreConverter;

      public Converter11To12(Converter110To120 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version) => version == ProjectVersions.V11_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project)
      {
         return _coreConverter.Convert(objectToUpdate);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project)
      {
         var (convertedToVersion, conversionHappened) = _coreConverter.ConvertXml(element);

         element.DescendantsAndSelfNamed("MoBiSimulation").Each(simulationNode =>
         {
            var simulationChanges = simulationNode.DescendantsAndSelfNamed("BuildConfiguration").Any(hasSimulationChanges);
            if (simulationChanges)
               simulationNode.AddAttribute("hasUntraceableChanges", true.ToString());

            _coreConverter.ConvertSimulation(simulationNode);
            conversionHappened = true;
         });
         return (convertedToVersion, conversionHappened);
      }

      private bool hasSimulationChanges(XElement buildConfiguration)
      {
         var simulationHasChanges = false;
         simulationHasChanges |= buildConfiguration.DescendantsAndSelfNamed("SpatialStructureInfo").Any(infoNodeHasSimulationChanges);
         simulationHasChanges |= buildConfiguration.DescendantsAndSelfNamed("MoleculesInfo").Any(infoNodeHasSimulationChanges);
         simulationHasChanges |= buildConfiguration.DescendantsAndSelfNamed("ReactionsInfo").Any(infoNodeHasSimulationChanges);
         simulationHasChanges |= buildConfiguration.DescendantsAndSelfNamed("PassiveTransportsInfo").Any(infoNodeHasSimulationChanges);
         simulationHasChanges |= buildConfiguration.DescendantsAndSelfNamed("ObserversInfo").Any(infoNodeHasSimulationChanges);
         simulationHasChanges |= buildConfiguration.DescendantsAndSelfNamed("EventGroupsInfo").Any(infoNodeHasSimulationChanges);
         simulationHasChanges |= buildConfiguration.DescendantsAndSelfNamed("ParameterStartValuesInfo").Any(infoNodeHasSimulationChanges);
         simulationHasChanges |= buildConfiguration.DescendantsAndSelfNamed("MoleculeStartValuesInfo").Any(infoNodeHasSimulationChanges);

         return simulationHasChanges;
      }

      private bool infoNodeHasSimulationChanges(XElement infoNode)
      {
         return !infoNode.GetAttribute("simulationChanges").Equals("0");
      }
   }
}