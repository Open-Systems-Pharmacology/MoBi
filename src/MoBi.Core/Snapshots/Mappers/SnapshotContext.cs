using OSPSuite.Core.Snapshots.Mappers;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Snapshots.Mappers
{
   public class SnapshotContext : SnapshotContext<MoBiProject>
   {
      //This constructor should only be called when initiation the project load and the project is not available
      protected SnapshotContext() : base(new MoBiProject(), ProjectVersions.Current)
      {
      }

      public SnapshotContext(MoBiProject project, int version) : base(project, version)
      {
      }

      public SnapshotContext(SnapshotContext baseContext) : base(baseContext.Project, baseContext.Version)
      {
      }

   }

   public class ProjectContext : SnapshotContext
   {
      public ProjectContext(bool runSimulations)
      {
         RunSimulations = runSimulations;
      }

      public bool RunSimulations { get; }
   }
}
