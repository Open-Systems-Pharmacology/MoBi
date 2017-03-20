using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Core.Helper
{
   public class ReportingHelper
   {
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public ReportingHelper(IDisplayUnitRetriever displayUnitRetriever)
      {
         _displayUnitRetriever = displayUnitRetriever;
      }

      public float? ConvertToDisplayUnit(IWithDisplayUnit withDisplayUnit, float? value)
      {
         return (float?) ConvertToDisplayUnit(withDisplayUnit, (double?) value);
      }

      public float? ConvertToDisplayUnit(Unit unit, IDimension dimension, float? value)
      {
         return (float?)ConvertToDisplayUnit(unit, dimension, (double?) value);
      }

      public double? ConvertToDisplayUnit(IWithDisplayUnit withDisplayUnit, double? value)
      {
         return value == null ? (double?) null : withDisplayUnit.ConvertToDisplayUnit(value);
      }

      public double? ConvertToDisplayUnit(Unit unit, IDimension dimension, double? value)
      {
         if (value == null)
            return null;
         return dimension.BaseUnitValueToUnitValue(GetDisplayUnitFor(unit, dimension), (double)value);
      }

      public Unit GetDisplayUnitFor(IDimension dimension)
      {
         return _displayUnitRetriever.PreferredUnitFor(dimension) ?? dimension.DefaultUnit;
      }

      public Unit GetDisplayUnitFor(Unit unit, IDimension dimension)
      {
         return unit ?? GetDisplayUnitFor(dimension);
      }

      public Unit GetDisplayUnitFor(IWithDisplayUnit withDisplayUnit)
      {
         return GetDisplayUnitFor(withDisplayUnit.DisplayUnit, withDisplayUnit.Dimension);
      }
   }
}
