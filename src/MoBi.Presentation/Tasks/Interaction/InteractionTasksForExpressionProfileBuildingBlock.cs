using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTaskForIndividualBuildingBlock: IInteractionTasksForBuildingBlock<IndividualBuildingBlock>, IInteractionTaskForPathAndValueEntity<IndividualBuildingBlock, IndividualParameter>
   {

   }

   public class InteractionTaskForIndividualBuildingBlock : InteractionTaskForPathAndValueEntity<IndividualBuildingBlock, IndividualParameter>, IInteractionTaskForIndividualBuildingBlock
   {
      public InteractionTaskForIndividualBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<IndividualBuildingBlock> editTask, IMoBiFormulaTask moBiFormulaTask) : base(interactionTaskContext, editTask, moBiFormulaTask)
      {
      }


   }

   public interface IInteractionTasksForExpressionProfileBuildingBlock : IInteractionTasksForBuildingBlock<ExpressionProfileBuildingBlock>, IInteractionTaskForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      
   }

   public class InteractionTasksForExpressionProfileBuildingBlock : InteractionTaskForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>, IInteractionTasksForExpressionProfileBuildingBlock
   {
      public InteractionTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock> editTask, IMoBiFormulaTask formulaTask) : 
         base(interactionTaskContext, editTask, formulaTask)
      {
      }


   }
}