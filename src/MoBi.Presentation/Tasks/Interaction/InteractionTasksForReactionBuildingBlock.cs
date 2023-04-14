using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForReactionBuildingBlock : InteractionTaskForCloneMergeBuildingBlock<MoBiReactionBuildingBlock, ReactionBuilder>
   {
      private readonly IReactionBuildingBlockMergeManager _reactionBuildingBlockMergeManager;
      private readonly IDiagramTask _diagramTask;
      private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;

      public InteractionTasksForReactionBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<MoBiReactionBuildingBlock> editTask,
         IInteractionTasksForBuilder<ReactionBuilder> builderTask,
         IReactionBuildingBlockMergeManager reactionBuildingBlockMergeManager,
         IDiagramTask diagramTask,
         IReactionBuildingBlockFactory reactionBuildingBlockFactory)
         : base(interactionTaskContext, editTask, builderTask, reactionBuildingBlockMergeManager)
      {
         _reactionBuildingBlockMergeManager = reactionBuildingBlockMergeManager;
         _diagramTask = diagramTask;
         _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
      }

      public override IMoBiCommand Merge(MoBiReactionBuildingBlock buildingBlockToMerge, MoBiReactionBuildingBlock targetBuildingBlock)
      {
         _reactionBuildingBlockMergeManager.SourceBuildingBlock = buildingBlockToMerge;
         _reactionBuildingBlockMergeManager.TargetBuildingBlock = targetBuildingBlock;

         return base.Merge(buildingBlockToMerge, targetBuildingBlock);
      }

      public override MoBiReactionBuildingBlock CreateNewEntity(MoBiProject parent)
      {
         return _reactionBuildingBlockFactory.Create();
      }

      protected override IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(ReactionBuilder builder, MoBiReactionBuildingBlock targetBuildingBlock, string originalBuilderName = null)
      {
         var macroCommand = base.GenerateAddCommandAndUpdateFormulaReferences(builder, targetBuildingBlock);
         macroCommand.Add(_diagramTask.MoveDiagramNodes(_reactionBuildingBlockMergeManager.SourceBuildingBlock, _reactionBuildingBlockMergeManager.TargetBuildingBlock, builder, originalBuilderName));
         macroCommand.Add(_interactionTaskContext.MoBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, builder));
         return macroCommand;
      }
   }
}