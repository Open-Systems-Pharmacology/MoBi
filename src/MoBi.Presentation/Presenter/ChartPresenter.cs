using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Binders;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using IChartTemplatingTask = MoBi.Presentation.Tasks.IChartTemplatingTask;

namespace MoBi.Presentation.Presenter
{
   public class ObservedDataAddedToChartEventArgs : EventArgs
   {
      public IReadOnlyList<DataRepository> AddedDataRepositories { get; set; }
   }

   /// <summary>
   ///    Aggregates the presenters needed to show a chart
   /// </summary>
   /// <remarks>
   ///    Using facade pattern
   /// </remarks>
   public interface IChartPresenter : IPresenter<IChartView>,
      IListener<ObservedDataRemovedEvent>,
      IListener<ChartTemplatesChangedEvent>
   {
      CurveChart Chart { get; }

      /// <summary>
      ///    Shows the specified chart with underlying data.
      /// </summary>
      /// <param name="chart">The chart to display</param>
      /// <param name="dataRepositories">The data used in the chart</param>
      /// <param name="defaultTemplate">If specified, this template will be used to initialize the chart</param>
      /// <remarks>
      ///    This method ensures the correct order of parameter setting
      /// </remarks>
      void Show(CurveChart chart, IReadOnlyList<DataRepository> dataRepositories, CurveChartTemplate defaultTemplate = null);

      void UpdateTemplatesFor(IWithChartTemplates withChartTemplates);
      event EventHandler<ObservedDataAddedToChartEventArgs> OnObservedDataAddedToChart;
   }

   public abstract class ChartPresenter : ChartPresenter<CurveChart, IChartView, IChartPresenter>, IChartPresenter
   {
      public event EventHandler<ObservedDataAddedToChartEventArgs> OnObservedDataAddedToChart = delegate { };

      protected readonly IMoBiContext _context;
      private readonly IUserSettings _userSettings;
      private readonly IChartUpdater _chartUpdater;
      protected readonly IChartTemplatingTask _chartTemplatingTask;
      protected readonly ICache<DataRepository, IMoBiSimulation> _dataRepositoryCache;

      private readonly IOutputMappingMatchingTask _outputMappingMatchingTask;
      private readonly ObservedDataDragDropBinder _observedDataDragDropBinder;
      private bool _initialized;

      private IChartDisplayPresenter displayPresenter => _chartPresenterContext.DisplayPresenter;
      private IChartEditorPresenter editorPresenter => _chartPresenterContext.EditorPresenter;

      protected ChartPresenter(IChartView chartView, ChartPresenterContext chartPresenterContext, IMoBiContext context, IUserSettings userSettings,
         IChartTemplatingTask chartTemplatingTask, IChartUpdater chartUpdater, IOutputMappingMatchingTask outputMappingMatchingTask) :
         base(chartView, chartPresenterContext)
      {
         _chartUpdater = chartUpdater;
         initializeDisplayPresenter();
         initializeEditorPresenter();

         _chartTemplatingTask = chartTemplatingTask;
         _dataRepositoryCache = new Cache<DataRepository, IMoBiSimulation>(onMissingKey: x => null);
         _outputMappingMatchingTask = outputMappingMatchingTask;

         _userSettings = userSettings;
         _context = context;

         _view.SetChartView(chartPresenterContext.EditorAndDisplayPresenter.BaseView);

         initLayout();
         initEditorPresenterSettings();

         _observedDataDragDropBinder = new ObservedDataDragDropBinder();

         AddSubPresenters(chartPresenterContext.EditorAndDisplayPresenter);
      }

      private void initializeEditorPresenter()
      {
         editorPresenter.SetCurveNameDefinition(CurveNameDefinition);
      }

      private void initializeDisplayPresenter()
      {
         displayPresenter.DragDrop += OnDragDrop;
         displayPresenter.DragOver += OnDragOver;
         initializeNoCurvesHint();
      }

      private void clearNoCurvesHint()
      {
         displayPresenter.SetNoCurvesSelectedHint(string.Empty);
      }

      private void initializeNoCurvesHint()
      {
         displayPresenter.SetNoCurvesSelectedHint(AppConstants.PleaseSelectCurveInChartEditor);
      }

      protected ChartOptions ChartOptions => _userSettings.ChartOptions;

      protected abstract string CurveNameDefinition(DataColumn column);

