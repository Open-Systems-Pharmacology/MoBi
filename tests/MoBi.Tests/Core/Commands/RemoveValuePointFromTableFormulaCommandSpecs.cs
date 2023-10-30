using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveValuePointFromTableFormulaCommand : ContextSpecification<RemoveValuePointFromTableFormulaCommand>
   {
      protected TableFormula _tableFormula;
      protected ValuePoint _valuePoint;
      protected IBuildingBlock _buildingBlock;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _tableFormula = new TableFormula();
         _valuePoint=new ValuePoint(1,2);
         _tableFormula.AddPoint(_valuePoint);
         _tableFormula.Dimension = A.Fake<IDimension>();
         _tableFormula.XDimension = A.Fake<IDimension>();
         _buildingBlock = A.Fake<IBuildingBlock>();

         sut = new RemoveValuePointFromTableFormulaCommand(_tableFormula, _valuePoint, _buildingBlock);
      }
   }

   public class When_executing_the_remove_point_from_table_formula_command : concern_for_RemoveValuePointFromTableFormulaCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_context).WithReturnType<string>().Returns("XML");

      }
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_the_point_from_the_command()
      {
         _tableFormula.AllPoints.Any().ShouldBeFalse();
      }
   }

   public class When_inversing_the_remove_point_from_table_formula_command : concern_for_RemoveValuePointFromTableFormulaCommand
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
      public void should_add_the_point_back_to_the_formula()
      {
         _tableFormula.ValueAt(1).ShouldBeEqualTo(2);
      }
   }
}	