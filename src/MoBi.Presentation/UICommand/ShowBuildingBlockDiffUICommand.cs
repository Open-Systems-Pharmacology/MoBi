using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ShowBuildingBlockDiffUICommand : ObjectUICommand<IBuildingBlock>
   {
      private readonly ISimulationComparisonTask _simulationComparisonTask;
      private readonly ITemplateResolverTask _templateResolverTask;

      public ShowBuildingBlockDiffUICommand(ISimulationComparisonTask simulationComparisonTask, ITemplateResolverTask templateResolverTask)
      {
         _templateResolverTask = templateResolverTask;
         _simulationComparisonTask = simulationComparisonTask;
      }

      protected override void PerformExecute()
      {
         _simulationComparisonTask.ShowDifferencesBetween(_templateResolverTask.TemplateBuildingBlockFor(Subject), Subject);
      }
   }
}