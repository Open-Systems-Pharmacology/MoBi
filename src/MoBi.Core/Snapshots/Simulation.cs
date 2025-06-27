using OSPSuite.Core.Snapshots;

namespace MoBi.Core.Snapshots;

public class Simulation : SnapshotBase
{
   public OutputMapping[] OutputMappings { get; set; }

   public CurveChart Chart { get; set; }
   public SimulationConfiguration Configuration { get; set; }
   public OutputSelections OutputSelections { get; set; }

   public string ParameterIdentificationWorkingDirectory { get; set; }
   public SimulationPredictedVsObservedChart SimulationPredictedVsObservedChart { get; set; }
   public CurveChart SimulationResidualVsTimeChart { get; set; }
}

public class SimulationPredictedVsObservedChart : CurveChart
{
   public float[] DeviationFoldValues { get; set; }
}