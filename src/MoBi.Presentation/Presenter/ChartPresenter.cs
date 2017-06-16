using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Helper;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands.Core;
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

      void UpdateTemplatesBasedOn(IWithChartTemplates withChartTemplates);
      void Edit(CurveChart curveChart);
   }

   public abstract class ChartPresenter : AbstractCommandCollectorPresenter<IChartView, IChartPresenter>, IChartPresenter
   {
      protected readonly IMoBiContext _context;
      private readonly IUserSettings _userSettings;
      private readonly IChartTasks _chartTasks;
      private readonly IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      protected readonly IChartTemplatingTask _chartTemplatingTask;
      private readonly IDataColumnToPathElementsMapper _dataColumnToPathElementsMapper;
      private readonly IChartEditorLayoutTask _chartEditorLayoutTask;
      protected readonly ICache<DataRepository, IMoBiSimulation> _simulations;
      private IChartDisplayPresenter _displayPresenter;
      private IChartEditorPresenter _editorPresenter;
      private readonly string _nameProperty;
      private readonly ObservedDataDragDropBinder _observedDataDragDropBinder;
      private IWithChartTemplates _withChartTemplates;

      protected ChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings,
         IChartTasks chartTasks, IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter,
         IChartTemplatingTask chartTemplatingTask, IDataColumnToPathElementsMapper dataColumnToPathElementsMapper,
         IChartEditorLayoutTask chartEditorLayoutTask) :
         base(chartView)
      {
         _chartTasks = chartTasks;
         _chartEditorAndDisplayPresenter = chartEditorAndDisplayPresenter;
         initializeDisplayPresenter(chartEditorAndDisplayPresenter);
         initializeEditorPresenter(chartEditorAndDisplayPresenter);
         initializeNoCurvesHint();

         _chartTemplatingTask = chartTemplatingTask;
         _simulations = new Cache<DataRepository, IMoBiSimulation>(onMissingKey: x => null);

         _dataColumnToPathElementsMapper = dataColumnToPathElementsMapper;
         _chartEditorLayoutTask = chartEditorLayoutTask;
         _userSettings = userSettings;
         _context = context;

         _view.SetChartView(_chartEditorAndDisplayPresenter.BaseView);

         initLayout();
         initEditorPresenterSettings();


         _nameProperty = MoBiReflectionHelper.PropertyName<CurveChart>(x => x.Name);
         _observedDataDragDropBinder = new ObservedDataDragDropBinder();

         AddSubPresenters(_chartEditorAndDisplayPresenter);
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

      protected ICommand<IMoBiContext> AddAndExecute(ICommand<IMoBiContext> command)
      {
         AddCommand(command.Run(_context));
         return command;
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
         yield return _chartEditorAndDisplayPresenter.ChartLayoutButton;
      }

      private void initLayout()
      {
         _chartEditorLayoutTask.InitFromUserSettings(_chartEditorAndDisplayPresenter);
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
         repositories.SelectMany(x => x.ObservationColumns()).Each(observationColumn => _editorPresenter.AddCurveForColumn(observationColumn));
      }

      private void addObservedDataIfNeeded(IEnumerable<DataRepository> dataRepositories)
      {
         //curves are already selected. no need to add default selection
         if (Chart.Curves.Any()) return;
         addObservedData(dataRepositories.ToList());
      }

      public CurveChart Chart
      {
         get => _displayPresenter.Chart;
         protected set => Edit(value);
      }

      public void Clear()
      {
         releasePropertyChanged();

         _editorPresenter.Clear();
         _displayPresenter.Clear();
      }

      private void releasePropertyChanged()
      {
         if (Chart != null)
            Chart.PropertyChanged -= onPropertyChanged;
      }

      private void subscribePropertyChanged()
      {
         if (Chart != null)
            Chart.PropertyChanged += onPropertyChanged;
      }

      public void Edit(CurveChart curveChart)
      {
         releasePropertyChanged();

         _editorPresenter.Edit(curveChart);
         _displayPresenter.Edit(curveChart);

         subscribePropertyChanged();
      }

      private void onPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         MarkChartOwnerAsChanged();
         if (e.PropertyName.Equals(_nameProperty))
         {
            _context.PublishEvent(new RenamedEvent(Chart));
         }
      }

      public void Show(CurveChart chart, IReadOnlyList<DataRepository> dataRepositories, CurveChartTemplate defaultTemplate = null)
      {
         try
         {
            clearNoCurvesHint();
            //do not validate template when showing a chart as the chart might well be without curves when initialized for the first time.
            var currentTemplate = defaultTemplate ?? _chartTemplatingTask.TemplateFrom(chart, validateTemplate: false);
            replaceSimulationRepositories(dataRepositories);
            Chart = chart;
            LoadFromTemplate(currentTemplate, triggeredManually: false);

            addObservedDataIfNeeded(dataRepositories);
         }
         finally
         {
            initializeNoCurvesHint();
         }
      }

      public virtual void UpdateTemplatesBasedOn(IWithChartTemplates withChartTemplates)
      {
         _withChartTemplates = withChartTemplates;
         ResetMenus();
      }

      private void addDataRepositoriesToSimulation(IReadOnlyCollection<DataRepository> dataRepositories)
      {
         dataRepositories.Each(dataRepository =>
         {
            if (_simulations.Contains(dataRepository))
               return;

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

      protected GridColumnSettings Column(BrowserColumns browserColumns)
      {
         return _editorPresenter.DataBrowserColumnSettingsFor(browserColumns);
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

      protected virtual void ResetMenus()
      {
         ClearMenuButtons();
         AddChartTemplateMenu();
         AddMenuButtons();
      }

      protected void AddChartTemplateMenu()
      {
         if (_withChartTemplates == null)
            return;

         _editorPresenter.AddChartTemplateMenu(_withChartTemplates, curveChartTemplate =>
            LoadFromTemplate(_withChartTemplates.ChartTemplates.FindByName(curveChartTemplate.Name), triggeredManually: true));
      }

      public void Handle(ChartTemplatesChangedEvent eventToHandle)
      {
         if (canHandle(eventToHandle))
            ResetMenus();
      }

      private bool canHandle(ChartTemplatesChangedEvent eventToHandle)
      {
         return Equals(eventToHandle.WithChartTemplates, _withChartTemplates);
      }
   }
}