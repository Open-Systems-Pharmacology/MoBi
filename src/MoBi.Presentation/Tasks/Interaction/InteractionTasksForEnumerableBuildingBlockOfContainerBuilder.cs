using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Helper;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<TBuildingBlock, TBuilder> 
      : InteractionTasksForEnumerableBuildingBlock<TBuildingBlock, TBuilder> 
      where TBuilder : class, IContainer, IBuilder where TBuildingBlock : class, IBuildingBlock<TBuilder>
   {
      protected InteractionTasksForEnumerableBuildingBlockOfContainerBuilder(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask, IInteractionTasksForBuilder<TBuilder> builderTask) : base(interactionTaskContext, editTask, builderTask)
      {
      }

      protected InteractionTasksForEnumerableBuildingBlockOfContainerBuilder(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask) : base(interactionTaskContext, editTask)
      {
      }

      protected override IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(TBuilder builder, TBuildingBlock targetBuildingBlock, string originalBuilderName = null)
      {
         var rhsFormulaDecoder = new RHSFormulaDecoder();
         var macroCommand = CreateAddBuilderMacroCommand(builder, targetBuildingBlock);

         macroCommand.Add(_builderTask.GetAddCommand(builder, targetBuildingBlock));

         builder.GetAllChildren<IUsingFormula>().Each(
            usingFormula => macroCommand.Add(_interactionTaskContext.MoBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, usingFormula)));

         builder.GetAllChildren<IParameter>().Each(parameter =>
            macroCommand.Add(_interactionTaskContext.MoBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, parameter, rhsFormulaDecoder))
            );

         return macroCommand;
      }
   }
}