using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Binders;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Settings;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using IChartTemplatingTask = MoBi.Presentation.Tasks.IChartTemplatingTask;

namespace MoBi.Presentation.Presenter
{
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
   }

   public abstract class ChartPresenter : ChartPresenter<CurveChart, IChartView, IChartPresenter>, IChartPresenter
   {
      protected readonly IMoBiContext _context;
      private readonly IUserSettings _userSettings;
      private readonly IChartTasks _chartTasks;
      protected readonly IChartTemplatingTask _chartTemplatingTask;
      private readonly IDataColumnToPathElementsMapper _dataColumnToPathElementsMapper;
      private readonly IChartEditorLayoutTask _chartEditorLayoutTask;
      protected readonly ICache<DataRepository, IMoBiSimulation> _simulations;
      private IChartDisplayPresenter _displayPresenter;
      private IChartEditorPresenter _editorPresenter;

      private readonly ObservedDataDragDropBinder _observedDataDragDropBinder;
      private readonly IChartUpdater _chartUpdater;
      private IWithChartTemplates _withChartTemplates;

      protected ChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings, IChartTasks chartTasks,
         IChartTemplatingTask chartTemplatingTask, IChartUpdater chartUpdater, ChartPresenterContext chartPresenterContext) :
         base(chartView, chartPresenterContext)
      {
         _chartTasks = chartTasks;
         initializeDisplayPresenter(chartPresenterContext.ChartEditorAndDisplayPresenter);
         initializeEditorPresenter(chartPresenterContext.ChartEditorAndDisplayPresenter);

         _chartTemplatingTask = chartTemplatingTask;
         _simulations = new Cache<DataRepository, IMoBiSimulation>(onMissingKey: x => null);

         _dataColumnToPathElementsMapper = chartPresenterContext.DataColumnToPathElementsMapper;
         _chartEditorLayoutTask = chartPresenterContext.EditorLayoutTask;
         _chartUpdater = chartUpdater;
         _userSettings = userSettings;
         _context = context;

         _view.SetChartView(chartPresenterContext.ChartEditorAndDisplayPresenter.BaseView);

         initLayout();
         initEditorPresenterSettings();

         _observedDataDragDropBinder = new ObservedDataDragDropBinder();

         AddSubPresenters(chartPresenterContext.ChartEditorAndDisplayPresenter);
      }

      private void initializeEditorPresenter(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter)
      {
         _editorPresenter = chartEditorAndDisplayPresenter.EditorPresenter;
         _editorPresenter.SetCurveNameDefinition(CurveNameDefinition);
      }

      private void initializeDisplayPresenter(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter)
      {
         _displayPresenter = chartEditorAndDisplayPresenter.DisplayPresenter;
         _displayPresenter.DragDrop += OnDragDrop;
         _displayPresenter.DragOver += OnDragOver;
         _displayPresenter.ExportToPDF = () => _chartTasks.ExportToPDF(Chart);
         initializeNoCurvesHint();
      }

      private void clearNoCurvesHint()
      {
         _displayPresenter.SetNoCurvesSelectedHint(string.Empty);
      }

      private void initializeNoCurvesHint()
      {
         _displayPresenter.SetNoCurvesSelectedHint(AppConstants.PleaseSelectCurveInChartEditor);
      }

      protected ChartOptions ChartOptions => _userSettings.ChartOptions;

      protected abstract string CurveNameDefinition(DataColumn column);

      private void initEditorPresenterSettings()
      {
         _editorPresenter.SetDisplayQuantityPathDefinition(displayPathForColumn);
         //Show all Columns
         _editorPresenter.SetShowDataColumnInDataBrowserDefinition(col => true);
      }

      private PathElements displayPathForColumn(DataColumn column)
      {
         var simulationForDataColumn = _simulations[column.Repository];
         var rootContainerForDataColumn = simulationForDataColumn?.Model.Root;
         return _dataColumnToPathElementsMapper.MapFrom(column, rootContainerForDataColumn);
      }

      //      protected ICommand<IMoBiContext> AddAndExecute(ICommand<IMoBiContext> command)
      //      {
      //         AddCommand(command.Run(_context));
      //         return command;
      //      }

      protected override ISimulation SimulationFor(DataColumn dataColumn)
      {
         return findSimulation(dataColumn.Repository);
      }

      protected void AddMenuButtons()
      {
         AllMenuButtons().Each(_editorPresenter.AddButton);
         _editorPresenter.AddUsedInMenuItem();
      }

      protected void ClearMenuButtons()
      {
         _editorPresenter.ClearButtons();
      }

      protected virtual IEnumerable<IMenuBarItem> AllMenuButtons()
      {
         yield return _chartPresenterContext.ChartEditorAndDisplayPresenter.ChartLayoutButton;
      }

      private void initLayout()
      {
         _chartEditorLayoutTask.InitFromUserSettings(_chartPresenterContext.ChartEditorAndDisplayPresenter);
      }

      protected void LoadFromTemplate(CurveChartTemplate chartTemplate, bool triggeredManually)
      {
         _chartTemplatingTask.InitFromTemplate(_simulations, Chart, _editorPresenter, chartTemplate, CurveNameDefinition, triggeredManually);
      }

      protected virtual void OnDragOver(object sender, DragEventArgs e)
      {
         if (simulationResultsIsBeingDragged(e))
            e.Effect = CanDropSimulation ? DragDropEffects.Move : DragDropEffects.None;
         else
            _observedDataDragDropBinder.PrepareDrag(e);
      }

      private static bool simulationResultsIsBeingDragged(DragEventArgs e)
      {
         var data = e.Data<IList<ITreeNode>>();
         //do not use null propagation as suggested by resharper
         // ReSharper disable once UseNullPropagation
         if (data == null)
            return false;

         return data.Count == data.OfType<HistoricalResultsNode>().Count();
      }

      protected virtual void OnDragDrop(object sender, DragEventArgs e)
      {
         if (simulationResultsIsBeingDragged(e) && CanDropSimulation)
         {
            var historicalResultsNodes = e.Data<IList<ITreeNode>>().OfType<HistoricalResultsNode>();
            addHistoricalResults(historicalResultsNodes.Select(result => result.Tag).ToList());
         }
         else
         {
            var droppedObservedData = _observedDataDragDropBinder.DroppedObservedDataFrom(e);
            addObservedData(droppedObservedData.ToList());
         }
      }

      protected abstract bool CanDropSimulation { get; }

      protected abstract void MarkChartOwnerAsChanged();

      private void addHistoricalResults(IReadOnlyList<DataRepository> repositories)
      {
         addDataRepositoriesToSimulation(repositories);
         _editorPresenter.AddDataRepositories(repositories);
         repositories.Each(repository => repository.SetPersistable(persistable: true));
      }

      private void addObservedData(IReadOnlyList<DataRepository> repositories)
      {
         _editorPresenter.AddDataRepositories(repositories);
         using (_chartUpdater.UpdateTransaction(Chart))
         {
            repositories.SelectMany(x => x.ObservationColumns()).Each(observationColumn => _editorPresenter.AddCurveForColumn(observationColumn));
         }
      }

      private void addObservedDataIfNeeded(IEnumerable<DataRepository> dataRepositories)
      {
         //curves are already selected. no need to add default selection
         if (Chart.Curves.Any()) return;
         addObservedData(dataRepositories.ToList());
      }

      protected override void ChartChanged()
      {
         base.ChartChanged();
         MarkChartOwnerAsChanged();
      }

      public void Show(CurveChart chart, IReadOnlyList<DataRepository> dataRepositories, CurveChartTemplate defaultTemplate = null)
      {
         try
         {
            clearNoCurvesHint();
            //do not validate template when showing a chart as the chart might well be without curves when initialized for the first time.
            var currentTemplate = defaultTemplate ?? _chartTemplatingTask.TemplateFrom(chart, validateTemplate: false);
            replaceSimulationRepositories(dataRepositories);
            InitializeAnalysis(chart);
            UpdateTemplatesBasedOn(_withChartTemplates);
            BindChartToEditors();
            LoadFromTemplate(currentTemplate, triggeredManually: false);

            addObservedDataIfNeeded(dataRepositories);
         }
         finally
         {
            initializeNoCurvesHint();
         }
      }

      public virtual void UpdateTemplatesFor(IWithChartTemplates withChartTemplates)
      {
         _withChartTemplates = withChartTemplates;
         UpdateTemplatesBasedOn(withChartTemplates);
      }

      private void addDataRepositoriesToSimulation(IReadOnlyCollection<DataRepository> dataRepositories)
      {
         dataRepositories.Where(dataRepository => !_simulations.Contains(dataRepository))
            .Each(dataRepository =>
            {
               var simulation = findSimulation(dataRepository) ?? findHistoricSimulation(dataRepository);
               if (simulation != null)
                  _simulations.Add(dataRepository, simulation);
            });
      }

      private IMoBiSimulation findSimulation(DataRepository dataRepository)
      {
         return _context.CurrentProject.Simulations
            .FirstOrDefault(simulation => Equals(simulation.Results, dataRepository));
      }

      private IMoBiSimulation findHistoricSimulation(DataRepository dataRepository)
      {
         return _context.CurrentProject.Simulations.FirstOrDefault(simulation => simulation.HistoricResults.Contains(dataRepository));
      }

      private void replaceSimulationRepositories(IReadOnlyCollection<DataRepository> dataRepositories)
      {
         var repositoriesToRemove = _simulations.Keys.Except(dataRepositories).ToList();
         repositoriesToRemove.Each(_simulations.Remove);

         addDataRepositoriesToSimulation(dataRepositories);

         _editorPresenter.AddDataRepositories(dataRepositories);

         var repositories = _editorPresenter.AllDataColumns.Select(col => col.Repository).Distinct();
         repositoriesToRemove = repositories.Except(dataRepositories).ToList();
         _editorPresenter.RemoveDataRepositories(repositoriesToRemove);
      }

      public void Handle(ObservedDataRemovedEvent eventToHandle)
      {
         var dataRepository = eventToHandle.DataRepository;
         _editorPresenter.RemoveDataRepositories(new[] {dataRepository});
         _displayPresenter.Refresh();
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _displayPresenter.DragDrop -= OnDragDrop;
         _displayPresenter.DragOver -= OnDragOver;
         Clear();
      }
   }
}