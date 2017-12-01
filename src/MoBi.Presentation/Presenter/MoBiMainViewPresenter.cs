using Castle.Core.Configuration;
using MoBi.Assets;
using MoBi.Core;
using OSPSuite.TeXReporting.Events;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Events;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Services;
using IProjectTask = MoBi.Presentation.Tasks.IProjectTask;

namespace MoBi.Presentation.Presenter
{
   public interface IMoBiMainViewPresenter : IMainViewPresenter,
      IListener<SimulationRunStartedEvent>,
      IListener<SimulationRunFinishedEvent>

   {
      void Run(StartOptions startOptions);
      bool FormClosing();
      void RestoreLayout();
   }

   public class MoBiMainViewPresenter : AbstractMainViewPresenter<IMoBiMainView, IMoBiMainViewPresenter>, IMoBiMainViewPresenter
   {
      private readonly IProjectTask _projectTask;
      private readonly IRepository<IMainViewItemPresenter> _allMainViewItemPresenters;
      private readonly ISkinManager _skinManager;
      private readonly IExitCommand _exitCommand;
      private readonly IUserSettings _userSettings;
      private readonly IMoBiConfiguration _configuration;
      private readonly IWatermarkStatusChecker _watermarkStatusChecker;

      public MoBiMainViewPresenter(
         IMoBiMainView view, 
         IRepository<IMainViewItemPresenter> allMainViewItemPresenters,
         IProjectTask projectTask, 
         ISkinManager skinManager, 
         IExitCommand exitCommand,
         IEventPublisher eventPublisher, 
         IUserSettings userSettings,
         ITabbedMdiChildViewContextMenuFactory contextMenuFactory, 
         IMoBiConfiguration configuration, 
         IWatermarkStatusChecker watermarkStatusChecker) : base(view, eventPublisher, contextMenuFactory)
      {
         _skinManager = skinManager;
         _exitCommand = exitCommand;
         _userSettings = userSettings;
         _configuration = configuration;
         _watermarkStatusChecker = watermarkStatusChecker;
         _allMainViewItemPresenters = allMainViewItemPresenters;
         _projectTask = projectTask;
         _view.AttachPresenter(this);
         _view.InitializeResources();
      }

      public override void Initialize()
      {
         _view.Initialize();
         View.Caption = _configuration.ProductDisplayName;
         _allMainViewItemPresenters.All().Each(x => x.Initialize());
         _skinManager.ActivateSkin(_userSettings, _userSettings.ActiveSkin);
      }

      public override void Run()
      {
         _watermarkStatusChecker.CheckWatermarkStatus();
      }

      public override void RemoveAlert()
      {
      }

      public override void OpenFile(string fileName)
      {
      }

      public void Run(StartOptions startOptions)
      {
         this.DoWithinExceptionHandler(() => startWithOptions(startOptions));
      }

      private void startWithOptions(StartOptions startOptions)
      {
         if (!startOptions.IsValid()) return;
         switch (startOptions.StartOptionsMode)
         {
            case StartOptionsMode.Project:
               _projectTask.OpenProjectFrom(startOptions.FileToLoad);
               break;
            case StartOptionsMode.Simulation:
               _projectTask.StartWithSimulation(startOptions.FileToLoad);
               break;
            case StartOptionsMode.Journal:
               _projectTask.StartWithJournal(startOptions.FileToLoad);
               break;
            default:
               return;
         }
      }

      public override void Handle(ReportCreationStartedEvent reportStartedEvent)
      {
         _view.DisplayNotification(AppConstants.Captions.ReportCreationStarted,
            AppConstants.Captions.ReportCreationStartedMessage(reportStartedEvent.ReportFullPath), string.Empty);
      }

      public override void Handle(ReportCreationFinishedEvent reportFinishedEvent)
      {
         _view.DisplayNotification(AppConstants.Captions.ReportCreationFinished,
            AppConstants.Captions.ReportCreationFinishedMessage(reportFinishedEvent.ReportFullPath), reportFinishedEvent.ReportFullPath);
      }

      public bool FormClosing()
      {
         _exitCommand.Execute();
         return _exitCommand.Canceled;
      }

      public void RestoreLayout()
      {
         _userSettings.RestoreLayout();
      }

      public void Handle(SimulationRunStartedEvent eventToHandle)
      {
         View.AllowChildActivation = false;
      }

      public void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         View.AllowChildActivation = true;
      }
   }
}