      private void initEditorPresenterSettings()
      {
         editorPresenter.SetDisplayQuantityPathDefinition(displayPathForColumn);
         //Show all Columns
         editorPresenter.SetShowDataColumnInDataBrowserDefinition(col => true);
      }

      private PathElements displayPathForColumn(DataColumn column)
      {
         var simulationForDataColumn = _dataRepositoryCache[column.Repository];
         var rootContainerForDataColumn = simulationForDataColumn?.Model.Root;
         return _chartPresenterContext.DataColumnToPathElementsMapper.MapFrom(column, rootContainerForDataColumn);
      }

      protected override ISimulation SimulationFor(DataColumn dataColumn)
      {
         return findSimulation(dataColumn.Repository);
      }

      protected void AddMenuButtons()
      {
         AllMenuButtons().Each(editorPresenter.AddButton);
         editorPresenter.AddUsedInMenuItem();
      }

      protected void ClearMenuButtons()
      {
         editorPresenter.ClearButtons();
      }

      protected virtual IEnumerable<IMenuBarItem> AllMenuButtons()
      {
         yield return _chartPresenterContext.EditorAndDisplayPresenter.ChartLayoutButton;
      }

      private void initLayout()
      {
         _chartPresenterContext.EditorLayoutTask.InitFromUserSettings(_chartPresenterContext.EditorAndDisplayPresenter);
      }

      protected void LoadFromTemplate(CurveChartTemplate chartTemplate, bool triggeredManually, bool propagateChartChangeEvent = true)
      {
         _chartTemplatingTask.InitFromTemplate(_dataRepositoryCache, Chart, editorPresenter, chartTemplate, CurveNameDefinition, triggeredManually,
            propagateChartChangeEvent);
      }

      protected virtual void OnDragOver(object sender, IDragEvent e)
      {
         if (simulationResultsIsBeingDragged(e))
            e.Effect = CanDropSimulation ? DragEffect.Move : DragEffect.None;
         else
            _observedDataDragDropBinder.PrepareDrag(e);
      }

      private static bool simulationResultsIsBeingDragged(IDragEvent e)
      {
         var data = e.Data<IList<ITreeNode>>();
         //do not use null propagation as suggested by resharper
         // ReSharper disable once UseNullPropagation
         if (data == null)
            return false;

         return data.Count == data.OfType<HistoricalResultsNode>().Count();
      }

      protected virtual void OnDragDrop(object sender, IDragEvent e)
      {
         if (simulationResultsIsBeingDragged(e) && CanDropSimulation)
         {
            var historicalResultsNodes = e.Data<IList<ITreeNode>>().OfType<HistoricalResultsNode>();
            addHistoricalResults(historicalResultsNodes.Select(result => result.Tag).ToList());
         }
         else
         {
            if (_userSettings.ColorGroupObservedDataFromSameFolder)
            {
               var droppedObservedDataWithFolderAddress = _observedDataDragDropBinder.DroppedObservedDataWithFolderPathFrom(e);
               addColorGroupedObservedData(droppedObservedDataWithFolderAddress);
            }
            else
            {
               var droppedObservedData = _observedDataDragDropBinder.DroppedObservedDataFrom(e);
               addObservedData(droppedObservedData.ToList(), addDataColumnsToEditorPresenter);
               _chartPresenterContext.Refresh();
            }
         }
      }

      private void addDataColumnsToEditorPresenter(IEnumerable<DataColumn> dataColumns)
      {
         dataColumns.Each(dataColumn => editorPresenter.AddCurveForColumn(dataColumn));
      }

      protected abstract bool CanDropSimulation { get; }

      protected abstract void MarkChartOwnerAsChanged();

      private void addColorGroupedObservedData(IReadOnlyList<IReadOnlyList<DataRepository>> observedData)
      {
         using (_chartUpdater.UpdateTransaction(Chart))
         {
            foreach (var observedDataNodesList in observedData)
            {
               addObservedData(observedDataNodesList, dataColumns => editorPresenter.AddCurvesWithSameColorForColumn(dataColumns.ToList()));
            }
         }
      }

      private void addObservedData(IReadOnlyList<DataRepository> repositories, Action<IEnumerable<DataColumn>> addAction)
      {

         editorPresenter.AddDataRepositories(repositories);
         addAction(repositories.SelectMany(x => x.ObservationColumns()));
         OnObservedDataAddedToChart(this, new ObservedDataAddedToChartEventArgs { AddedDataRepositories = repositories });
      }

