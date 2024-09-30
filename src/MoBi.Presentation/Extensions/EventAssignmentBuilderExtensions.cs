using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Collections;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Extensions
{
   public static class EventAssignmentBuilderExtensions
   {
      public static  ICache<IObjectBase, string> GetForbiddenAssignees(this EventAssignmentBuilder eventAssignmentBuilder)
      {
         var cache = new Cache<IObjectBase, string>();
         if (eventAssignmentBuilder.UseAsValue || eventAssignmentBuilder.Formula == null)
            return cache;

         eventAssignmentBuilder.Formula.ObjectPaths.Select(x => x.TryResolve<IUsingFormula>(eventAssignmentBuilder)).Each(x => cache.Add(x, AppConstants.Captions.AssigningFormulaCreatesCircularReference));
         return cache;
      }
   }
}
