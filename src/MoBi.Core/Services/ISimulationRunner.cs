using MoBi.Core.Domain.Model;

namespace MoBi.Core.Services
{
   public interface ISimulationRunner
   {
      void RunSimulation(IMoBiSimulation simulation, bool defineSettings = false);
      void StopSimulation();
   }
}