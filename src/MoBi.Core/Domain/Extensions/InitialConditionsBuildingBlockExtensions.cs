using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Extensions
{
   public static class InitialConditionsBuildingBlockExtensions
   {
      /// <summary>
      /// Looks up the description of a initial condition from a list of builders
      /// </summary>
      /// <param name="builders">The list of builders to search</param>
      /// <param name="name">The name of the molecule builder</param>
      /// <returns>If the molecule builder is found, returns it's description, otherwise returns the empty string</returns>
      public static string FindDescriptionForInitialConditionFromBuilder(this IEnumerable<MoleculeBuilder> builders, string name)
      {
         var moleculeBuilder = builders.FirstOrDefault(builder => builder.Name.Equals(name));
         return moleculeBuilder == null ? string.Empty : moleculeBuilder.Description;
      }
   }
}