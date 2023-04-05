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
   public class InteractionTasksForReactionBuildingBlock : InteractionTaskForCloneMergeBuildingBlock<IMoBiReactionBuildingBlock, IReactionBuilder>
   {
      private readonly IReactionBuildingBlockMergeManager _reactionBuildingBlockMergeManager;
      private readonly IDiagramTask _diagramTask;
      private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;

      public InteractionTasksForReactionBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<IMoBiReactionBuildingBlock> editTask,
         IInteractionTasksForBuilder<IReactionBuilder> builderTask,
         IReactionBuildingBlockMergeManager reactionBuildingBlockMergeManager,
         IDiagramTask diagramTask,
         IReactionBuildingBlockFactory reactionBuildingBlockFactory)
         : base(interactionTaskContext, editTask, builderTask, reactionBuildingBlockMergeManager)
      {
         _reactionBuildingBlockMergeManager = reactionBuildingBlockMergeManager;
         _diagramTask = diagramTask;
         _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
      }

      public override IMoBiCommand Merge(IMoBiReactionBuildingBlock buildingBlockToMerge, IMoBiReactionBuildingBlock targetBuildingBlock)
      {
         _reactionBuildingBlockMergeManager.SourceBuildingBlock = buildingBlockToMerge;
         _reactionBuildingBlockMergeManager.TargetBuildingBlock = targetBuildingBlock;

         return base.Merge(buildingBlockToMerge, targetBuildingBlock);
      }

      public override IMoBiReactionBuildingBlock CreateNewEntity(MoBiProject parent)
      {
         return _reactionBuildingBlockFactory.Create();
      }

      protected override IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(IReactionBuilder builder, IMoBiReactionBuildingBlock targetBuildingBlock, string originalBuilderName = null)
      {
         var macroCommand = base.GenerateAddCommandAndUpdateFormulaReferences(builder, targetBuildingBlock);
         macroCommand.Add(_diagramTask.MoveDiagramNodes(_reactionBuildingBlockMergeManager.SourceBuildingBlock, _reactionBuildingBlockMergeManager.TargetBuildingBlock, builder, originalBuilderName));
         macroCommand.Add(_interactionTaskContext.MoBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, builder));
         return macroCommand;
      }
   }
}