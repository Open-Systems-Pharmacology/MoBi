using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class concern_for_ExpressionValueChangedCommand : ContextSpecification<PathAndValueEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>>
   {
      protected ExpressionProfileBuildingBlock _buildingBlock;
      protected double? _newValue;
      protected ExpressionParameter _expressionParameter;
      protected double? _oldValue;
      protected IMoBiContext _context;
      protected IFormula _oldFormula;

      protected override void Context()
      {
         _newValue = 3.0;
         _oldValue = 2.0;
         _oldFormula = new ExplicitFormula {Id = "formulaId"};
         _expressionParameter = new ExpressionParameter { Value = _oldValue, Formula = _oldFormula, DisplayUnit = new Unit("unit", 1, 0)};
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new ExpressionProfileBuildingBlock
         {
            _expressionParameter,
         };
         _buildingBlock.Id = "id";

         sut = new PathAndValueEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>(_expressionParameter, _newValue, _expressionParameter.DisplayUnit, _buildingBlock);
      }
   }

   public class When_the_command_has_run : concern_for_ExpressionValueChangedCommand
   {
      protected override void Because()
      {
         sut.RunCommand(_context);
      }

      [Observation]
      public void the_expression_parameter_value_should_be_updated()
      {
         _expressionParameter.Value.ShouldBeEqualTo(_newValue);
      }

      [Observation]
      public void the_initial_state_should_be_set()
      {
         _expressionParameter.InitialValue.ShouldBeEqualTo(_oldValue);
         _expressionParameter.InitialFormulaId.ShouldBeEqualTo(_oldFormula.Id);
         _expressionParameter.InitialUnit.ShouldNotBeNull();
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
         sut.RunCommand(_context);
         sut.RestoreExecutionData(_context);
         sut.InverseCommand(_context).RunCommand(_context);
      }

      [Observation]
      public void the_expression_parameter_value_should_be_updated()
      {
         _expressionParameter.Value.ShouldBeEqualTo(_oldValue);
      }

      [Observation]
      public void the_initial_state_should_be_reset()
      {
         _expressionParameter.InitialValue.ShouldBeNull();
         _expressionParameter.InitialFormulaId.ShouldBeNull();
         _expressionParameter.InitialUnit.ShouldBeNull();
         _expressionParameter.HasInitialState.ShouldBeFalse();
      }
   }
}