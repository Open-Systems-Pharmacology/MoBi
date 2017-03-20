using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddValuePointCommand : ContextSpecification<AddValuePointCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected TableFormula _tableFormula;
      protected ValuePoint _valuePoint;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _tableFormula = new TableFormula();
         _tableFormula.Dimension = A.Fake<IDimension>();
         _tableFormula.XDimension = A.Fake<IDimension>();
         _valuePoint = new ValuePoint(1, 2);
         _buildingBlock = A.Fake<IBuildingBlock>();

         sut = new AddValuePointCommand(_tableFormula, _valuePoint, _buildingBlock);
      }
   }

   public class When_adding_a_point_to_a_table_formula : concern_for_AddValuePointCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_tableFormula.XDimension).WithReturnType<double>().Returns(10);
         A.CallTo(_tableFormula.Dimension).WithReturnType<double>().Returns(20);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_add_the_point_to_the_formula()
      {
         _tableFormula.AllPoints().ShouldContain(_valuePoint);
      }

      [Observation]
      public void the_description_should_contain_the_values_in_their_display_unit()
      {
         sut.Description.Contains("20").ShouldBeTrue();
         sut.Description.Contains("10").ShouldBeTrue();
      }
   }

   public class When_executing_the_inverse_of_the_add_point_to_formula_command : concern_for_AddValuePointCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_context).WithReturnType<ValuePoint>().Returns(_valuePoint);
         A.CallTo(_context).WithReturnType<TableFormula>().Returns(_tableFormula);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_remove_the_point_that_was_added()
      {
         _tableFormula.AllPoints().ShouldBeEmpty();
      }
   }
}