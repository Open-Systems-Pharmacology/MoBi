using System;
using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Snapshots.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using Project = MoBi.Core.Snapshots.Project;

namespace MoBi.Presentation.Presenter
{
   public interface ILoadProjectFromSnapshotPresenter : ILoadFromSnapshotPresenter<MoBiProject>
   {
      /// <summary>
      ///    Project loaded from selected snapshot file. It is null if the user cancels the action or if the file was not loaded
      ///    properly
      /// </summary>
      MoBiProject LoadProject();
   }

   public class LoadProjectFromSnapshotPresenter : LoadProjectFromSnapshotPresenter<MoBiProject, Project>, ILoadProjectFromSnapshotPresenter
   {
      private readonly IUnregisterTask _unRegisterTask;
      private readonly IRegisterTask _registerTask;

      public LoadProjectFromSnapshotPresenter(ILoadFromSnapshotView view,
         ILogPresenter logPresenter,
         ISnapshotTask snapshotTask,
         IDialogCreator dialogCreator,
         IObjectTypeResolver objectTypeResolver,
         IOSPSuiteLogger logger,
         IEventPublisher eventPublisher,
         IQualificationPlanRunner qualificationPlanRunner,
         IUnregisterTask unRegisterTask,
         IRegisterTask registerTask) : base(view, logPresenter, snapshotTask, dialogCreator, objectTypeResolver, logger, eventPublisher, qualificationPlanRunner)
      {
         _unRegisterTask = unRegisterTask;
         _registerTask = registerTask;
      }

      protected override IReadOnlyList<QualificationPlan> AllQualificationPlansFrom(MoBiProject project) => Array.Empty<QualificationPlan>();

      protected override void RegisterProject(MoBiProject project)
      {
         _registerTask.Register(project);
      }

      protected override void UnRegisterProjects(List<MoBiProject> projects) => _unRegisterTask.UnregisterAllIn(ProjectFrom(projects));
   }
}