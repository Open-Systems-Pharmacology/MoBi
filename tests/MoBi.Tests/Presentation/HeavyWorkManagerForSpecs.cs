using System;
using System.Threading.Tasks;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation
{
   public class HeavyWorkManagerForSpecs : IHeavyWorkManager
   {
      public bool Start(Action heavyWorkAction)
      {
         heavyWorkAction.Invoke();
         return true;
      }

      public bool Start(Action heavyWorkAction, string caption)
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