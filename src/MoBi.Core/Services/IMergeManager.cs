using System;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Collections;

namespace MoBi.Core.Services;

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