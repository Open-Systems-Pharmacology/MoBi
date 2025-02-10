using MoBi.Core.Domain.Model;

namespace MoBi.Core.Events
{
   public class ShowSimulationChangesEvent
   {
      public readonly IMoBiSimulation Simulation;

      public ShowSimulationChangesEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }
   }
}
