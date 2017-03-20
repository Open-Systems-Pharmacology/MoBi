using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Extensions
{
   public static class SpatialStructureExtensions
   {
      public static T WithTopContainer<T>(this T spatialStructure,IContainer topContainer) where T:ISpatialStructure
      {
         spatialStructure.AddTopContainer(topContainer);
         return spatialStructure;
      }

      public static bool IsInSpatialStructure<T>(this ISpatialStructure spatialStructure,T lookFor) where T : class, IEntity
      {
         if (spatialStructure.Any(c => c.Equals(lookFor))) return true;
         return spatialStructure.Aggregate(false,
            (current, topContainier) =>
               current || topContainier.GetAllChildren<T>().Contains(lookFor));
      }
   }
}