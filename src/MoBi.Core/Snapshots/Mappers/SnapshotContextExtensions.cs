using OSPSuite.Core.Snapshots.Mappers;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Snapshots.Mappers;

public static class SnapshotContextExtensions
{
   public static MoBiProject MoBiProject(this SnapshotContext context)
   {
      return context.Project as MoBiProject;
   }
}