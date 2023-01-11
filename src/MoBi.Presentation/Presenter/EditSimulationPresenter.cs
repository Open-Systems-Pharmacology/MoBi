using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter.ModelDiagram;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
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
      IListener<UserDefinedSelectedEvent>
   {
      void LoadDiagram();
      string CreateResultTabCaption(string viewCaption);
   }

   public class EditSimulationPresenter : SingleStartPresenter<IEditSimulationView, IEditSimulationPresenter>, IEditSimulationPresenter, IListener<ObservedDataRemovedFromAnalysableEvent>
   {
      private IMoBiSimulation _simulation;
      private readonly IHierarchicalSimulationPresenter _hierarchicalPresenter;
      private readonly ISimulationDiagramPresenter _simulationDiagramPresenter;
      private readonly ISimulationChartPresenter _chartPresenter;
      private readonly ISimulationPredictedVsObservedChartPresenter _simulationPredictedVsObservedChartPresenter;
      private readonly ISimulationResidualVsTimeChartPresenter _simulationResidualVsTimeChartPresenter;
      private readonly IEditSolverSettingsPresenter _solverSettingsPresenter;
      private readonly IEditOutputSchemaPresenter _editOutputSchemaPresenter;
      private readonly IEditInSimulationPresenterFactory _showPresenterFactory;
      private readonly ICache<Type, IEditInSimulationPresenter> _cacheShowPresenter;
      private bool _diagramLoaded;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IChartFactory _chartFactory;
      private readonly IEditFavoritesInSimulationPresenter _favoritesPresenter;
      private readonly IUserDefinedParametersPresenter _userDefinedParametersPresenter;
      private readonly IChartTasks _chartTask;
      protected readonly IMoBiContext _context;
      private readonly ISimulationOutputMappingPresenter _simulationOutputMappingPresenter;
      private readonly IOutputMappingMatchingTask _outputMappingMatchingTask;

      public EditSimulationPresenter(IEditSimulationView view, ISimulationChartPresenter chartPresenter,
         IHierarchicalSimulationPresenter hierarchicalPresenter, ISimulationDiagramPresenter simulationDiagramPresenter,
         IEditSolverSettingsPresenter solverSettingsPresenter, IEditOutputSchemaPresenter editOutputSchemaPresenter,
         IEditInSimulationPresenterFactory showPresenterFactory, IHeavyWorkManager heavyWorkManager, IChartFactory chartFactory,
         IEditFavoritesInSimulationPresenter favoritesPresenter, IChartTasks chartTask,
         IUserDefinedParametersPresenter userDefinedParametersPresenter, ISimulationOutputMappingPresenter simulationOutputMappingPresenter,
         ISimulationPredictedVsObservedChartPresenter simulationPredictedVsObservedChartPresenter,
         ISimulationResidualVsTimeChartPresenter simulationResidualVsTimeChartPresenter, IMoBiContext context,
         IOutputMappingMatchingTask outputMappingMatchingTask)
         : base(view)
      {
         _editOutputSchemaPresenter = editOutputSchemaPresenter;
         _showPresenterFactory = showPresenterFactory;
         _heavyWorkManager = heavyWorkManager;
         _chartFactory = chartFactory;
         _favoritesPresenter = favoritesPresenter;
         _chartTask = chartTask;
         _userDefinedParametersPresenter = userDefinedParametersPresenter;
         _solverSettingsPresenter = solverSettingsPresenter;
         _hierarchicalPresenter = hierarchicalPresenter;
         _simulationDiagramPresenter = simulationDiagramPresenter;
         _simulationPredictedVsObservedChartPresenter = simulationPredictedVsObservedChartPresenter;
         _simulationResidualVsTimeChartPresenter = simulationResidualVsTimeChartPresenter;
         _chartPresenter = chartPresenter;
         _simulationOutputMappingPresenter = simulationOutputMappingPresenter;
         _view.SetTreeView(hierarchicalPresenter.BaseView);
         _view.SetModelDiagram(_simulationDiagramPresenter.View);
         _hierarchicalPresenter.ShowOutputSchema = showOutputSchema;
         _hierarchicalPresenter.ShowSolverSettings = showSolverSettings;
         _hierarchicalPresenter.SimulationFavorites = () => _favoritesPresenter.Favorites();
         _view.SetChartView(chartPresenter.View);
         _view.SetPredictedVsObservedView(simulationPredictedVsObservedChartPresenter.View);
         _view.SetResidualsVsTimeView(simulationResidualVsTimeChartPresenter.View);
         _view.SetDataView(_simulationOutputMappingPresenter.View);
         AddSubPresenters(_chartPresenter, _hierarchicalPresenter, _simulationDiagramPresenter, _solverSettingsPresenter, _editOutputSchemaPresenter,
            _favoritesPresenter, _userDefinedParametersPresenter, _simulationOutputMappingPresenter, _simulationPredictedVsObservedChartPresenter,
            _simulationResidualVsTimeChartPresenter);
         _cacheShowPresenter = new Cache<Type, IEditInSimulationPresenter> { OnMissingKey = x => null };
         _chartPresenter.OnObservedDataAddedToChart += onObservedDataAddedToChart;
         _context = context;
         _outputMappingMatchingTask = outputMappingMatchingTask;
      }

      public string CreateResultTabCaption(string chartName)
      {
         return string.IsNullOrWhiteSpace(chartName) ? AppConstants.Captions.Results : chartName;
      }

      private void showSolverSettings()
      {
         _view.SetEditView(_solverSettingsPresenter.BaseView);
      }

      private void showOutputSchema()
      {
         _view.SetEditView(_editOutputSchemaPresenter.BaseView);
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _cacheShowPresenter.Each(p => p.ReleaseFrom(eventPublisher));
         _cacheShowPresenter.Clear();
      }

      public void Edit(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _hierarchicalPresenter.Edit(simulation);
         _solverSettingsPresenter.Edit(_simulation);
         _editOutputSchemaPresenter.Edit(_simulation);
         _favoritesPresenter.Edit(_simulation);
         _chartPresenter.UpdateTemplatesFor(_simulation);
         _view.SetEditView(_favoritesPresenter.BaseView);
         _simulationOutputMappingPresenter.EditSimulation(simulation);
         UpdateCaption();
         _view.Display();
         loadChart();
     }

      private void addObservedDataRepositories(IList<DataRepository> data, IEnumerable<Curve> curves)
      {
         foreach (var curve in curves.Where(c => c.IsObserved()))
         {
            data.AddUnique(curve.xData.Repository);
            data.AddUnique(curve.yData.Repository);
         }
      }

      public override void Edit(object subject)
      {
         Edit(subject.DowncastTo<IMoBiSimulation>());
      }

      public override object Subject => _simulation;

      private void loadChart()
      {
         CurveChartTemplate defaultTemplate = null;

         var data = new List<DataRepository>();
         if (_simulation.ResultsDataRepository != null)
            data.Add(_simulation.ResultsDataRepository);


         //This whole initialization of Chart in presenter is really ugly.
         //It's done like this now so that we can release 11.1 but should be done like in PK-Sim (multiple chart per simulation, added dynamically)
         if (_simulation.Chart == null)
         {
            _simulation.Chart = _chartFactory.Create<CurveChart>().WithAxes();
            _chartTask.SetOriginText(_simulation.Name, _simulation.Chart);
         }

         if (_simulation.PredictedVsObservedChart == null) 
            _simulation.PredictedVsObservedChart = _chartFactory.Create<SimulationPredictedVsObservedChart>();

         if(_simulationPredictedVsObservedChartPresenter.Chart == null)
            _simulationPredictedVsObservedChartPresenter.InitializeAnalysis(_simulation.PredictedVsObservedChart, _simulation);  
         else
            _simulationPredictedVsObservedChartPresenter.UpdateAnalysisBasedOn(_simulation);

         if (_simulation.ResidualVsTimeChart == null) 
            _simulation.ResidualVsTimeChart = _chartFactory.Create<SimulationResidualVsTimeChart>();

         if (_simulationResidualVsTimeChartPresenter.Chart == null) 
            _simulationResidualVsTimeChartPresenter.InitializeAnalysis(_simulation.ResidualVsTimeChart, _simulation);
         else
            _simulationResidualVsTimeChartPresenter.UpdateAnalysisBasedOn(_simulation);


         // Whether or not the chart is new, if it has no curves
         // we apply the simulation default template
         if (_simulation.Chart.Curves.Count == 0)
            defaultTemplate = _simulation.DefaultChartTemplate;

         addObservedDataRepositories(data, _simulation.Chart.Curves);
         _chartPresenter.Show(_simulation.Chart, data, defaultTemplate);
      }

      public void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         if (!_simulation.Equals(eventToHandle.Simulation))
            return;

         if (!_view.ShowsResults)
            _view.ShowResultsTab();

         _chartTask.SetOriginText(_simulation.Name, _simulation.Chart);

         loadChart();
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
         showPresenter.Simulation = _simulation;
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
         if (entity == null) return false;
         return _simulation.Model.Root.Equals(entity.RootContainer) || _simulation.Model.Neighborhoods.Equals(entity.RootContainer);
      }

      public void ZoomIn()
      {
         _simulationDiagramPresenter.Zoom(AppConstants.Diagram.Model.ZoomInFactor);
      }

      public void ZoomOut()
      {
         _simulationDiagramPresenter.Zoom(1 / AppConstants.Diagram.Model.ZoomInFactor);
      }

      public void FitToPage()
      {
         _simulationDiagramPresenter.Zoom(AppConstants.Diagram.Base.ZoomFitToPageFactor);
      }

      public void LayoutByForces()
      {
         _simulationDiagramPresenter.Layout(null, AppConstants.Diagram.Base.LayoutDepthChildren, null);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.SimulationCaption(_simulation.Name);
      }

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

      private bool canHandle(IObjectBaseEvent objectBaseEvent)
      {
         return Equals(_simulation, objectBaseEvent.ObjectBase);
      }

      private void onObservedDataAddedToChart(object sender, ObservedDataAddedToChartEventArgs e)
      {
         foreach (var dataRepository in e.AddedDataRepositories)
         {
            _outputMappingMatchingTask.AddMatchingOutputMapping(dataRepository, _simulation);
            _context.PublishEvent(new ObservedDataAddedToAnalysableEvent(_simulation, dataRepository, false));
         }
      }

      public void Handle(ObservedDataRemovedFromAnalysableEvent e)
      {
         if (_simulation == e.Analysable.DowncastTo<IMoBiSimulation>())
            _chartPresenter.RefreshSimulationChart();
      }
   }
}