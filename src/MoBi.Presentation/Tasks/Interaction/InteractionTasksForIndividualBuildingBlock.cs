using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForIndividualBuildingBlock : IInteractionTasksForProjectBuildingBlock<IndividualBuildingBlock>, 
      IInteractionTasksForPathAndValueEntity<MoBiProject, IndividualBuildingBlock, IndividualParameter>,
      IInteractionTasksForProjectBuildingBlock
   {
   }

   public class InteractionTasksForIndividualBuildingBlock : InteractionTasksForProjectPathAndValueEntityBuildingBlocks<IndividualBuildingBlock, IndividualParameter>, IInteractionTasksForIndividualBuildingBlock
   {
      public InteractionTasksForIndividualBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<IndividualBuildingBlock> editTask, IMoBiFormulaTask moBiFormulaTask) : base(interactionTaskContext, editTask, moBiFormulaTask)
      {
      }
   }
}