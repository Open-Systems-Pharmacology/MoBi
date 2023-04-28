using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_IgnoreReplaceMergeManager : ContextSpecification<ExtendStartValuesManager<FakeObject>>
   {
      protected IMergeConflictResolverPresenter _mergeConflictResolverPresenter;
      private IApplicationController _applicationController;

      protected override void Context()
      {
         _applicationController = A.Fake<IApplicationController>();
         _mergeConflictResolverPresenter = A.Fake<IMergeConflictResolverPresenter>();
         A.CallTo(() => _applicationController.Start<IMergeConflictResolverPresenter>()).Returns(_mergeConflictResolverPresenter);
         sut = new FakeObjectMergeManager(_applicationController);
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

   public class When_resolving_conflicts_on_objects : concern_for_IgnoreReplaceMergeManager
   {
      private ICache<string, FakeObject> _mergeCache;
      private ICache<string, FakeObject> _targetCache;

      protected override void Because()
      {
         _mergeCache = GetMergeCache();
         _targetCache = GetTargetCache();
         sut.Merge(_mergeCache, _targetCache);
      }

      [Observation]
      public void conflicts_must_be_resolved()
      {
         A.CallTo(() => _mergeConflictResolverPresenter.ShowConflict(_mergeCache["object1"], _targetCache["object1"], 2)).MustHaveHappened();
         A.CallTo(() => _mergeConflictResolverPresenter.ShowConflict(_mergeCache["object2"], _targetCache["object2"], 1)).MustHaveHappened();
         A.CallTo(() => _mergeConflictResolverPresenter.ShowConflict(_mergeCache["object3"], _targetCache["object3"], 0)).MustHaveHappened();
      }
   }

   public class When_canceling_conflict_resolution : concern_for_IgnoreReplaceMergeManager
   {
      private Action _cancelAction;

      protected override void Context()
      {
         base.Context();
         _cancelAction = A.Fake<Action>();
         sut.CancelAction = _cancelAction;
         A.CallTo(() => _mergeConflictResolverPresenter.ShowConflict(A<IObjectBase>.Ignored, A<IObjectBase>.Ignored, A<int>.Ignored)).Returns(MergeConflictOptions.Cancel);
      }

      protected override void Because()
      {
         sut.Merge(GetMergeCache(), GetTargetCache());
      }

      [Observation]
      public void must_have_called_cancel_action_once()
      {
         A.CallTo(() => _cancelAction()).MustHaveHappened();
      }
   }

   public class When_applying_conflict_resolution_to_all : concern_for_IgnoreReplaceMergeManager
   {
      private ICache<string, FakeObject> _mergeCache;
      private ICache<string, FakeObject> _targetCache;
      private Action<FakeObject> _addAction;
      private Action<FakeObject> _removeAction;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _mergeConflictResolverPresenter.ShowConflict(A<IObjectBase>.Ignored, A<IObjectBase>.Ignored, A<int>.Ignored)).Returns(MergeConflictOptions.ReplaceAll);

         _addAction = A.Fake<Action<FakeObject>>();
         sut.AddAction = _addAction;
         _removeAction = A.Fake<Action<FakeObject>>();
         sut.RemoveAction = _removeAction;
      }

      protected override void Because()
      {
         _mergeCache = GetMergeCache();
         _targetCache = GetTargetCache();
         sut.Merge(_mergeCache, _targetCache);
      }

      [Observation]
      public void should_show_dialog_only_once()
      {
         A.CallTo(() => _mergeConflictResolverPresenter.ShowConflict(A<IObjectBase>.Ignored, A<IObjectBase>.Ignored, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
      }

      [Observation]
      public void calls_to_replace_all_elements_should_result()
      {
         A.CallTo(() => _addAction(A<FakeObject>.Ignored)).MustHaveHappenedANumberOfTimesMatching(x => x == 3);
         A.CallTo(() => _removeAction(A<FakeObject>.Ignored)).MustHaveHappenedANumberOfTimesMatching(x => x == 3);
      }
   }

   public class FakeObject : StartValueBase
   {
      
   }

   class FakeObjectMergeManager : ExtendStartValuesManager<FakeObject>
   {
      public FakeObjectMergeManager(IApplicationController applicationController)
         : base(applicationController, A.Fake<IMapper<FakeObject, ObjectBaseSummaryDTO>>(), A.Fake<IMoBiContext>())
      {
      }

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
