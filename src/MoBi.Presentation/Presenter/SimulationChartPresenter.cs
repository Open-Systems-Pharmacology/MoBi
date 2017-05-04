using System;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using IChartTemplatingTask = MoBi.Presentation.Tasks.IChartTemplatingTask;

namespace MoBi.Presentation.Presenter
{
   public interface ISimulationChartPresenter : IChartPresenter
   {
   }

   public class SimulationChartPresenter : ChartPresenter, ISimulationChartPresenter
   {
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;

      public SimulationChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings, IChartTasks chartTasks,
         IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter, IChartTemplatingTask chartTemplatingTask, IDataColumnToPathElementsMapper dataColumnToPathElementsMapper,
         IChartEditorLayoutTask chartEditorLayoutTask, IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper)
         : base(chartView, context, userSettings, chartTasks, chartEditorAndDisplayPresenter, chartTemplatingTask, dataColumnToPathElementsMapper, chartEditorLayoutTask)
      {
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
      }

      protected override bool CanDropSimulation => false;

      protected override void MarkChartOwnerAsChanged()
      {
         _simulations.Each(simulation => simulation.HasChanged = true);
      }

      protected override string CurveNameDefinition(DataColumn column)
      {
         var simulationForDataColumn = _simulations[column.Repository];

         return _quantityDisplayPathMapper.DisplayPathAsStringFor(simulationForDataColumn, column);
      }
   }
}