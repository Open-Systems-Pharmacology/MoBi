using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForObserverBuildingBlock : InteractionTasksForEnumerableBuildingBlock<Module, ObserverBuildingBlock, ObserverBuilder>
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