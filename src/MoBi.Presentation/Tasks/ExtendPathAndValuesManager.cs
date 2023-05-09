using System;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;

namespace MoBi.Presentation.Tasks
{
   public interface IExtendPathAndValuesManager<T> where T : PathAndValueEntity
   {
      /// <summary>
      ///    Manages the conflict of the two caches
      /// </summary>
      /// <param name="merge">The cache being merged into the target</param>
      /// <param name="target">The target cache of the merge</param>
      /// <param name="areElementsEquivalent">
      ///    Optionally specify a Func that will detect for equivalence between two conflicting
      ///    elements
      /// </param>
      void Merge(ICache<string, T> merge, ICache<string, T> target, Func<T, T, bool> areElementsEquivalent = null);

      /// <summary>
      ///    In the event of a conflict where user specifies that the newly merged element should replace the existing, this
      ///    method will be used to remove the existing element
      /// </summary>
      Action<T> RemoveAction { set; }

      /// <summary>
      ///    In the event of a conflict where user specifies that the newly merged element should replace the existing, this
      ///    method will be used to add the newly merged element
      /// </summary>
      Action<T> AddAction { set; }

      /// <summary>
      ///    In the event that the user cancels the merge part way through managing conflicts, this method will be run.
      /// </summary>
      Action CancelAction { set; }
   }
   
   public abstract class ExtendPathAndValuesManager<T> : AbstractMergeManager<T>, IExtendPathAndValuesManager<T> where T : PathAndValueEntity
   {
      public Action<T> RemoveAction
      {
         set => _removeAction = value;
      }

      public Action<T> AddAction
      {
         set => _addAction = value;
      }

      public Action CancelAction
      {
         set => _cancelAction = value;
      }

      protected ExtendPathAndValuesManager(IApplicationController applicationController, INameCorrector nameCorrector, IMapper<T, ObjectBaseSummaryDTO> dtoMapper,
         IMoBiContext context)
         : base(applicationController, nameCorrector, dtoMapper, context)
      {

      }

      protected ExtendPathAndValuesManager(IApplicationController applicationController, IMapper<T, ObjectBaseSummaryDTO> dtoMapper, IMoBiContext context)
         : base(applicationController, null, dtoMapper, context)
      {
      }
   }
}