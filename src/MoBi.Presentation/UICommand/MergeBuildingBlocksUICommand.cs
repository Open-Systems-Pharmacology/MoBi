using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   public class MergeBuildingBlocksUICommand : IUICommand
   {
      private readonly IMergeWorkflowTask _workflowsTask;

      public MergeBuildingBlocksUICommand(IMergeWorkflowTask workflowsTask)
      {
         _workflowsTask = workflowsTask;
      }

      public void Execute()
      {
         _workflowsTask.StartSimulationMerge();
      }
   }
}