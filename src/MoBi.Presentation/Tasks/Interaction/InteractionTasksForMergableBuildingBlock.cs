using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Presentation.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForMergableBuildingBlock<TBuildingBlock, TBuilder> : InteractionTasksForEnumerableBuildingBlock<TBuildingBlock, TBuilder> where TBuildingBlock : class, IBuildingBlock<TBuilder> where TBuilder : class, IObjectBase, IUsingFormula, IMoleculeDependentBuilder
   {
      protected IMergeIgnoreReplaceMergeManager<TBuilder> _mergeIgnoreReplaceMergeManager;
      protected IFormulaTask _formulaTask;
      protected IMoleculeListTasks _moleculeListTasks;

      protected InteractionTasksForMergableBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask, IInteractionTasksForBuilder<TBuilder> builderTask, IMergeIgnoreReplaceMergeManager<TBuilder> mergeIgnoreReplaceMergeManager, IFormulaTask formulaTask, IMoleculeListTasks moleculeListTasks) 
         : base(interactionTaskContext, editTask, builderTask)
      {
         _mergeIgnoreReplaceMergeManager = mergeIgnoreReplaceMergeManager;
         _formulaTask = formulaTask;
         _moleculeListTasks = moleculeListTasks;
      }

      public override IMoBiCommand Merge(TBuildingBlock buildingBlockToMerge, TBuildingBlock targetBuildingBlock)
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

   }
}