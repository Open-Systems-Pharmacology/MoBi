using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class ShowBuildingBlockDiffUICommand : IUICommand
   {
      private readonly ISimulationComparisonTask _simulationComparisonTask;
      private IBuildingBlock _simulationBuildingBlock;
      private readonly ITemplateResolverTask _templateResolverTask;

      public ShowBuildingBlockDiffUICommand(ISimulationComparisonTask simulationComparisonTask, ITemplateResolverTask templateResolverTask)
      {
         _templateResolverTask = templateResolverTask;
         _simulationComparisonTask = simulationComparisonTask;
      }

      public ShowBuildingBlockDiffUICommand Initialize(IBuildingBlock simulationBuildingBlock)
      {
         _simulationBuildingBlock = simulationBuildingBlock;
         return this;
      }

      public void Execute()
      {
         _simulationComparisonTask.ShowDifferencesBetween(_templateResolverTask.TemplateBuildingBlockFor(_simulationBuildingBlock), _simulationBuildingBlock);
      }
   }
}