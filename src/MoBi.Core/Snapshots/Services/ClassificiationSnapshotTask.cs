using MoBi.Core.Domain.Model;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Services
{
   public interface IClassificationSnapshotTask : IClassificationSnapshotTask<MoBiProject>
   {
   }

   public class ClassificationSnapshotTask : ClassificationSnapshotTask<MoBiProject>, IClassificationSnapshotTask
   {
      public ClassificationSnapshotTask(ClassificationMapper classificationMapper) : base(classificationMapper)
      {
      }

      protected override ClassificationSnapshotContext<MoBiProject> ContextFor<TClassifiable, TSubject>(SnapshotContext<MoBiProject> snapshotContext)
      {
         return new ClassificationSnapshotContext(ClassificationTypeFor<TClassifiable>(), snapshotContext);
      }
   }
}
