using System.Collections.Generic;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots.Mappers;
using MoBi.Core.Domain.Model;
using Classification = OSPSuite.Core.Snapshots.Classification;

namespace MoBi.Core.Snapshots.Mappers;

public class ClassificationSnapshotContext : ClassificationSnapshotContext<MoBiProject>
{
   public ClassificationSnapshotContext(ClassificationType classificationType, SnapshotContext<MoBiProject> baseContext) : base(classificationType, baseContext, ProjectVersions.Current)
   {

   }

}

public class ClassificationMapper : ClassificationMapper<MoBiProject>;