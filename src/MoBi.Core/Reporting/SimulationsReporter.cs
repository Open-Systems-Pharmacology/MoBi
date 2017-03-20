using OSPSuite.TeXReporting.Items;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Reporting
{
   internal class SimulationsReporter : BuildingBlocksReporter<IMoBiSimulation>
   {
      public SimulationsReporter(SimulationReporter reporter) : base(reporter, new Part(Constants.SIMULATIONS))
      {
      }
   }
}