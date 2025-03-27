using MoBi.Core.Domain.Model;
using System.Threading.Tasks;

namespace MoBi.Core.Services
{
   public interface ISimulationRunner
   {
      void RunSimulation(IMoBiSimulation simulation, bool defineSettings = false);
      Task RunSimulationAsync(IMoBiSimulation simulation, bool defineSettings = false);
      /// <summary>
      /// Stops one simulation or all simulations if <paramref name="simulation"/> is <c>null</c>.
      /// </summary>
      void StopSimulation(IMoBiSimulation simulation = null);
   }
}