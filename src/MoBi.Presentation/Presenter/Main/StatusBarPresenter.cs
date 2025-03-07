using System;
using MoBi.Assets;
using OSPSuite.TeXReporting.Events;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.MenusAndBars;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Views;
using MoBi.Core.Events;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace MoBi.Presentation.Presenter.Main
{
   public interface IStatusBarPresenter : IMainViewItemPresenter,
      IListener<ProjectLoadedEvent>,
      IListener<ProgressInitEvent>,
      IListener<ProgressingEvent>,
      IListener<ProgressDoneEvent>,
      IListener<ProjectClosedEvent>,
      IListener<ProjectSavedEvent>,
      IListener<ReportCreationStartedEvent>,
      IListener<ReportCreationFinishedEvent>,
      IListener<SimulationsRunCanceledEvent>,
      IListener<SimulationRunFinishedEvent>,
      IListener<SimulationRunStartedEvent>
   {
   }

   public class StatusBarPresenter : IStatusBarPresenter
   {
      private readonly IStatusBarView _view;
      private readonly IMoBiConfiguration _moBiConfiguration;
      public event EventHandler StatusChanged = delegate { };
      private int _numberOfReportsBeingCreated;
      private readonly ConcurrentDictionary<string, bool> _simulations = new ConcurrentDictionary<string, bool>(); 

      private int activeSimulations => _simulations.Count(x => x.Value == true);
      
      public StatusBarPresenter(IStatusBarView view, IMoBiConfiguration moBiConfiguration)
      {
         _view = view;
         _moBiConfiguration = moBiConfiguration;
      }

      public void Initialize()
      {
         StatusBarElements.All().Each(_view.AddItem);
         updateProjectInfo(AppConstants.None, string.Empty, string.Empty, false);
         update(StatusBarElements.Version)
            .WithCaption(_moBiConfiguration.FullVersionDisplay);
         hideProgressBar();
      }

      public void ToggleVisibility()
      {
         /*nothing to do here*/
      }

      public void Handle(ProjectLoadedEvent eventToHandle)
      {
         updateProjectInfo(eventToHandle.Project, true);
      }

      private void updateProjectInfo(IProject project, bool enabled)
      {
         var dimensionModeCaption = project.DowncastTo<MoBiProject>().ReactionDimensionMode == ReactionDimensionMode.AmountBased ? AppConstants.Captions.AmountBasedModel : AppConstants.Captions.ConcentrationBasedModel;
         updateProjectInfo(project.Name, project.FilePath, dimensionModeCaption, enabled);
      }

      private void updateProjectInfo(string projectName, string projectPath, string reactionDimensionMode, bool enabled)
      {
         update(StatusBarElements.ProjectName)
            .WithCaption($"Project: {projectName}")
            .And.ToolTipText($"Project: {projectName}")
            .And.Enabled(enabled);

         update(StatusBarElements.ProjectPath)
            .WithCaption(projectPath)
            .And.ToolTipText($"Project File: {projectPath}")
            .And.Enabled(enabled);


         update(StatusBarElements.ProjectReactionDimensionMode)
            .WithCaption(reactionDimensionMode)
            .And.ToolTipText($"Reaction Rate Base: {reactionDimensionMode}")
            .And.Enabled(enabled);
      }

      private void updateReportInfo()
      {
         string caption = "";
         if (_numberOfReportsBeingCreated == 1)
            caption = "1 report is being created...";
         else if (_numberOfReportsBeingCreated > 1)
            caption = $"{_numberOfReportsBeingCreated} reports are being created...";

         update(StatusBarElements.Report)
            .WithCaption(caption);
      }

      private IStatusBarElementExpression update(StatusBarElement statusBarElement)
      {
         return _view.BarElementExpressionFor(statusBarElement);
      }

      public void Handle(ProgressInitEvent eventToHandle)
      {

         update(StatusBarElements.ProgressBar)
            .WithValue(0)
            .And.Visible(true);

         update(StatusBarElements.ProgressStatus)
            .WithCaption($"{eventToHandle.Message} (1/{_simulations.Count})")
            .And.Visible(true);
      }

      public void Handle(ProgressingEvent eventToHandle)
      {
         update(StatusBarElements.ProgressBar)
            .WithValue(eventToHandle.ProgressPercent);

         update(StatusBarElements.ProgressStatus)
            .WithCaption($"{eventToHandle.Message} ({_simulations.Count +1 - activeSimulations}/{_simulations.Count})");
      }

      public void Handle(ProgressDoneEvent eventToHandle)
      {
         var message = eventToHandle is ProgressDoneWithMessageEvent ?
            $"{(eventToHandle as ProgressDoneWithMessageEvent).Message} ({_simulations.Count + 1 - activeSimulations}/{_simulations.Count})": 
            string.Empty;

         if (activeSimulations == 0)
         {
            resetCountersAndHideBar();
            return;
         }

         update(StatusBarElements.ProgressStatus)
            .WithCaption($"{message}");
      }

      private void resetCountersAndHideBar()
      {
         _simulations.Clear();
         hideProgressBar();
      }

      public void Handle(SimulationsRunCanceledEvent eventToHandle)
      {
         resetCountersAndHideBar();
      }

      private void hideProgressBar()
      {
         update(StatusBarElements.ProgressBar)
            .Visible(false)
            .And.WithValue(0);

         update(StatusBarElements.ProgressStatus)
            .WithCaption(string.Empty)
            .And.Visible(false);
      }

      public void Handle(ProjectSavedEvent eventToHandle)
      {
         updateProjectInfo(eventToHandle.Project, true);
      }

      public void ViewChanged()
      {
         //nothing to do
      }

      public bool CanClose => true;

      public IView BaseView => null;

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         updateProjectInfo(AppConstants.None, string.Empty, string.Empty, false);
      }

      public void ReleaseFrom(IEventPublisher eventPublisher)
      {
         eventPublisher.RemoveListener(this);
      }

      public void Handle(ReportCreationStartedEvent eventToHandle)
      {
         _numberOfReportsBeingCreated++;
         updateReportInfo();
      }

      public void Handle(ReportCreationFinishedEvent eventToHandle)
      {
         _numberOfReportsBeingCreated--;
         updateReportInfo();
      }

      public void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         _simulations[eventToHandle.Simulation.Id] = false;
         if (activeSimulations == 0)
         {
            resetCountersAndHideBar();
         }
      }

      public void Handle(SimulationRunStartedEvent eventToHandle)
      {
         _simulations[eventToHandle.Simulation.Id] = true;
      }
   }
}