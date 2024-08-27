using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class ExportExpressionProfilesBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      public ExportExpressionProfilesBuildingBlockToExcelUICommand(
         IInteractionTasksForExpressionProfileBuildingBlock interactionTasks)
         : base(interactionTasks)
      {
      }
   }
}