using System.Threading.Tasks;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Qualification;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Core.Snapshots.Services;
using SnapshotContext = OSPSuite.Core.Snapshots.Mappers.SnapshotContext;

namespace MoBi.Core.Snapshots.Services;

public interface ISnapshotTask : ISnapshotTask<MoBiProject, Project>
{
   /// <summary>
   ///    Loads the project from the snapshot. If there are any PK-Sim modules, they are rebuilt through a local installation
   ///    of PK-Sim.
   ///    If any of the PK-Sim building blocks should have markdown exported it will be done during the module rebuild.
   /// </summary>
   /// <param name="snapshot">The MoBi project snapshot</param>
   /// <param name="runSimulations">
   ///    True if the simulations specified in the snapshot should be run during the project
   ///    creation
   /// </param>
   /// <param name="qualificationConfiguration">
   ///    The configuration containing any building block inputs that should have report
   ///    markdown exported while the module is rebuilt in PK-Sim
   /// </param>
   /// <returns>The newly created MoBi project and the result of the markdown export as an array of InputMapping</returns>
   Task<(MoBiProject, InputMapping[])> LoadProjectFromSnapshotAndExportInputsAsync(Project snapshot, bool runSimulations, QualificationConfiguration qualificationConfiguration);
}

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

   public Task<(MoBiProject, InputMapping[])> LoadProjectFromSnapshotAndExportInputsAsync(Project snapshot, bool runSimulations, QualificationConfiguration qualificationConfiguration)
   {
      _moBiContext.NewProject();
      return _projectMapper.MapToModelAndExportInputs(snapshot, new ProjectContext(_moBiContext.CurrentProject, runSimulations), qualificationConfiguration);
   }
}