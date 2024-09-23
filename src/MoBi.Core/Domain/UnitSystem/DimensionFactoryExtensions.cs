using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Domain.UnitSystem
{
   public static class DimensionFactoryExtensions
   {
      public static IDimension TryGetDimension(this IDimensionFactory dimensionFactory, string dimensionName)
      {
         try
         {
            return dimensionFactory.Dimension(dimensionName);
         }
         catch (KeyNotFoundException)
         {
            // We could make the fix once here rather than per-usage
            return null;
         }
      }
   }
}