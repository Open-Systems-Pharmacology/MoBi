using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.MenusAndBars;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Events;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Presentation.Presenter.Main
{
   public interface IMenuAndToolBarPresenter : IMainViewItemPresenter,
      IListener<ProjectLoadedEvent>,
      IListener<ProjectCreatedEvent>,
      IListener<ProjectSavedEvent>,
      IListener<ProjectClosedEvent>,
      IListener<SimulationRunStartedEvent>,
      IListener<SimulationRunFinishedEvent>,
      IListener<ScreenActivatedEvent>,
      IListener<NoActiveScreenEvent>,
      IListener<EditJournalPageStartedEvent>,
      IListener<JournalLoadedEvent>,
      IListener<JournalClosedEvent>,
      IListener<ParameterIdentificationStartedEvent>,
      IListener<ParameterIdentificationTerminatedEvent>,
      IListener<SensitivityAnalysisStartedEvent>,
      IListener<SensitivityAnalysisTerminatedEvent>

   {
   }

   public class MenuAndToolBarPresenter : AbstractMenuAndToolBarPresenter, IMenuAndToolBarPresenter,
      IVisitor<ParameterIdentification>,
      IVisitor<SensitivityAnalysis>,
      IVisitor<MoBiSimulation>
   {
      private readonly IMenuBarItemRepository _menuBarItemRepository;
      private readonly IButtonGroupRepository _buttonGroupRepository;

      private readonly ISkinManager _skinManager;

      private readonly IMoBiContext _context;

      //cache containing the name of the ribbon category corresponding to a given type.Returns an empty string if not found
      private readonly ICache<Type, string> _dynamicRibbonPageCache = new Cache<Type, string>(t => string.Empty);
      private bool _sensitivityRunning;
      private readonly List<ParameterIdentification> _runningParameterIdentifications = new List<ParameterIdentification>();
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private readonly ISimulationRunner _simulationRunner;
      public MenuAndToolBarPresenter(IMenuAndToolBarView view, IMenuBarItemRepository menuBarItemRepository,
         IButtonGroupRepository buttonGroupRepository, IMRUProvider mruProvider, ISkinManager skinManager,
         IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever, ISimulationRunner simulationRunner)
         : base(view, menuBarItemRepository, mruProvider)
      {
         _skinManager = skinManager;
         _context = context;
         _menuBarItemRepository = menuBarItemRepository;
         _buttonGroupRepository = buttonGroupRepository;
         _activeSubjectRetriever = activeSubjectRetriever;
         _simulationRunner = simulationRunner;
      }

      protected override void AddRibbonPages()
      {
         _view.AddApplicationMenu(_buttonGroupRepository.Find(ButtonGroupIds.File));
         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.BuildingBlocks), AppConstants.RibbonPages.Modeling);
         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.ProjectSimulationSettings), AppConstants.RibbonPages.Modeling);

         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.ParameterIdentification), RibbonPages.ParameterIdentificationAndSensitivity);
         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.SensitivityAnalysis), RibbonPages.ParameterIdentificationAndSensitivity);

         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.Journal), AppConstants.RibbonPages.WorkingJournal);

         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.Import), AppConstants.RibbonPages.ImportExport);
         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.Export), AppConstants.RibbonPages.ImportExport);

         _view.InitializeSkinGallery(_skinManager, AppConstants.RibbonCategories.Skins, AppConstants.RibbonPages.Utilities);
         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.Tools), AppConstants.RibbonPages.Utilities);
         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.DisplayUnits), AppConstants.RibbonPages.Utilities);
         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.Favorites), AppConstants.RibbonPages.Utilities);
         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.History), AppConstants.RibbonPages.Utilities);

         _view.AddPageGroupToPage(_buttonGroupRepository.Find(ButtonGroupIds.View), AppConstants.RibbonPages.Views);

         initializeDynamicPages();

         _view.AddQuickAccessButton(_menuBarItemRepository[MenuBarItemIds.OpenProject]);
         _view.AddQuickAccessButton(_menuBarItemRepository[MenuBarItemIds.SaveProject]);
         _view.AddPageHeaderItemLinks(_menuBarItemRepository[MenuBarItemIds.Help]);
      }

      private void initializeDynamicPages()
      {
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.Molecules, Color.LightGreen);
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.Reactions, Color.LightGreen);
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.SpatialStructures, Color.LightGreen);
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.PassiveTransports, Color.LightGreen);
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.Observers, Color.LightGreen);
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.Events, Color.LightGreen);
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.InitialConditions, Color.LightGreen);
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.ParameterValues, Color.LightGreen);
         _view.CreateDynamicPageCategory(AppConstants.RibbonCategories.Simulation, Color.LightGreen);
         _view.CreateDynamicPageCategory(RibbonCategories.ParameterIdentification, Color.LightGreen);
         _view.CreateDynamicPageCategory(RibbonCategories.SensitivityAnalysis, Color.LightGreen);

         _dynamicRibbonPageCache.Add(typeof(MoleculeBuildingBlock), AppConstants.RibbonCategories.Molecules);
         _dynamicRibbonPageCache.Add(typeof(ReactionBuildingBlock), AppConstants.RibbonCategories.Reactions);
         _dynamicRibbonPageCache.Add(typeof(ObserverBuildingBlock), AppConstants.RibbonCategories.Observers);
         _dynamicRibbonPageCache.Add(typeof(SpatialStructure), AppConstants.RibbonCategories.SpatialStructures);

         _dynamicRibbonPageCache.Add(typeof(PassiveTransportBuildingBlock), AppConstants.RibbonCategories.PassiveTransports);
         _dynamicRibbonPageCache.Add(typeof(EventGroupBuildingBlock), AppConstants.RibbonCategories.Events);
         _dynamicRibbonPageCache.Add(typeof(InitialConditionsBuildingBlock), AppConstants.RibbonCategories.InitialConditions);
         _dynamicRibbonPageCache.Add(typeof(ParameterValuesBuildingBlock), AppConstants.RibbonCategories.ParameterValues);
         _dynamicRibbonPageCache.Add(typeof(IMoBiSimulation), AppConstants.RibbonCategories.Simulation);
         _dynamicRibbonPageCache.Add(typeof(ParameterIdentification), RibbonCategories.ParameterIdentification);
         _dynamicRibbonPageCache.Add(typeof(SensitivityAnalysis), RibbonCategories.SensitivityAnalysis);

         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.AddMolecule), AppConstants.RibbonPages.DynamicMolecules, AppConstants.RibbonCategories.Molecules);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.AddReaction), AppConstants.RibbonPages.DynamicReactions, AppConstants.RibbonCategories.Reactions);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.EditDiagram), AppConstants.RibbonPages.DynamicReactions, AppConstants.RibbonCategories.Reactions);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.AddSpatialStructure), AppConstants.RibbonPages.DynamicSpatialStructures, AppConstants.RibbonCategories.SpatialStructures);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.EditDiagram), AppConstants.RibbonPages.DynamicSpatialStructures, AppConstants.RibbonCategories.SpatialStructures);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.AddPassiveTransport), AppConstants.RibbonPages.DynamicPassiveTransports, AppConstants.RibbonCategories.PassiveTransports);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.AddObserver), AppConstants.RibbonPages.DynamicObservers, AppConstants.RibbonCategories.Observers);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.AddEvent), AppConstants.RibbonPages.DynamicEvents, AppConstants.RibbonCategories.Events);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.EditInitialConditions), AppConstants.RibbonPages.DynamicInitialConditions, AppConstants.RibbonCategories.InitialConditions);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.EditParameterValues), AppConstants.RibbonPages.DynamicParameterValues, AppConstants.RibbonCategories.ParameterValues);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.Simulation), AppConstants.RibbonPages.DynamicRunSimulation, AppConstants.RibbonCategories.Simulation);

         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.RunParameterIdentification), RibbonPages.RunParameterIdentification, RibbonCategories.ParameterIdentification);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.ParameterIdentificationAnalyses), RibbonPages.RunParameterIdentification, RibbonCategories.ParameterIdentification);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.ParameterIdentificationConfidenceInterval), RibbonPages.RunParameterIdentification, RibbonCategories.ParameterIdentification);

         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.RunSensitivityAnalysis), RibbonPages.RunSensitivityAnalysis, RibbonCategories.SensitivityAnalysis);
         _view.AddDynamicPageGroupToPageCategory(_buttonGroupRepository.Find(ButtonGroupIds.SensitivityAnalysisPKParameterAnalyses), RibbonPages.RunSensitivityAnalysis, RibbonCategories.SensitivityAnalysis);
      }

      public void Handle(ProjectCreatedEvent eventToHandle)
      {
         projectItemsAreEnabled = true;
      }

      protected override void DisableMenuBarItemsForProgramStart()
      {
         DisableAll();
         enableDefaultItems();
      }

      private void enableDefaultItems()
      {
         _menuBarItemRepository[MenuBarItemIds.NewProject].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.NewAmountProject].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.NewConcentrationProject].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.OpenProject].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.Options].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.HistoryView].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.NotificationView].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.ComparisonView].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.OpenSimulation].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.ModuleExplorerView].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.SimulationExplorerView].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.GarbageCollection].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.Exit].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.About].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.Help].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.JournalView].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.JournalDiagramView].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.ImportSBML].Enabled = true;
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         projectItemsAreEnabled = false;
      }

      private void updateSaveProjectButtons(bool enabled)
      {
         var canSave = enabled && !_context.ProjectIsReadOnly;
         _menuBarItemRepository[MenuBarItemIds.SaveGroup].Enabled = enabled;
         _menuBarItemRepository[MenuBarItemIds.SaveProject].Enabled = canSave;
         _menuBarItemRepository[MenuBarItemIds.SaveProjectAs].Enabled = enabled;
      }

      private bool enableActiveSimulationItems
      {
         set
         {
            _menuBarItemRepository[MenuBarItemIds.Run].Enabled = value;
            _menuBarItemRepository[MenuBarItemIds.RunWithSettings].Enabled = value;
            _menuBarItemRepository[MenuBarItemIds.ConfigureActiveSimulation].Enabled = value;
            _menuBarItemRepository[MenuBarItemIds.CalculateScaleFactors].Enabled = value;
         }
      }

      private bool projectItemsAreEnabled
      {
         set
         {
            bool enabled = value;
            updateSaveProjectButtons(enabled);
            _menuBarItemRepository[MenuBarItemIds.CloseProject].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.ManageUserDisplayUnits].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.ManageProjectDisplayUnits].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.UpdateAllToDisplayUnits].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewSimulation].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewIndividual].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewExpressionProfile].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewModule].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.EditProjectSimulationSettings].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.SaveProjectSimulationSettings].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadProjectSimulationSettings].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadSimulationIntoProject].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.HistoryReportGroup].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.ExportHistoryToExcel].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewMolecule].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadMolecule].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadMoleculeFromTemplate].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.AddPKSimMolecule].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewReaction].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadReaction].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadReactionFromTemplate].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewTopContainer].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadTopContainer].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadTopContainerFromTemplate].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewPassiveTransport].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadPassiveTransport].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadPassiveTransportFromTemplate].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewObserverGroup].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewAmountObserver].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewContainerObserver].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadObserverGroup].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadContainerObserver].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadAmountObserver].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadObserverFromTemplateGroup].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadContainerObserverFromTemplate].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadAmountObserverFromTemplate].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewEvent].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadEvent].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadEventFromTemplate].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewReactionMolecule].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.ZoomIn].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.ZoomOut].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.FitToPage].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewNeighborhood].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.AddObservedData].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadObservedData].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.InitialConditionsExtend].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.ParameterValuesExtend].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.AddProteinExpression].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.SearchView].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewParameterValue].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.NewInitialConditions].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.CreateJournalPage].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.SelectJournal].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.LoadFavorites].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.SaveFavorites].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.CreateParameterIdentification].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.ParameterIdentificationFeedbackView].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.CreateSensitivityAnalysis].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.SensitivityAnalysisFeedbackView].Enabled = enabled;
            _menuBarItemRepository[MenuBarItemIds.ClearHistory].Enabled = enabled;
         }
      }

      public void Handle(SimulationRunStartedEvent eventToHandle)
      {
         _menuBarItemRepository[MenuBarItemIds.Stop].Enabled = true;
      }

      public void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         enableDefaultItems();
         projectItemsAreEnabled = true;
         _menuBarItemRepository[MenuBarItemIds.Stop].Enabled = _simulationRunner.IsAnySimulationRunning();
         _menuBarItemRepository[MenuBarItemIds.Run].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.RunWithSettings].Enabled = true;
         _menuBarItemRepository[MenuBarItemIds.CalculateScaleFactors].Enabled = true;
      }

      public bool SimulationRunEnabled
      {
         set => enableActiveSimulationItems = value;
      }

      public void Handle(ScreenActivatedEvent screenActivatedEvent)
      {
         hideAllDynamicCategories();

         var subject = screenActivatedEvent.Presenter.Subject;
         if (subject == null) return;

         this.Visit(subject);

         var matchingPage = _dynamicRibbonPageCache.Keys.FirstOrDefault(x => subject.IsAnImplementationOf(x));
         if (matchingPage == null) return;

         _view.SetPageCategoryVisibility(_dynamicRibbonPageCache[matchingPage], visible: true);
      }

      public void Handle(NoActiveScreenEvent eventToHandle)
      {
         enableActiveSimulationItems = false;
         hideAllDynamicCategories();
      }

      private void hideAllDynamicCategories()
      {
         AppConstants.RibbonCategories.AllDynamicCategories()
            .Each(c => _view.SetPageCategoryVisibility(c, visible: false));
      }

      public void Handle(EditJournalPageStartedEvent eventToHandle)
      {
         _menuBarItemRepository[MenuBarItemIds.JournalEditorView].Enabled = true;
      }

      public void Handle(JournalLoadedEvent eventToHandle)
      {
         enableJournalItems = true;
      }

      public void Handle(JournalClosedEvent eventToHandle)
      {
         enableJournalItems = false;
      }

      private bool enableJournalItems
      {
         set
         {
            _menuBarItemRepository[MenuBarItemIds.SearchJournal].Enabled = value;
            _menuBarItemRepository[MenuBarItemIds.ExportJournal].Enabled = value;
            _menuBarItemRepository[MenuBarItemIds.RefreshJournal].Enabled = value;
         }
      }

      public void Visit(ParameterIdentification parameterIdentification)
      {
         updateParameterIdentificationItems(parameterIdentification);
      }

      public void Handle(ParameterIdentificationStartedEvent parameterIdentificationEvent)
      {
         _runningParameterIdentifications.Add(parameterIdentificationEvent.ParameterIdentification);
         updateParameterIdentificationItems(parameterIdentificationEvent.ParameterIdentification);
      }

      public void Handle(ParameterIdentificationTerminatedEvent parameterIdentificationEvent)
      {
         var parameterIdentification = parameterIdentificationEvent.ParameterIdentification;
         _runningParameterIdentifications.Remove(parameterIdentificationEvent.ParameterIdentification);
         //only update if selected subject in the actual parameter identification terminated
         var activeSubject = _activeSubjectRetriever.Active<ParameterIdentification>();
         if (Equals(activeSubject, parameterIdentification))
            updateParameterIdentificationItems(parameterIdentification);
      }

      private void updateParameterIdentificationItems(ParameterIdentification parameterIdentification)
      {
         var parameterIdentificationRunning = _runningParameterIdentifications.Contains(parameterIdentification);
         var hasResult = !parameterIdentificationRunning && parameterIdentification.HasResults;
         _menuBarItemRepository[MenuBarItemIds.RunParameterIdentification].Enabled = !parameterIdentificationRunning;
         _menuBarItemRepository[MenuBarItemIds.StopParameterIdentification].Enabled = parameterIdentificationRunning;
         _menuBarItemRepository[MenuBarItemIds.TimeProfileParameterIdentification].Enabled = hasResult;
         _menuBarItemRepository[MenuBarItemIds.PredictedVsObservedParameterIdentification].Enabled = hasResult;
         _menuBarItemRepository[MenuBarItemIds.CorrelationMatrixParameterIdentification].Enabled = hasResult;
         _menuBarItemRepository[MenuBarItemIds.CovarianceMatrixParameterIdentification].Enabled = hasResult;
         _menuBarItemRepository[MenuBarItemIds.ResidualsVsTimeParameterIdentification].Enabled = hasResult;
         _menuBarItemRepository[MenuBarItemIds.ResidualHistogramParameterIdentification].Enabled = hasResult;
         _menuBarItemRepository[MenuBarItemIds.TimeProfilePredictionInterval].Enabled = hasResult;
         _menuBarItemRepository[MenuBarItemIds.TimeProfileConfidenceInterval].Enabled = hasResult;
         _menuBarItemRepository[MenuBarItemIds.TimeProfileVPCInterval].Enabled = hasResult;
      }

      public void Visit(MoBiSimulation simulation)
      {
         enableActiveSimulationItems = true;
      }

      public void Handle(SensitivityAnalysisStartedEvent sensitivityAnalysisEvent)
      {
         _sensitivityRunning = true;
         updateSensitivityAnalysisItems(sensitivityAnalysisEvent.SensitivityAnalysis);
      }

      public void Handle(SensitivityAnalysisTerminatedEvent sensitivityAnalysisEvent)
      {
         _sensitivityRunning = false;
         updateSensitivityAnalysisItems(sensitivityAnalysisEvent.SensitivityAnalysis);
      }

      private void updateSensitivityAnalysisItems(SensitivityAnalysis sensitivityAnalysis)
      {
         var hasResult = !_sensitivityRunning && sensitivityAnalysis.HasResults;
         _menuBarItemRepository[MenuBarItemIds.RunSensitivityAnalysis].Enabled = !_sensitivityRunning;
         _menuBarItemRepository[MenuBarItemIds.StopSensitivityAnalysis].Enabled = _sensitivityRunning;
         _menuBarItemRepository[MenuBarItemIds.SensitivityAnalysisPKParameterAnalysis].Enabled = hasResult;
      }

      public void Visit(SensitivityAnalysis sensitivityAnalysis)
      {
         updateSensitivityAnalysisItems(sensitivityAnalysis);
      }
   }
}