using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTaskForCloneMergeBuildingBlock<TBuildingBlock, TBuilder> : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<TBuildingBlock, TBuilder>
      where TBuilder : class, IContainer 
      where TBuildingBlock : class, IBuildingBlock<TBuilder>
   {
      protected IIgnoreReplaceCloneMergeManager<TBuilder> _ignoreReplaceCloneMergeManager;

      protected InteractionTaskForCloneMergeBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<TBuildingBlock> editTask,
         IInteractionTasksForBuilder<TBuilder> builderTask,
         IIgnoreReplaceCloneMergeManager<TBuilder> cloneMergeManager)
         : base(interactionTaskContext, editTask, builderTask)
      {
         _ignoreReplaceCloneMergeManager = cloneMergeManager;
      }

      public override IMoBiCommand Merge(TBuildingBlock buildingBlockToMerge, TBuildingBlock targetBuildingBlock)
      {
         if (targetBuildingBlock == null)
            return AddToProject(buildingBlockToMerge);

         var moBiMacroCommand = CreateMergeMacroCommand(targetBuildingBlock);

         _ignoreReplaceCloneMergeManager.RemoveAction = builder => moBiMacroCommand.Add(_builderTask.GetRemoveCommand(builder, targetBuildingBlock));
         _ignoreReplaceCloneMergeManager.AddAction = builder => moBiMacroCommand.Add(GenerateAddCommandAndUpdateFormulaReferences(builder, targetBuildingBlock, builder.Name));
         _ignoreReplaceCloneMergeManager.CloneAction = (builder, name) => moBiMacroCommand.Add(GenerateAddCommandAndUpdateFormulaReferences(builder, targetBuildingBlock, name));
         _ignoreReplaceCloneMergeManager.CancelAction = moBiMacroCommand.Clear;

         _ignoreReplaceCloneMergeManager.Merge(buildingBlockToMerge.ToCache(builder => builder.Name), targetBuildingBlock.ToCache(builder => builder.Name));

         moBiMacroCommand.Run(Context);

         return moBiMacroCommand;
      }
   }
}