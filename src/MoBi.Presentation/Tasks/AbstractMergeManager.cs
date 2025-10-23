using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public interface IMergeManager<T> where T : class, IObjectBase
   {
      /// <summary>
      ///    Manages the conflict of the two caches
      /// </summary>
      /// <param name="merge">The cache being merged into the target</param>
      /// <param name="target">The target cache of the merge</param>
      /// <param name="mergeConflictOption">The default option when merging items that are equivalent</param>
      /// <param name="areElementsEquivalent">
      ///    Optionally specify a Func that will detect for equivalence between two conflicting
      ///    elements
      /// </param>
      void Merge(ICache<string, T> merge, ICache<string, T> target, MergeConflictOptions mergeConflictOption, Func<T, T, bool> areElementsEquivalent = null);
   }

   public abstract class AbstractMergeManager<T> : IMergeManager<T> where T : class, IObjectBase
   {
      protected Action<T> _removeAction = element => { };
      protected Action<T> _addAction = element => { };

      public virtual void Merge(ICache<string, T> merge, ICache<string, T> target, MergeConflictOptions mergeConflictOption, Func<T, T, bool> areElementsEquivalent = null)
      {
         var conflictResolution = areElementsEquivalent ?? ((x, y) => false);
         var conflictingElements = GetConflictingElements(merge, target, conflictResolution).ToList();

         foreach (var key in conflictingElements)
         {
            if (mergeConflictOption.IsReplace())
            {
               _removeAction(target[key]);
               _addAction(merge[key]);
            }
         }

         GetElementsToAdd(merge, target).Each(_addAction);
      }

      protected virtual IReadOnlyList<string> GetConflictingElements(ICache<string, T> cacheToMerge, ICache<string, T> targetCache, Func<T, T, bool> areElementsEquivalent)
      {
         return cacheToMerge.Keys.Where(key => targetCache.Contains(key) && !areElementsEquivalent(cacheToMerge[key], targetCache[key])).ToList();
      }

      protected virtual IEnumerable<T> GetElementsToAdd(ICache<string, T> cacheToMerge, ICache<string, T> targetCache)
      {
         return cacheToMerge.Keys.Where(key => !targetCache.Contains(key)).Select(key => cacheToMerge[key]);
      }
   }
}