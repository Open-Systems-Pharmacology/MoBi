using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class MergeBuildingBlockUICommand<T> : ObjectUICommand<T> where T : class, IBuildingBlock
   {
      private readonly IMergeWorkflowTask _mergeWorkflowTask;

      public MergeBuildingBlockUICommand(IMergeWorkflowTask mergeWorkflowTask)
      {
         _mergeWorkflowTask = mergeWorkflowTask;
      }

      protected override void PerformExecute()
      {
         _mergeWorkflowTask.StartSingleBuildingBlockMerge(Subject);
      }
   }
}