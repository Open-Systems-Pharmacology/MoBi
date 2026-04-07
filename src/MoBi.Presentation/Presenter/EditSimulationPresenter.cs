using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.ModelDiagram;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSimulationPresenter :
      IPresenter<IEditSimulationView>,
      ISingleStartPresenter<IMoBiSimulation>,
      IDiagramBuildingBlockPresenter,
      IListener<SimulationRunFinishedEvent>,
      IListener<EntitySelectedEvent>,
      IListener<SimulationReloadEvent>,
      IListener<FavoritesSelectedEvent>,
      IListener<UserDefinedSelectedEvent>,
      IListener<ShowSimulationChangesEvent>,
      IListener<SimulationRunStartedEvent>,
      IListener<SimulationAnalysisCreatedEvent>
   {
      void LoadDiagram();
      void LoadChanges();
      void RemoveAnalysis(ISimulationAnalysisPresenter analysisPresenter);
   }

   public class EditSimulationPresenter : SingleStartPresenter<IEditSimulationView, IEditSimulationPresenter>, IEditSimulationPresenter, IListener<ObservedDataRemovedFromAnalysableEvent>
   {
      private IMoBiSimulation _simulation;
      private readonly IHierarchicalSimulationPresenter _hierarchicalPresenter;
      private readonly ISimulationDiagramPresenter _simulationDiagramPresenter;
      private readonly IEditSolverSettingsPresenter _solverSettingsPresenter;
      private readonly IEditOutputSchemaPresenter _editOutputSchemaPresenter;
      private readonly ISimulationChangesPresenter _simulationChangesPresenter;
      private readonly ISimulationEntitySourceReferenceFactory _entitySourceReferenceFactory;
      private readonly IEditInSimulationPresenterFactory _showPresenterFactory;
      private readonly ICache<Type, IEditInSimulationPresenter> _cacheShowPresenter;
      private bool _diagramLoaded;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IEditFavoritesInSimulationPresenter _favoritesPresenter;
      private readonly IUserDefinedParametersPresenter _userDefinedParametersPresenter;
      protected readonly IMoBiContext _context;
      private readonly ISimulationOutputMappingPresenter _simulationOutputMappingPresenter;
      private readonly IOutputMappingMatchingTask _outputMappingMatchingTask;
      private TrackableSimulation _trackableSimulation;
      private readonly ISimulationRunner _simulationRunner;
      private readonly ISimulationAnalysisPresenterFactory _simulationAnalysisPresenterFactory;
      private readonly IList<ISimulationAnalysisPresenter> _analysisPresenters = new List<ISimulationAnalysisPresenter>();

      public EditSimulationPresenter(
         IEditSimulationView view,
         IHierarchicalSimulationPresenter hierarchicalPresenter,
         ISimulationDiagramPresenter simulationDiagramPresenter,
         IEditSolverSettingsPresenter solverSettingsPresenter,
         IEditOutputSchemaPresenter editOutputSchemaPresenter,
         IEditInSimulationPresenterFactory showPresenterFactory,
         IHeavyWorkManager heavyWorkManager,
         IEditFavoritesInSimulationPresenter favoritesPresenter,
         IUserDefinedParametersPresenter userDefinedParametersPresenter,
         ISimulationOutputMappingPresenter simulationOutputMappingPresenter,
         IMoBiContext context,
         IOutputMappingMatchingTask outputMappingMatchingTask,
         ISimulationChangesPresenter changesPresenter,
         ISimulationEntitySourceReferenceFactory entitySourceReferenceFactory,
         ISimulationRunner simulationRunner,
         ISimulationAnalysisPresenterFactory simulationAnalysisPresenterFactory)
         : base(view)
      {
         _simulationChangesPresenter = changesPresenter;
         _entitySourceReferenceFactory = entitySourceReferenceFactory;
         _editOutputSchemaPresenter = editOutputSchemaPresenter;
         _showPresenterFactory = showPresenterFactory;
         _heavyWorkManager = heavyWorkManager;
         _favoritesPresenter = favoritesPresenter;
         _userDefinedParametersPresenter = userDefinedParametersPresenter;
         _solverSettingsPresenter = solverSettingsPresenter;
         _hierarchicalPresenter = hierarchicalPresenter;
         _simulationDiagramPresenter = simulationDiagramPresenter;
         _simulationOutputMappingPresenter = simulationOutputMappingPresenter;
         _context = context;
         _simulationRunner = simulationRunner;
         _simulationAnalysisPresenterFactory = simulationAnalysisPresenterFactory;
         _outputMappingMatchingTask = outputMappingMatchingTask;
         _view.SetTreeView(hierarchicalPresenter.BaseView);
         _view.SetModelDiagram(_simulationDiagramPresenter.View);
         _hierarchicalPresenter.ShowOutputSchema = showOutputSchema;
         _hierarchicalPresenter.ShowSolverSettings = showSolverSettings;
         _hierarchicalPresenter.SimulationFavorites = () => _favoritesPresenter.Favorites();
         _view.SetChangesView(changesPresenter.View);
         _view.SetDataView(_simulationOutputMappingPresenter.View);
         AddSubPresenters(_hierarchicalPresenter, _simulationDiagramPresenter, _solverSettingsPresenter, _editOutputSchemaPresenter,
            _favoritesPresenter, _userDefinedParametersPresenter, _simulationOutputMappingPresenter, _simulationChangesPresenter);
         _cacheShowPresenter = new Cache<Type, IEditInSimulationPresenter> { OnMissingKey = x => null };
      }

      public void LoadChanges() => _simulationChangesPresenter.Edit(_simulation);

      private void showSolverSettings() => _view.SetEditView(_solverSettingsPresenter.BaseView);

      private void showOutputSchema() => _view.SetEditView(_editOutputSchemaPresenter.BaseView);

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _cacheShowPresenter.Each(p => p.ReleaseFrom(eventPublisher));
         _cacheShowPresenter.Clear();
         releaseAllAnalysisPresenters(eventPublisher);
      }

      private void releaseAllAnalysisPresenters(IEventPublisher eventPublisher)
      {
         _analysisPresenters.Each(x => releasePresenter(x, eventPublisher));
         _analysisPresenters.Clear();
      }

      private void releasePresenter(ISimulationAnalysisPresenter presenter, IEventPublisher eventPublisher)
      {
         presenter.Clear();
         presenter.ReleaseFrom(eventPublisher);
         unRegisterObservedDataEvent(presenter);
      }

      public void Edit(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _hierarchicalPresenter.Edit(simulation);
         _solverSettingsPresenter.Edit(_simulation);
         _editOutputSchemaPresenter.Edit(_simulation);
         _view.SetEditView(_favoritesPresenter.BaseView);
         _simulationChangesPresenter.Edit(_simulation);
         _simulationOutputMappingPresenter.EditSimulation(simulation);
         UpdateCaption();
         _view.Display();
         loadAnalyses();
         _trackableSimulation = new TrackableSimulation(_simulation, _entitySourceReferenceFactory.CreateFor(simulation));
         _favoritesPresenter.TrackableSimulation = _trackableSimulation;
         _favoritesPresenter.Edit(_simulation);
         _view.SetParametersTabEnabled(_simulationRunner.IsSimulationIdle(simulation));
      }

      private void loadAnalyses() => _simulation.Analyses.Each(addAnalysis);

      private void addAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         var presenter = _simulationAnalysisPresenterFactory.PresenterFor(simulationAnalysis);
         _analysisPresenters.Add(presenter);
         registerObservedDataEvent(presenter);
         presenter.InitializeAnalysis(simulationAnalysis, _simulation);
         _view.AddAnalysis(presenter);
      }

      private void unRegisterObservedDataEvent(ISimulationAnalysisPresenter presenter)
      {
         if (presenter is IChartPresenter chartPresenter)
            chartPresenter.OnObservedDataAddedToChart -= onObservedDataAddedToChart;
      }

      private void registerObservedDataEvent(ISimulationAnalysisPresenter presenter)
      {
         if (presenter is IChartPresenter chartPresenter)
            chartPresenter.OnObservedDataAddedToChart += onObservedDataAddedToChart;
      }

      public void RemoveAnalysis(ISimulationAnalysisPresenter analysisPresenter)
      {
         _analysisPresenters.Remove(analysisPresenter);
         _simulation.RemoveAnalysis(analysisPresenter.Analysis);
         _view.RemoveAnalysis(analysisPresenter);
         analysisPresenter.Clear();
         unRegisterObservedDataEvent(analysisPresenter);
      }

      public override void Edit(object subject) => Edit(subject.DowncastTo<IMoBiSimulation>());

      public override object Subject => _simulation;

      public void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         if (!_simulation.Equals(eventToHandle.Simulation))
            return;

         _view.SetParametersTabEnabled(true);

         if (!_view.ShowsResults && eventToHandle.Success)
            _view.ShowResultsTab();

         _analysisPresenters.Each(x => x.UpdateAnalysisBasedOn(_simulation));
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         var entity = eventToHandle.ObjectBase as IEntity;
         if (!shouldShow(entity))
            return;

         _view.Display();

         var parameter = entity as IParameter;
         if (parameter != null)
            setupEditPresenter(parameter, parameter.ParentContainer);

         else
            setupEditPresenter(entity);
      }

      private void setupEditPresenter(IParameter parameter, IContainer parentContainer)
      {
         var presenter = setupEditPresenter(parentContainer).DowncastTo<IEditPresenterWithParameters>();
         presenter.SelectParameter(parameter);
      }

      private IEditInSimulationPresenter setupEditPresenter(IEntity entity)
      {
         var entityType = entity.GetType();
         var showPresenter = _cacheShowPresenter[entityType];
         if (showPresenter == null)
         {
            //create a new one and add it to the cache
            showPresenter = _showPresenterFactory.PresenterFor(entity);
            if (showPresenter == null)
               return null;

            showPresenter.InitializeWith(CommandCollector);
            _cacheShowPresenter.Add(entityType, showPresenter);
         }

         _view.SetEditView(showPresenter.BaseView);
         showPresenter.TrackableSimulation = _trackableSimulation;
         showPresenter.Edit(entity);
         return showPresenter;
      }

      public void LoadDiagram()
      {
         if (_diagramLoaded) return;
         _heavyWorkManager.Start(() => _simulationDiagramPresenter.Edit(_simulation), AppConstants.Captions.LoadingDiagram);
         _diagramLoaded = true;
      }

      private bool shouldShow(IEntity entity)
      {
         if (entity == null)
            return false;

         return _simulation.Model.Root.Equals(entity.RootContainer) || _simulation.Model.Neighborhoods.Equals(entity.RootContainer);
      }

      public void ZoomIn() => _simulationDiagramPresenter.Zoom(AppConstants.Diagram.Model.ZoomInFactor);

      public void ZoomOut() => _simulationDiagramPresenter.Zoom(1 / AppConstants.Diagram.Model.ZoomInFactor);

      public void FitToPage() => _simulationDiagramPresenter.Zoom(AppConstants.Diagram.Base.ZoomFitToPageFactor);

      public void LayoutByForces() => _simulationDiagramPresenter.Layout(null, AppConstants.Diagram.Base.LayoutDepthChildren, null);

      protected override void UpdateCaption() => _view.Caption = AppConstants.Captions.SimulationCaption(_simulation.Name);

      public void Handle(SimulationReloadEvent eventToHandle)
      {
         if (_simulation.Equals(eventToHandle.Simulation))
            reloadAll();
      }

      private void reloadAll()
      {
         _hierarchicalPresenter.Clear();
         _diagramLoaded = false;
         Edit(_simulation);
      }

      public void Handle(FavoritesSelectedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         _view.SetEditView(_favoritesPresenter.BaseView);
      }

      public void Handle(UserDefinedSelectedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         _userDefinedParametersPresenter.ShowUserDefinedParametersIn(_simulation.Model.Root);
         _view.SetEditView(_userDefinedParametersPresenter.BaseView);
      }

      private void onObservedDataAddedToChart(object sender, ObservedDataAddedToChartEventArgs e)
      {
         var observedDataToAdd = observedDataNotMappedInTheSimulation(e);

         if (!observedDataToAdd.Any())
            return;

         foreach (var dataRepository in observedDataToAdd)
         {
            _outputMappingMatchingTask.AddMatchingOutputMapping(dataRepository, _simulation);
            _context.PublishEvent(new ObservedDataAddedToAnalysableEvent(_simulation, dataRepository, false));
         }
      }

      private List<DataRepository> observedDataNotMappedInTheSimulation(ObservedDataAddedToChartEventArgs e)
      {
         return e.AddedDataRepositories
            .Where(dataRepository => !_simulation.OutputMappings.Any(x => x.UsesObservedData(dataRepository))).ToList();
      }

      public void Handle(ObservedDataRemovedFromAnalysableEvent e)
      {
         if (_simulation == e.Analysable.DowncastTo<IMoBiSimulation>())
            _analysisPresenters.Each(x => x.UpdateAnalysisBasedOn(_simulation));
      }

      public void Handle(SimulationAnalysisCreatedEvent eventToHandle)
      {
         if (!Equals(eventToHandle.Analysable, _simulation))
            return;

         addAnalysis(eventToHandle.SimulationAnalysis);
      }

      private bool canHandle(IObjectBaseEvent objectBaseEvent) => Equals(_simulation, objectBaseEvent.ObjectBase);

      public void Handle(ShowSimulationChangesEvent eventToHandle)
      {
         if (eventToHandle.Simulation == _simulation)
         {
            _view.ShowChangesTab();
         }
      }

      public void Handle(SimulationRunStartedEvent eventToHandle)
      {
         if (!_simulation.Equals(eventToHandle.Simulation))
            return;

         _view.SetParametersTabEnabled(false);
      }
   }
}