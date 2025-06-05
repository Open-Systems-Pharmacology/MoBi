using MoBi.Core.Domain.Model;
using System.Threading.Tasks;
using MoBi.Core.Services;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Mappers;
using SnapshotContext = OSPSuite.Core.Snapshots.Mappers.SnapshotContext;

namespace MoBi.Core.Snapshots.Services
{
   public interface ISnapshotTask : OSPSuite.Core.Snapshots.Services.ISnapshotTask<MoBiProject, Project>
   {

   }

   public class SnapshotTask : OSPSuite.Core.Snapshots.Services.SnapshotTask<MoBiProject, Project>, ISnapshotTask
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly ProjectMapper _projectMapper;

      public SnapshotTask(IJsonSerializer jsonSerializer, 
         ISnapshotMapper snapshotMapper, 
         IDialogCreator dialogCreator, 
         IObjectTypeResolver objectTypeResolver, 
         IMoBiContext executionContext,
         IMoBiProjectRetriever projectRetriever,
         ProjectMapper projectMapper) : 
         base(jsonSerializer, snapshotMapper, dialogCreator, objectTypeResolver, executionContext)
      {
         _projectRetriever = projectRetriever;
         _projectMapper = projectMapper;
      }

      protected override Task<MoBiProject> ProjectFrom(Project snapshot, bool runSimulations)
      {
         return _projectMapper.MapToModel(snapshot, new ProjectContext(runSimulations));
      }

      protected override SnapshotContext GetSnapshotContext()
      {
         return new Mappers.SnapshotContext(_projectRetriever.Current, ProjectVersions.Current);
      }

      protected override MoBiProject GetProject()
      {
         return _projectRetriever.Current;
      }
   }
   
}
