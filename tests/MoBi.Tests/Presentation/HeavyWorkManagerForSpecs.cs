using System;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public class HeavyWorkManagerForSpecs : IHeavyWorkManager
   {
      public bool Start(Action heavyWorkAction, CancellationTokenSource cts = null)
      {
         heavyWorkAction.Invoke();
         return true;
      }

      public bool Start(Action heavyWorkAction, string caption, CancellationTokenSource cts = null)
      {
         return Start(heavyWorkAction);
      }

      public void StartAsync(Action heavyWorkAction)
      {
         Task.Run(heavyWorkAction);
      }

      public event EventHandler<HeavyWorkEventArgs> HeavyWorkedFinished = delegate { };
   }
}