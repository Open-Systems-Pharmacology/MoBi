using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class ExpressionParameterValueOrUnitChangedCommand : ValueWithPathEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>
   {
      public ExpressionParameterValueOrUnitChangedCommand(ExpressionParameter builder, double? newBaseValue, Unit newDisplayUnit, ExpressionProfileBuildingBlock buildingBlock) : base(builder, newBaseValue, newDisplayUnit, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ExpressionParameterValueOrUnitChangedCommand(_builder, _oldBaseValue, _oldDisplayUnit, _buildingBlock).AsInverseFor(this);
      }

      protected override double? GetOldValue()
      {
         return _builder.Value;
      }

      protected override IObjectPath GetPath()
      {
         return _builder.Path;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _builder = _buildingBlock.Single(expressionParameter => expressionParameter.Path.Equals(_valuePath));
      }

      protected override void SetNewValue(double? newBaseValue)
      {
         _builder.Value = newBaseValue;
      }
   }
}