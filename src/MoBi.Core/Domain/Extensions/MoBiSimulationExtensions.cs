using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.Core.Domain.Extensions
{
   public static class MoBiSimulationExtensions
   {
      /// <summary>
      ///    Returns a <see cref="SimulationTransfer" /> to export the <paramref name="simulation" /> to pkml. The transfer also
      ///    contains the changes tracked in the simulation, so that they are restored when the pkml is loaded back into MoBi.
      /// </summary>
      public static SimulationTransfer ToSimulationTransfer(this IMoBiSimulation simulation) => new SimulationTransfer
      {
         Simulation = simulation,
         OriginalQuantityValues = simulation.OriginalQuantityValues.ToList(),
         HasUntraceableChanges = simulation.HasUntraceableChanges
      };
   }
}
