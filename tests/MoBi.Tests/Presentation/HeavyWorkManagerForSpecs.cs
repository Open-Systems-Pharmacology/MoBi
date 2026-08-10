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

   /// <summary>
   ///    Reproduces the behavior of the real heavy work manager for an action that throws: the exception is reported to the
   ///    user instead of being propagated and <c>Start</c> returns <c>false</c>
   /// </summary>
   public class ExceptionSwallowingHeavyWorkManagerForSpecs : IHeavyWorkManager
   {
      public bool Start(Action heavyWorkAction, CancellationTokenSource cts = null)
      {
         try
         {
            heavyWorkAction.Invoke();
            return true;
         }
         catch (Exception)
         {
            return false;
         }
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