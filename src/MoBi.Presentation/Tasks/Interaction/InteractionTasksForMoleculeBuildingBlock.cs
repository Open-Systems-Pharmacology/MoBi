using MoBi.Core.Commands;
using MoBi.Core.Events;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForMoleculeBuildingBlock : IInteractionTasksForBuildingBlock<Module, MoleculeBuildingBlock>
   {
      void Edit(MoleculeBuildingBlock moleculeBuildingBlock, MoleculeBuilder moleculeBuilder);
   }

   public class InteractionTasksForMoleculeBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<Module, MoleculeBuildingBlock, MoleculeBuilder>, IInteractionTasksForMoleculeBuildingBlock
   {
      private readonly IEditTasksForBuildingBlock<MoleculeBuildingBlock> _editTaskForBuildingBlock;

      public InteractionTasksForMoleculeBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<MoleculeBuildingBlock> editTask,
         IInteractionTasksForBuilder<MoleculeBuilder> builderTask)
         : base(interactionTaskContext, editTask, builderTask)
      {
         _editTaskForBuildingBlock = editTask;
      }

      public override IMoBiCommand GetRemoveCommand(MoleculeBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<MoleculeBuildingBlock>(objectToRemove, parent);
      }

      public override IMoBiCommand GetAddCommand(MoleculeBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<MoleculeBuildingBlock>(itemToAdd, parent);
      }

      public void Edit(MoleculeBuildingBlock moleculeBuildingBlock, MoleculeBuilder moleculeBuilder)
      {
         _editTaskForBuildingBlock.EditBuildingBlock(moleculeBuildingBlock);
         Context.PublishEvent(new EntitySelectedEvent(moleculeBuilder, this));
      }
   }
}