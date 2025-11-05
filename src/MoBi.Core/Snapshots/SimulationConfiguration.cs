using OSPSuite.Core.Snapshots;

namespace MoBi.Core.Snapshots;

public class SimulationConfiguration : SnapshotBase
{
   public SimulationSettings Settings { set; get; }
   public string IndividualBuildingBlock { get; set; }
   public string[] ExpressionProfiles { get; set; }
   public ModuleConfiguration[] ModuleConfigurations { get; set; }
}