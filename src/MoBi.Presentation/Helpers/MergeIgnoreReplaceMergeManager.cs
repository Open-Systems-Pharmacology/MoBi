using System;
using OSPSuite.Utility;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Helpers
{
   public abstract class MergeIgnoreReplaceMergeManager<T> : IgnoreReplaceMergeManager<T>, IMergeIgnoreReplaceMergeManager<T> where T : class, IObjectBase
   {
      protected MergeIgnoreReplaceMergeManager(IApplicationController applicationController, IMapper<T, ObjectBaseSummaryDTO> dtoMapper, IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
         _mergeOptionEnabled = true;
      }

      public Action<T, T> MergeAction
      {
         set { _mergeAction = value; }
      }
   }
}