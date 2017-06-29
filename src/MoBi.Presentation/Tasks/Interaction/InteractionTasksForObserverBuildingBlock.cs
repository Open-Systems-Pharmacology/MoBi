using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForObserverBuildingBlock : InteractionTasksForMergableBuildingBlock<IObserverBuildingBlock, IObserverBuilder>
   {
      private readonly IMoBiFormulaTask _moBiFormulaTask;

      public InteractionTasksForObserverBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<IObserverBuildingBlock> editTask,
         IInteractionTasksForBuilder<IObserverBuilder> builderTask,
         IMoleculeListTasks moleculeListTasks,
         IObserverBuildingBlockMergeManager mergeIgnoreReplaceMergeManager,
         IFormulaTask formulaTask,
         IMoBiFormulaTask moBiFormulaTask)
         : base(interactionTaskContext, editTask, builderTask, mergeIgnoreReplaceMergeManager, formulaTask, moleculeListTasks)
      {
         _formulaTask = formulaTask;
         _moBiFormulaTask = moBiFormulaTask;
      }

      protected override IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(IObserverBuilder builder, IObserverBuildingBlock targetBuildingBlock, string originalBuilderName = null)
      {
         var macroCommand = CreateAddBuilderMacroCommand(builder, targetBuildingBlock);

         macroCommand.Add(_builderTask.GetAddCommand(builder, targetBuildingBlock));
         macroCommand.Add(_moBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, builder));

         return macroCommand;
      }
   }
}