using OSPSuite.Core.Snapshots;

namespace MoBi.Core.Snapshots;

public class SimulationSettings : SnapshotBase
{
   public SolverSettings Solver { get; set; }
   public OutputSchema OutputSchema { get; set; }
   public double RandomSeed { get; set; }
}