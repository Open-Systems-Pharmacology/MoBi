using MoBi.Presentation.Tasks;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class RemoveSummaryChartUICommand : ObjectUICommand<ICurveChart>
   {
      private readonly IChartTasks _chartTasks;

      public RemoveSummaryChartUICommand(IChartTasks chartTasks)
      {
         _chartTasks = chartTasks;
      }

      protected override void PerformExecute()
      {
         _chartTasks.Remove(Subject);
      }
   }
}