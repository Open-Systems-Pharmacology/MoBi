using OSPSuite.Core.Snapshots;

namespace MoBi.Core.Snapshots;

public class ModuleConfiguration : SnapshotBase
{
   public string Module { get; set; }
   public string SelectedInitialConditions { get; set; }
   public string SelectedParameterValues { get; set; }
}