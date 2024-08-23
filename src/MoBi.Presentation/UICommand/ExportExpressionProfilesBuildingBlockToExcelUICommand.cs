using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class ExportExpressionProfilesBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      public ExportExpressionProfilesBuildingBlockToExcelUICommand(
          IInteractionTasksForExpressionProfileBuildingBlock interactionTasks,
          IExpressionProfileBuildingBlockToDataTableMapper mapper)
         : base(interactionTasks, mapper)
      {
      }
   }
}