using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_ExpressionParameterValueOrUnitChangedCommand : ContextSpecification<ExpressionParameterValueOrUnitChangedCommand>
   {
      protected ExpressionProfileBuildingBlock _buildingBlock;
      protected ExpressionParameter _expressionParameter;
      protected Unit _newUnit;
      protected Dimension _dimension;
      protected double? _value;
      protected Unit _unit;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _expressionParameter = new ExpressionParameter();
         _dimension = new Dimension(new BaseDimensionRepresentation(), "dimensionName", "baseUnit");
         _newUnit = new Unit("newUnit", 10, 0);
         _dimension.AddUnit(_newUnit);

         _buildingBlock = new ExpressionProfileBuildingBlock()
         {
            _expressionParameter
         };
         _buildingBlock.Id = "8";

         _value = GetNewValueForContext();
         _unit = GetNewUnitForContext();

         sut = new ExpressionParameterValueOrUnitChangedCommand(_expressionParameter, _value, _unit, _buildingBlock);
      }

      protected abstract double? GetNewValueForContext();

      protected abstract Unit GetNewUnitForContext();
   }

   public class When_reversing_the_unit_of_the_command : concern_for_ExpressionParameterValueOrUnitChangedCommand
   {
      private double? _oldValue;
      private Unit _oldUnit;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<ExpressionProfileBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         _oldValue = _expressionParameter.Value;
         _oldUnit = _expressionParameter.DisplayUnit;
         sut.Execute(_context);
         sut.RestoreExecutionData(_context);
      }

      protected override void Because()
      {
         sut.InverseCommand(_context).Execute(_context);
      }

      protected override double? GetNewValueForContext()
      {
         return _expressionParameter.Value;
      }

      protected override Unit GetNewUnitForContext()
      {
         return _newUnit;
      }

      [Observation]
      public void The_value_should_be_restored()
      {
         _expressionParameter.DisplayUnit.ShouldBeEqualTo(_oldUnit);
         _expressionParameter.Value.ShouldBeEqualTo(_oldValue);
      }
   }

   public class When_reversing_the_value_of_the_command : concern_for_ExpressionParameterValueOrUnitChangedCommand
   {
      private double? _oldValue;
      private Unit _oldUnit;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<ExpressionProfileBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         _oldValue = _expressionParameter.Value;
         _oldUnit = _expressionParameter.DisplayUnit;
         sut.Execute(_context);
         sut.RestoreExecutionData(_context);
      }

      protected override void Because()
      {
         sut.InverseCommand(_context).Execute(_context);
      }

      protected override double? GetNewValueForContext()
      {
         return 12;
      }

      protected override Unit GetNewUnitForContext()
      {
         return _dimension.BaseUnit;
      }

      [Observation]
      public void The_value_should_be_restored()
      {
         _expressionParameter.DisplayUnit.ShouldBeEqualTo(_oldUnit);
         _expressionParameter.Value.ShouldBeEqualTo(_oldValue);
      }
   }

   public class When_updating_the_unit_of_the_expression_parameter : concern_for_ExpressionParameterValueOrUnitChangedCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      protected override double? GetNewValueForContext()
      {
         return _expressionParameter.Value;
      }

      protected override Unit GetNewUnitForContext()
      {
         return _newUnit;
      }

      [Observation]
      public void The_unit_should_be_updated_in_the_parameter()
      {
         _expressionParameter.DisplayUnit.ShouldBeEqualTo(GetNewUnitForContext());
      }
   }

   public class When_updating_the_value_of_the_expression_parameter : concern_for_ExpressionParameterValueOrUnitChangedCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      protected override double? GetNewValueForContext()
      {
         return 12;
      }

      protected override Unit GetNewUnitForContext()
      {
         return _dimension.BaseUnit;
      }

      [Observation]
      public void the_value_should_be_updated_in_the_parameter()
      {
         _expressionParameter.Value.ShouldBeEqualTo(GetNewValueForContext());
      }
   }
}