      private void addHistoricalResults(IReadOnlyList<DataRepository> repositories)
      {
         addDataRepositoriesToDataRepositoryCache(repositories);
         editorPresenter.AddDataRepositories(repositories);
         repositories.Each(repository => repository.SetPersistable(persistable: true));
      }

      private void addObservedDataIfNeeded(IEnumerable<DataRepository> dataRepositories)
      {
         //curves are already selected. no need to add default selection
         if (Chart.Curves.Any()) 
            return;
         addObservedData(dataRepositories.ToList(), addDataColumnsToEditorPresenter);
         _chartPresenterContext.Refresh();
      }

      protected override void ChartChanged()
      {
         base.ChartChanged();
         MarkChartOwnerAsChanged();
      }

      public override void InitializeAnalysis(CurveChart chart)
      {
         if (_initialized) return;
         //this will prevent re-initializing the layout if this was already done (no UI flicker)
         base.InitializeAnalysis(chart);
         _initialized = true;
      }

      public void Show(CurveChart chart, IReadOnlyList<DataRepository> dataRepositories, CurveChartTemplate defaultTemplate = null)
      {
         try
         {
            clearNoCurvesHint();
            InitializeAnalysis(chart);

            //do not validate template when showing a chart as the chart might well be without curves when initialized for the first time.
            var currentTemplate = defaultTemplate ?? _chartTemplatingTask.TemplateFrom(chart, validateTemplate: false);
            replaceSimulationRepositories(dataRepositories);
            LoadFromTemplate(currentTemplate, triggeredManually: false, propagateChartChangeEvent: false);
            addObservedDataIfNeeded(dataRepositories);
         }
         finally
         {
            initializeNoCurvesHint();
         }
      }

      public virtual void UpdateTemplatesFor(IWithChartTemplates withChartTemplates)
      {
         UpdateTemplatesBasedOn(withChartTemplates);
      }

      private void addDataRepositoriesToDataRepositoryCache(IReadOnlyCollection<DataRepository> dataRepositories)
      {
         dataRepositories.Where(dataRepository => !_dataRepositoryCache.Contains(dataRepository))
            .Each(dataRepository =>
            {
               var simulation = findSimulation(dataRepository) ?? findHistoricSimulation(dataRepository);
               if (simulation == null) return;

               _dataRepositoryCache.Add(dataRepository, simulation);
               ChartEditorPresenter.AddOutputMappings(simulation.OutputMappings);
               _outputMappingMatchingTask.AddMatchingOutputMapping(dataRepository, simulation);

               _context.PublishEvent(new ObservedDataAddedToAnalysableEvent(simulation, dataRepository, false));
            });
      }

      private IMoBiSimulation findSimulation(DataRepository dataRepository)
      {
         return _context.CurrentProject.Simulations
            .FirstOrDefault(simulation => Equals(simulation.ResultsDataRepository, dataRepository));
      }

      private IMoBiSimulation findHistoricSimulation(DataRepository dataRepository)
      {
         return _context.CurrentProject.Simulations.FirstOrDefault(simulation => simulation.HistoricResults.Contains(dataRepository));
      }

      private void replaceSimulationRepositories(IReadOnlyCollection<DataRepository> dataRepositories)
      {
         var repositoriesToRemove = _dataRepositoryCache.Keys.Except(dataRepositories).ToList();
         var simulationsToRemove = _dataRepositoryCache.KeyValues.Where(x => repositoriesToRemove.Contains(x.Key)).Select(x => x.Value).ToList();
         repositoriesToRemove.Each(_dataRepositoryCache.Remove);
         simulationsToRemove.Each(simulation => ChartEditorPresenter.RemoveOutputMappings(simulation.OutputMappings));

         addDataRepositoriesToDataRepositoryCache(dataRepositories);

         editorPresenter.AddDataRepositories(dataRepositories);

         var repositories = editorPresenter.AllDataColumns.Select(col => col.Repository).Distinct();
         repositoriesToRemove = repositories.Except(dataRepositories).ToList();
         editorPresenter.RemoveDataRepositories(repositoriesToRemove);
      }

      public void Handle(ObservedDataRemovedEvent eventToHandle)
      {
         var dataRepository = eventToHandle.DataRepository;
         editorPresenter.RemoveDataRepositories(new[] { dataRepository });
         displayPresenter.Refresh();
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         displayPresenter.DragDrop -= OnDragDrop;
         displayPresenter.DragOver -= OnDragOver;
         Clear();
      }
   }
}