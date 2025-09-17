using MoBi.Core.Domain.Model;
using System.Threading.Tasks;

namespace MoBi.Core.Services
{
   public interface ISimulationRunner
   {
      Task RunSimulationAsync(IMoBiSimulation simulation, bool defineSettings = false);
      void StopSimulation(IMoBiSimulation simulation);
      void StopAllSimulations();
      bool IsSimulationRunning(IMoBiSimulation simulation);
      bool IsAnySimulationRunning();
      bool IsSimulationIdle(IMoBiSimulation simulation);
   }
}