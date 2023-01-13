using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForIndividualBuildingBlock : IInteractionTasksForBuildingBlock<IndividualBuildingBlock>, IInteractionTasksForPathAndValueEntity<IndividualBuildingBlock, IndividualParameter>
   {
   }

   public class InteractionTasksForIndividualBuildingBlock : InteractionTasksForPathAndValueEntity<IndividualBuildingBlock, IndividualParameter>, IInteractionTasksForIndividualBuildingBlock
   {
      public InteractionTasksForIndividualBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<IndividualBuildingBlock> editTask, IMoBiFormulaTask moBiFormulaTask) : base(interactionTaskContext, editTask, moBiFormulaTask)
      {
      }
   }
}