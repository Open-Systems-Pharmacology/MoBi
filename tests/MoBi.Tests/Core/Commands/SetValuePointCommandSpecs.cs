using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetValuePointCommand : ContextSpecification<SetValuePointCommand>
   {
      protected TableFormula _tableFormula;
      protected ValuePoint _valuePoint;
      protected IBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected double _newValue;
      protected string _tableFormulaId;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _newValue = 10;
         _tableFormulaId = "FormulaId";
         _valuePoint = new ValuePoint(1, 11);
         _tableFormula = new TableFormula { Id = _tableFormulaId, Dimension = DomainHelperForSpecs.AmountDimension, XDimension = DomainHelperForSpecs.TimeDimension };
         _tableFormula.AddPoint(_valuePoint);
         _tableFormula.AddPoint(2, 22);

         _buildingBlock = A.Fake<IBuildingBlock>();
      }
   }

   public class When_executing_the_set_x_value_point_command : concern_for_SetValuePointCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new SetValuePointXValueCommand(_tableFormula, _valuePoint, _newValue, _buildingBlock);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_value_in_the_point_according_to_the_value_given_as_parameter()
      {
         _valuePoint.X.ShouldBeEqualTo(_newValue);
      }
   }

   public class When_inversing_the_set_y_value_point_command : concern_for_SetValuePointCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new SetValuePointYValueCommand(_tableFormula, _valuePoint, _newValue, _buildingBlock);
         A.CallTo(_context).WithReturnType<TableFormula>().Returns(_tableFormula);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_reset_the_value_to_its_original_value()
      {
         _valuePoint.Y.ShouldBeEqualTo(11);
      }
   }

   public class When_executing_the_set_y_value_point_command : concern_for_SetValuePointCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new SetValuePointYValueCommand(_tableFormula, _valuePoint, _newValue, _buildingBlock);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_value_in_the_point_according_to_the_value_given_as_parameter()
      {
         _valuePoint.Y.ShouldBeEqualTo(_newValue);
      }
   }

   public class When_inversing_the_set_x_value_point_command : concern_for_SetValuePointCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new SetValuePointXValueCommand(_tableFormula, _valuePoint, _newValue, _buildingBlock);
         A.CallTo(_context).WithReturnType<TableFormula>().Returns(_tableFormula);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_reset_the_value_to_its_original_value()
      {
         _valuePoint.X.ShouldBeEqualTo(1);
      }
   }
}