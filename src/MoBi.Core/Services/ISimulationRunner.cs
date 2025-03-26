using MoBi.Core.Domain.Model;
using System.Threading.Tasks;

namespace MoBi.Core.Services
{
   public interface ISimulationRunner
   {
      void RunSimulation(IMoBiSimulation simulation, bool defineSettings = false);
      Task RunSimulationAsync(IMoBiSimulation simulation, bool defineSettings = false);
      /// <summary>
      /// Stops one simulation or all simulations if simulation is null
      /// </summary>
      /// <param name="simulation"></param>
      void StopSimulation(IMoBiSimulation simulation = null);
   }
}