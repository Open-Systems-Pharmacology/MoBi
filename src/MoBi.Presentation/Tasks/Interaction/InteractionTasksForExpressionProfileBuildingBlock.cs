using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForExpressionProfileBuildingBlock : IInteractionTasksForBuildingBlock<ExpressionProfileBuildingBlock>, IInteractionTaskForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      
   }

   public class InteractionTasksForExpressionProfileBuildingBlock : InteractionTaskForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>, IInteractionTasksForExpressionProfileBuildingBlock
   {
      public InteractionTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock> editTask, IMoBiFormulaTask formulaTask) : 
         base(interactionTaskContext, editTask, formulaTask)
      {
      }

      public override IMoBiCommand Merge(ExpressionProfileBuildingBlock buildingBlockToMerge, ExpressionProfileBuildingBlock targetBuildingBlock)
      {
         // TODO
         return new MoBiEmptyCommand();
      }

      public override IMoBiCommand SetFormula(ExpressionProfileBuildingBlock buildingBlock, ExpressionParameter builder, IFormula formula)
      {
         return SetFormula(buildingBlock, builder, formula, shouldClearValue:builder.Value.HasValue);
      }

      protected override IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(ExpressionParameter builder, ExpressionProfileBuildingBlock targetBuildingBlock, string originalBuilderName = null)
      {
         // You cannot add ExpressionParameters to the buildingBlock
         return new MoBiMacroCommand();
      }

      protected override double? ValueFromBuilder(ExpressionParameter builder)
      {
         return builder.Value;
      }

      public override IMoBiCommand ChangeValueFormulaCommand(ExpressionProfileBuildingBlock buildingBlock, ExpressionParameter builder, IFormula formula)
      {
         return new ChangeValueFormulaCommand<ExpressionParameter>(buildingBlock, builder, formula, builder.Formula).Run(Context);
      }

      protected override IMoBiCommand SetValueWithUnit(ExpressionParameter builder, double? unitValueToBaseUnitValue, Unit unit, ExpressionProfileBuildingBlock startValues)
      {
         return new PathAndValueEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>(builder, unitValueToBaseUnitValue, unit, startValues).Run(Context);
      }
   }
}