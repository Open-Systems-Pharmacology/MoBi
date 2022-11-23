using MoBi.Presentation.DTO;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.Presentation.Extensions
{
   public static class BreadCrumbsExtensions
   {
      public static bool HasAtLeastTwoDistinctValues(this IEnumerable<IBreadCrumbsDTO> breadcrumbs, int pathElementIndex)
      {
         return breadcrumbs.Select(x => x.PathElementByIndex(pathElementIndex)).Distinct().Count() >= 2;
      }
   }
}
