using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Helpers
{
   public abstract class AbstractMergeManager<T> where T : class, IObjectBase
   {
      protected Action<T> _removeAction;
      protected Action<T> _addAction;
      protected Action _cancelAction;
      private readonly IMapper<T, ObjectBaseSummaryDTO> _dtoMapper;
      protected Action<T, string> _cloneAction;
      protected Action<T, T> _mergeAction;
      private readonly IApplicationController _applicationController;
      private readonly INameCorrector _nameCorrector;
      private readonly IMoBiContext _context;
      protected bool _mergeOptionEnabled = false;
      protected bool _cloneOptionEnabled = false;

      protected AbstractMergeManager(IApplicationController applicationController, INameCorrector nameCorrector, IMapper<T, ObjectBaseSummaryDTO> dtoMapper, IMoBiContext context)
      {
         _applicationController = applicationController;
         _nameCorrector = nameCorrector;
         _addAction = element => { };
         _removeAction = element => { };
         _cancelAction = () => { };
         _cloneAction = (element, originalName) => { };
         _mergeAction = (target, merge) => { };
         _dtoMapper = dtoMapper;
         _context = context;
      }

      public virtual void Merge(ICache<string, T> merge, ICache<string, T> target, Func<T, T, bool> areElementsEquivalent = null)
      {
         var resolved = 0;
         var conflictResolution = areElementsEquivalent ?? ((x, y) => false);
         var conflictingElements = GetConflictingElements(merge, target, conflictResolution).ToList();

         var option = MergeConflictOptions.SkipOnce;
         foreach (var key in conflictingElements)
         {
            resolved++;
            if (!option.IsAppliedToAll())
               option = prompt(merge[key], target[key], conflictingElements.Count() - resolved);

            if (option.IsClone())
            {
               if (option.IsAutoRename())
               {
                  _nameCorrector.AutoCorrectName(AppConstants.UnallowedNames.Union(target.Select(targetEntity => targetEntity.Name)), merge[key]);
               }
               else
               {
                  if (!_nameCorrector.CorrectName(AppConstants.UnallowedNames.Union(target.Select(targetEntity => targetEntity.Name)), merge[key]))
                     return;
               }
               _cloneAction(merge[key], key);
            }
            else if (option.IsMerge())
               _mergeAction(target[key], merge[key]);

            else if (option.IsSkip())
               continue;

            else if (option == MergeConflictOptions.Cancel)
            {
               _context.PublishEvent(new MergeCanceledEvent());
               _cancelAction();
               return;
            }
            else
            {
               _removeAction(target[key]);
               _addAction(merge[key]);
            }
         }

         GetElementsToAdd(merge, target).Each(_addAction);
      }

      private MergeConflictOptions prompt(T merge, T target, int remainingConflicts)
      {
         using (var presenter = _applicationController.Start<IMergeConflictResolverPresenter>())
         {
            presenter.MergeOptionEnabled = _mergeOptionEnabled;
            presenter.CloneOptionEnabled = _cloneOptionEnabled;
 
            presenter.ObjectBaseSummaryDTOMapper = objectBase => _dtoMapper.MapFrom(objectBase as T);
            return presenter.ShowConflict(merge, target, remainingConflicts);
         }
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