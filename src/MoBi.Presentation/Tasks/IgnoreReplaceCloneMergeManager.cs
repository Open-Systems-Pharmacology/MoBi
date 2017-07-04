using System;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility;

namespace MoBi.Presentation.Tasks
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