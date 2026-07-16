using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportExpressionProfileBuildingBlockSnapshotUICommand : ObjectUICommand<ExpressionProfileBuildingBlock>
   {
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTasksForExpressionProfileBuildingBlock;

      public ExportExpressionProfileBuildingBlockSnapshotUICommand(IInteractionTasksForExpressionProfileBuildingBlock interactionTasksForExpressionProfileBuildingBlock)
      {
         _interactionTasksForExpressionProfileBuildingBlock = interactionTasksForExpressionProfileBuildingBlock;
      }

      protected override void PerformExecute() => _interactionTasksForExpressionProfileBuildingBlock.ExportBuildingBlockSnapshot(Subject);
   }
}