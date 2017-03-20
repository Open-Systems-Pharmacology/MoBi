using System;
using OSPSuite.Utility;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Helpers
{
   public abstract class IgnoreReplaceCloneMergeManager<T> : IgnoreReplaceMergeManager<T>, IIgnoreReplaceCloneMergeManager<T> where T : class, IObjectBase
   {

      protected IgnoreReplaceCloneMergeManager(
         IApplicationController applicationController, 
         INameCorrector nameCorrector,
         IMapper<T, ObjectBaseSummaryDTO> dtoMapper,
         IMoBiContext context)
         : base(applicationController, nameCorrector, dtoMapper, context)
      {
         _cloneOptionEnabled = true;
      }

      public Action<T, string> CloneAction
      {
         set { _cloneAction = value; }
      }
   }
}