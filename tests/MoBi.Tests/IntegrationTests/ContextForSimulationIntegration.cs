using MoBi.Core.Domain.Model;

namespace MoBi.IntegrationTests
{
   public abstract class ContextForSimulationIntegration<T> : ContextForIntegration<T>
   {
      protected IMoBiSimulation _simulation;

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         Unregister(_simulation);
      }
   }
}