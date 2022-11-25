using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_ExpressionValueChangedCommand : ContextSpecification<ValueWithPathEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>>
   {
      protected ExpressionProfileBuildingBlock _buildingBlock;
      protected double? _newValue;
      protected ExpressionParameter _expressionParameter;
      protected double? _oldValue;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _newValue = 3.0;
         _oldValue = null;
         _expressionParameter = new ExpressionParameter { StartValue = _oldValue };
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new ExpressionProfileBuildingBlock
         {
            _expressionParameter,
         };
         _buildingBlock.Id = "id";

         sut = new ValueWithPathEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>(_expressionParameter, _newValue, _expressionParameter.DisplayUnit, _buildingBlock);
      }
   }

   public class When_the_command_has_run : concern_for_ExpressionValueChangedCommand
   {
      protected override void Because()
      {
         sut.Run(_context);
      }

      [Observation]
      public void the_expression_parameter_value_should_be_updated()
      {
         _expressionParameter.StartValue.ShouldBeEqualTo(_newValue);
      }
   }

   public class When_the_command_has_been_reversed : concern_for_ExpressionValueChangedCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<ExpressionProfileBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }

      protected override void Because()
      {
         sut.Run(_context);
         sut.RestoreExecutionData(_context);
         sut.InverseCommand(_context).Run(_context);
      }

      [Observation]
      public void the_expression_parameter_value_should_be_updated()
      {
         _expressionParameter.StartValue.ShouldBeEqualTo(_oldValue);
      }
   }
}