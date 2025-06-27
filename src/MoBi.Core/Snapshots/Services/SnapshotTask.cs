using System.Threading.Tasks;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Core.Snapshots.Services;
using SnapshotContext = OSPSuite.Core.Snapshots.Mappers.SnapshotContext;

namespace MoBi.Core.Snapshots.Services;

public interface ISnapshotTask : ISnapshotTask<MoBiProject, Project>;

public class SnapshotTask : SnapshotTask<MoBiProject, Project>, ISnapshotTask
{
   private readonly IMoBiContext _moBiContext;
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
      _moBiContext = executionContext;
      _projectRetriever = projectRetriever;
      _projectMapper = projectMapper;
   }

   protected override Task<MoBiProject> ProjectFrom(Project snapshot, bool runSimulations)
   {
      _moBiContext.NewProject();
      return _projectMapper.MapToModel(snapshot, new ProjectContext(_moBiContext.CurrentProject, runSimulations));
   }

   protected override SnapshotContext GetSnapshotContext() => new(_projectRetriever.Current, SnapshotVersions.Current);

   protected override MoBiProject GetProject() => _projectRetriever.Current;
}