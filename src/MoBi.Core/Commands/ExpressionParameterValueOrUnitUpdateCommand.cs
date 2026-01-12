using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands;

public class ExpressionParameterValueOrUnitUpdateCommand : PathAndValueEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>
{
   public ExpressionParameterValueOrUnitUpdateCommand(ExpressionParameter builder, ExpressionParameterValueUpdate expressionParameterValueUpdate, ExpressionProfileBuildingBlock buildingBlock) : base(builder, expressionParameterValueUpdate.UpdatedValue, builder.DisplayUnit, buildingBlock)
   {
         
   }

   protected override void ExecuteWith(IMoBiContext context)
   {
      base.ExecuteWith(context);

      // In this context we are not storing the initial state because we are setting the initial state as queried from PK-Sim
      _builder.InitialValue = null;
      _builder.InitialFormulaId = null;
      _builder.InitialUnit = null;
   }
}