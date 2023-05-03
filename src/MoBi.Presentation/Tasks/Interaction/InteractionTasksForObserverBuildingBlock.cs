using MoBi.Core.Commands;
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

      public override IMoBiCommand GetRemoveCommand(ObserverBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<ObserverBuildingBlock>(objectToRemove, parent);
      }

      public override IMoBiCommand GetAddCommand(ObserverBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<ObserverBuildingBlock>(itemToAdd, parent);
      }
   }
}