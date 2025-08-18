using System;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Extensions
{
   public static class ParameterExtensions
   {
      public static bool ShouldExportToSnapshot(this IParameter parameter)
      {
         if (parameter == null)
            return false;

         if (parameter.IsDefault)
            return false;

         return parameter.ValueIsDefined();
      }

      /// <summary>
      ///    Returns <c>true</c> if the value can be computed and is not NaN otherwise <c>false</c>
      /// </summary>
      public static bool ValueIsDefined(this IParameter parameter)
      {
         if (!ValueIsComputable(parameter))
            return false;

         return !double.IsNaN(parameter.Value);
      }

      /// <summary>
      ///    Returns <c>true</c> if the value can be computed otherwise <c>false</c>
      /// </summary>
      public static bool ValueIsComputable(this IParameter parameter)
      {
         try
         {
            //let's compute the value. Exception will be thrown if value cannot be calculated
            var v = parameter.Value;
            return true;
         }
         catch (Exception)
         {
            //this is a parameter that cannot be evaluated and should not be exported
            return false;
         }
      }
   }
}
