using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForObserverBuildingBlock : InteractionTasksForMergableBuildingBlock<ObserverBuildingBlock, ObserverBuilder>
   {
      private readonly IMoBiFormulaTask _moBiFormulaTask;

      public InteractionTasksForObserverBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<ObserverBuildingBlock> editTask,
         IInteractionTasksForBuilder<ObserverBuilder> builderTask,
         IMoleculeListTasks moleculeListTasks,
         IObserverBuildingBlockMergeManager mergeIgnoreReplaceMergeManager,
         IFormulaTask formulaTask,
         IMoBiFormulaTask moBiFormulaTask)
         : base(interactionTaskContext, editTask, builderTask, mergeIgnoreReplaceMergeManager, formulaTask, moleculeListTasks)
      {
         _formulaTask = formulaTask;
         _moBiFormulaTask = moBiFormulaTask;
      }

      protected override IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(ObserverBuilder builder, ObserverBuildingBlock targetBuildingBlock, string originalBuilderName = null)
      {
         var macroCommand = CreateAddBuilderMacroCommand(builder, targetBuildingBlock);

         macroCommand.Add(_builderTask.GetAddCommand(builder, targetBuildingBlock));
         macroCommand.Add(_moBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, builder));

         return macroCommand;
      }
   }
}