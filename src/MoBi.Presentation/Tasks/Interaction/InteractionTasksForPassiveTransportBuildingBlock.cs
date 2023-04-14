using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForPassiveTransportBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<PassiveTransportBuildingBlock, TransportBuilder>
   {
      private readonly IMoleculeListTasks _moleculeListTasks;
      private readonly IPassiveTranportBuildingBlockMergeManager _mergeIgnoreReplaceMergeManager;
      private readonly IFormulaTask _formulaTask;
      private readonly IMoBiFormulaTask _moBiFormulaTask;

      public InteractionTasksForPassiveTransportBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<PassiveTransportBuildingBlock> editTask,
         IInteractionTasksForBuilder<TransportBuilder> builderTask,
         IMoleculeListTasks moleculeListTasks,
         IFormulaTask formulaTask,
         IPassiveTranportBuildingBlockMergeManager mergeIgnoreReplaceMergeManager,
         IMoBiFormulaTask moBiFormulaTask)
         : base(interactionTaskContext, editTask, builderTask)
      {
         _moleculeListTasks = moleculeListTasks;
         _formulaTask = formulaTask;
         _mergeIgnoreReplaceMergeManager = mergeIgnoreReplaceMergeManager;
         _moBiFormulaTask = moBiFormulaTask;
      }

      public override IMoBiCommand Merge(PassiveTransportBuildingBlock buildingBlockToMerge, PassiveTransportBuildingBlock targetBuildingBlock)
      {
         if (targetBuildingBlock == null)
            return AddToProject(buildingBlockToMerge);

         var moBiMacroCommand = CreateMergeMacroCommand(targetBuildingBlock);

         _mergeIgnoreReplaceMergeManager.RemoveAction = builder => moBiMacroCommand.Add(_builderTask.GetRemoveCommand(builder, targetBuildingBlock));
         _mergeIgnoreReplaceMergeManager.AddAction = builder => moBiMacroCommand.Add(GenerateAddCommandAndUpdateFormulaReferences(builder, targetBuildingBlock));
         _mergeIgnoreReplaceMergeManager.MergeAction = (target, merge) =>
         {
            if (_formulaTask.FormulasAreTheSame(target.Formula, merge.Formula))
            {
               _moleculeListTasks.MergeMoleculeLists(merge, target, targetBuildingBlock);
            }
         };
         _mergeIgnoreReplaceMergeManager.CancelAction = moBiMacroCommand.Clear;

         _mergeIgnoreReplaceMergeManager.Merge(buildingBlockToMerge.ToCache(builder => builder.Name), targetBuildingBlock.ToCache(builder => builder.Name));

         moBiMacroCommand.Run(Context);

         return moBiMacroCommand;
      }

      protected override IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(TransportBuilder builder, PassiveTransportBuildingBlock targetBuildingBlock, string originalBuilderName = null)
      {
         var macroCommand = base.GenerateAddCommandAndUpdateFormulaReferences(builder, targetBuildingBlock);
         macroCommand.Add(_moBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, builder));
         return macroCommand;
      }
   }
}