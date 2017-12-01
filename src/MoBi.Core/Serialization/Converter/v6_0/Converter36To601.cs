using System.Xml.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Converter.v6_0;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Serialization.Converter.v6_0
{
   public class Converter36To601 : IMoBiObjectConverter,
      IVisitor<IMoBiProject>
   {
      private readonly Converter56To601 _coreConverter56To601;
      private static readonly string RHS_DIMENSION_OLD_SUFFIX = " per Time";
      private static readonly string MOL_WEIGHT = "MolWeight";
      private bool _converted;

      public Converter36To601(Converter56To601 coreConverter56To601)
      {
         _coreConverter56To601 = coreConverter56To601;
      }

      public bool IsSatisfiedBy(int version) => version == ProjectVersions.V3_6_1;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, IMoBiProject project)
      {
         _converted = false;
         var (_, coreConversionHappened) = _coreConverter56To601.Convert(objectToUpdate);
         this.Visit(objectToUpdate);
         return (ProjectVersions.V6_0_1, _converted || coreConversionHappened);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, IMoBiProject project)
      {
         _converted = false;
         var (_, coreConversionHappened) = _coreConverter56To601.ConvertXml(element);
         element.DescendantsAndSelfNamed("DisplayUnitMap").Each(displayUnitMapElement => convertPerTimeDimension(displayUnitMapElement, "dimension"));
         element.DescendantsAndSelfNamed("Formula").Each(displayUnitMapElement => convertPerTimeDimension(displayUnitMapElement, "dim"));
         element.DescendantsAndSelfNamed("Map").Each(displayUnitMapElement => convertPerTimeDimension(displayUnitMapElement, "s"));
         element.DescendantsAndSelfNamed("Parameter").Each(convertParameterQuantityType);
         return (ProjectVersions.V6_0_1, _converted || coreConversionHappened);
      }

      public void Visit(IMoBiProject project)
      {
         foreach (var observedData in project.AllObservedData)
         {
            observedData.ExtendedProperties.Remove(MOL_WEIGHT);
            updateObservedDataPaths(observedData);
            _converted = true;
         }
      }

      private void convertParameterQuantityType(XElement parameterElement)
      {
         var quantityTypeAttribute = parameterElement.Attribute("quantityType");
         if (quantityTypeAttribute == null)
            return;

         quantityTypeAttribute.SetValue(QuantityType.Parameter.ToString());
         _converted = true;
      }

      private void convertPerTimeDimension(XElement displayUnitMapElement, XName dimensionAttributeName)
      {
         var dimensionAttribute = displayUnitMapElement.Attribute(dimensionAttributeName);
         if (dimensionAttribute == null)
            return;

         dimensionAttribute.SetValue(dimensionNameFrom(dimensionAttribute.Value));
         _converted = true;
      }

      private string dimensionNameFrom(string dimensionName)
      {
         if (string.IsNullOrEmpty(dimensionName))
            return dimensionName;

         if (!dimensionName.Contains(RHS_DIMENSION_OLD_SUFFIX))
            return dimensionName;

         return dimensionName.Replace(RHS_DIMENSION_OLD_SUFFIX, AppConstants.RHSDimensionSuffix);
      }

      private void updateObservedDataPaths(DataRepository repository)
      {
         var baseGrid = repository.BaseGrid;
         var baseGridName = baseGrid.Name.Replace(ObjectPath.PATH_DELIMITER, "\\");
         baseGrid.QuantityInfo = new QuantityInfo(baseGrid.Name, new[] {repository.Name, baseGridName}, QuantityType.Time);

         foreach (var col in repository.AllButBaseGrid())
         {
            var colName = col.Name.Replace(ObjectPath.PATH_DELIMITER, "\\");
            var quantityInfo = new QuantityInfo(col.Name, new[] {repository.Name, colName}, QuantityType.Undefined);
            col.QuantityInfo = quantityInfo;
         }
      }
   }
}