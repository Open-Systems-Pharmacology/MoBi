using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Helper
{
   public static class MergedDimensionFactoryExtensions
   {
      public static IDimension GetDimensionForChart(this IDimensionFactory dimensionFactory, string dimensionName)
      {
         return dimensionFactory.GetMergedDimensionFor(new DataColumn
                                                          {
            Dimension =
               dimensionFactory.GetDimension(dimensionName)
         });
      }
   }
}