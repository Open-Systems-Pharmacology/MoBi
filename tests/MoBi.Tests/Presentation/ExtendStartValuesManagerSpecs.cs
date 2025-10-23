using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_ExtendStartValuesManager : ContextSpecification<ExtendPathAndValuesManager<FakeObject>>
   {
      protected override void Context()
      {
         sut = new FakeObjectMergeManager();
      }

      protected ICache<string, FakeObject> GetTargetCache()
      {
         var cache = new Cache<string, FakeObject>
         {
            {"object1", new FakeObject()},
            {"object2", new FakeObject()},
            {"object3", new FakeObject()}
         };

         return cache;
      }

      protected ICache<string, FakeObject> GetMergeCache()
      {
         var cache = new Cache<string, FakeObject>
         {
            {"object1", new FakeObject()},
            {"object2", new FakeObject()},
            {"object3", new FakeObject()}
         };
         return cache;
      }
   }

   public class FakeObject : PathAndValueEntity
   {
      
   }

   class FakeObjectMergeManager : ExtendPathAndValuesManager<FakeObject>
   {
      protected override IReadOnlyList<string> GetConflictingElements(ICache<string, FakeObject> cacheToMerge, ICache<string, FakeObject> targetCache, Func<FakeObject, FakeObject, bool> areElementsEquivalent)
      {
         return cacheToMerge.Keys.Where(key => targetCache.Keys.ContainsItem(key)).ToList();
      }

      protected override IEnumerable<FakeObject> GetElementsToAdd(ICache<string, FakeObject> cacheToMerge, ICache<string, FakeObject> targetCache)
      {
         return cacheToMerge.Keys.Where(key => !targetCache.Contains(key)).Select(key => cacheToMerge[key]);
      }
   }
}
