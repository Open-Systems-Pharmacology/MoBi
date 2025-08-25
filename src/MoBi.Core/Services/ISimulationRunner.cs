using MoBi.Core.Domain.Model;

namespace MoBi.Core.Services
{
   public interface ISimulationRunner : ICoreSimulationRunner
   {
      bool IsAnySimulationRunning();
      bool IsSimulationIdle(IMoBiSimulation simulation);
   }
}