using System.Collections.Generic;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class RemoveMultipleSummaryChartUICommand : ObjectUICommand<IReadOnlyList<CurveChart>>
   {
      private readonly IChartTasks _chartTasks;

      public RemoveMultipleSummaryChartUICommand(IChartTasks chartTasks)
      {
         _chartTasks = chartTasks;
      }

      protected override void PerformExecute()
      {
         _chartTasks.RemoveMultipleSummaryCharts(Subject);
      }
   }
}