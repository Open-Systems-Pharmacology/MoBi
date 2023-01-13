using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForExpressionProfileBuildingBlock : IInteractionTasksForBuildingBlock<ExpressionProfileBuildingBlock>, IInteractionTasksForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
   }

   public class InteractionTasksForExpressionProfileBuildingBlock : InteractionTasksForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>, IInteractionTasksForExpressionProfileBuildingBlock
   {
      public InteractionTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock> editTask, IMoBiFormulaTask formulaTask) :
         base(interactionTaskContext, editTask, formulaTask)
      {
      }
   }
}