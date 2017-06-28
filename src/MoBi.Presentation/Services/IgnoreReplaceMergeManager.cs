using System;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility;

namespace MoBi.Presentation.Services
{
   public abstract class IgnoreReplaceMergeManager<T> : AbstractMergeManager<T>, IIgnoreReplaceMergeManager<T> where T : class, IObjectBase
   {
      public Action<T> RemoveAction
      {
         set { _removeAction = value; }
      }

      public Action<T> AddAction
      {
         set { _addAction = value; }
      }

      public Action CancelAction
      {
         set { _cancelAction = value; }
      }

      protected IgnoreReplaceMergeManager(IApplicationController applicationController, INameCorrector nameCorrector, IMapper<T, ObjectBaseSummaryDTO> dtoMapper,
         IMoBiContext context)
         : base(applicationController, nameCorrector, dtoMapper, context)
      {

      }

      protected IgnoreReplaceMergeManager(IApplicationController applicationController, IMapper<T, ObjectBaseSummaryDTO> dtoMapper, IMoBiContext context)
         : base(applicationController, null, dtoMapper, context)
      {
      }
   }
}