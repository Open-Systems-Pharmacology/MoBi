using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForEnumerableBuildingBlock<TBuildingBlock, TBuilder> : InteractionTasksForBuildingBlock<TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock, IBuildingBlock<TBuilder>
      where TBuilder : class, IBuilder
   {
      protected readonly IInteractionTasksForBuilder<TBuilder> _builderTask;

      protected InteractionTasksForEnumerableBuildingBlock(
         IInteractionTaskContext interactionTaskContext, 
         IEditTasksForBuildingBlock<TBuildingBlock> editTask, 
         IInteractionTasksForBuilder<TBuilder> builderTask)
         : base(interactionTaskContext, editTask)
      {
         _builderTask = builderTask;
      }

      protected InteractionTasksForEnumerableBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask) : this(interactionTaskContext, editTask, null)
      {
         
      }

      protected abstract IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(TBuilder builder, TBuildingBlock targetBuildingBlock, string originalBuilderName = null);

      protected MoBiMacroCommand CreateAddBuilderMacroCommand(TBuilder builder, TBuildingBlock targetBuildingBlock)
      {
         var objectType = _interactionTaskContext.GetTypeFor<TBuilder>();
         return new MoBiMacroCommand
         {
            Description = AppConstants.Commands.AddToDescription(objectType, builder.Name, targetBuildingBlock.Name),
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = objectType
         };
      }
   }
}