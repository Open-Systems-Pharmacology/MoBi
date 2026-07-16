using OSPSuite.Core.Snapshots;

namespace MoBi.Core.Snapshots;

public class SimulationPredictedVsObservedChart : CurveChart
{
   public float[] DeviationFoldValues { get; set; }
}