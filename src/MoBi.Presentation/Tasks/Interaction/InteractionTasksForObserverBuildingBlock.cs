using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForObserverBuildingBlock : InteractionTasksForEnumerableBuildingBlock<ObserverBuildingBlock, ObserverBuilder>
   {
      private readonly IMoBiFormulaTask _moBiFormulaTask;

      public InteractionTasksForObserverBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<ObserverBuildingBlock> editTask,
         IInteractionTasksForBuilder<ObserverBuilder> builderTask,
         IMoBiFormulaTask moBiFormulaTask)
         : base(interactionTaskContext, editTask, builderTask)
      {
         _moBiFormulaTask = moBiFormulaTask;
      }
   }
}