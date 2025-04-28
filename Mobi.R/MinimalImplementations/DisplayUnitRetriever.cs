using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.R.MinimalImplementations
{
   public class DisplayUnitRetriever : IDisplayUnitRetriever
   {
      public Unit PreferredUnitFor(IWithDimension withDimension, Unit defaultUnit = null)
      {
         return PreferredUnitFor(withDimension.Dimension);
      }

      public Unit PreferredUnitFor(IWithDisplayUnit withDisplayUnit)
      {
         return PreferredUnitFor(withDisplayUnit.Dimension);
      }

      public Unit PreferredUnitFor(IDimension dimension)
      {
         return dimension.DefaultUnit;
      }
   }
}