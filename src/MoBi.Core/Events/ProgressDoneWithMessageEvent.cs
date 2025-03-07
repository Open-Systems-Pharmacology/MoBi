using OSPSuite.Utility.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoBi.Core.Events
{
   public class ProgressDoneWithMessageEvent : ProgressDoneEvent
   {
      public string Message { get; }

      public ProgressDoneWithMessageEvent(string message)
      {
         Message = message;
      }
   }
}
