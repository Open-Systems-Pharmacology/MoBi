using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Snapshots.Services;

namespace MoBi.Presentation.Presenter
{
   public interface ILoadProjectFromSnapshotPresenter : ILoadFromSnapshotPresenter<MoBiProject>
   {
      /// <summary>
      /// Project loaded from selected snapshot file. It is null if the user cancels the action or if the file was not loaded properly
      /// </summary>
      MoBiProject LoadProject();

   }

   public class LoadProjectFromSnapshotPresenter : LoadProjectFromSnapshotPresenter<MoBiProject, MoBi.Core.Snapshots.Project>, ILoadProjectFromSnapshotPresenter
   {
      private readonly IRegisterTask _registerTask;
      private readonly IUnregisterTask _unRegisterTask;

      public LoadProjectFromSnapshotPresenter(ILoadFromSnapshotView view,
         ILogPresenter logPresenter,
         ISnapshotTask snapshotTask,
         IDialogCreator dialogCreator,
         IObjectTypeResolver objectTypeResolver,
         IOSPSuiteLogger logger,
         IEventPublisher eventPublisher,
         IQualificationPlanRunner qualificationPlanRunner,
         IRegisterTask registerTask,
         IUnregisterTask unRegisterTask) : base(view, logPresenter, snapshotTask, dialogCreator, objectTypeResolver, logger, eventPublisher, qualificationPlanRunner)
      {
         _registerTask = registerTask;
         _unRegisterTask = unRegisterTask;
      }

      protected override IReadOnlyList<QualificationPlan> AllQualificationPlansFrom(MoBiProject project) => Array.Empty<QualificationPlan>();

      protected override void RegisterProject(MoBiProject project) => _registerTask.Register(project);

      protected override void UnRegisterProjects(List<MoBiProject> projects) => _unRegisterTask.UnregisterAllIn(ProjectFrom(projects));
   }
}
