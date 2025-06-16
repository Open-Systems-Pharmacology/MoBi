using OSPSuite.Core.Snapshots.Mappers;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Data;

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

   public class SnapshotContextWithDataRepository : SnapshotContextWithDataRepository<MoBiProject>
   {
      public SnapshotContextWithDataRepository(DataRepository dataRepository, SnapshotContext<MoBiProject> baseContext) : base(dataRepository, baseContext, ProjectVersions.Current)
      {
      }
   }
}
