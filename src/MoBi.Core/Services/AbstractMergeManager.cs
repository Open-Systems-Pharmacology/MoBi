using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
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