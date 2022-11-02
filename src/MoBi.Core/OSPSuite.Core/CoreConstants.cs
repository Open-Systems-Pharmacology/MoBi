using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core
{
   public static class CoreConstants
   {
      // TODO: remove common code from PK-sim when this is promoted to Core

      public static class ContainerName
      {
         public static string ExpressionProfileName(string moleculeName, string species, string category)
            => compositeNameFor(char.Parse(ObjectPath.PATH_DELIMITER), moleculeName, species, category);

         public static (string moleculeName, string speciesName, string category) NamesFromExpressionProfileName(string expressionProfileName)
         {
            var names = NamesFromCompositeName(expressionProfileName, char.Parse(ObjectPath.PATH_DELIMITER));
            if (names.Count != 3)
               return (string.Empty, string.Empty, string.Empty);

            return (names[0], names[1], names[2]);
         }
      }
      
      public const char COMPOSITE_SEPARATOR = '-';

      public static IReadOnlyList<string> NamesFromCompositeName(string compositeName, char separator = COMPOSITE_SEPARATOR)
      {
         return compositeName.Split(separator);
      }

      private static string compositeNameFor(char separator, params string[] names)
      {
         if (names == null || names.Length == 0)
            return string.Empty;

         var nonEmptyNames = names.ToList();
         nonEmptyNames.RemoveAll(string.IsNullOrEmpty);

         return nonEmptyNames.Select(x => x.Trim()).ToString($"{separator}");
      }
   }

   
}
