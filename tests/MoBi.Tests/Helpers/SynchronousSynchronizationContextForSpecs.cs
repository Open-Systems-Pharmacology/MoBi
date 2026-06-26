using System.Threading;

namespace MoBi.HelpersForTests
{
   //Runs posted and sent callbacks inline.
   public class SynchronousSynchronizationContextForSpecs : SynchronizationContext
   {
      public override void Post(SendOrPostCallback callback, object state) => callback(state);
      public override void Send(SendOrPostCallback callback, object state) => callback(state);
   }
}
