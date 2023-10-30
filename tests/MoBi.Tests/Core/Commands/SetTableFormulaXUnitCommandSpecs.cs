using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetTableFormulaXUnitCommand : ContextSpecification<SetTableFormulaXDisplayUnitCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected Unit _newUnit;
      protected Unit _oldUnit;
      protected TableFormula _tableFormula;
      protected IMoBiContext _context;
      protected double _xValueInMinutes;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _newUnit = DomainHelperForSpecs.TimeDimension.Unit("seconds");
         _oldUnit = DomainHelperForSpecs.TimeDimension.Unit("min");
         _tableFormula = new TableFormula { XDimension = DomainHelperForSpecs.TimeDimension };
         _xValueInMinutes = 1.0;
         _tableFormula.AddPoint(new ValuePoint(_xValueInMinutes, 1.0));
         _context = A.Fake<IMoBiContext>();

         A.CallTo(() => _context.Get<TableFormula>(_tableFormula.Id)).Returns(_tableFormula);

         _tableFormula.XDisplayUnit = _oldUnit;
         sut = new SetTableFormulaXDisplayUnitCommand(_newUnit, _oldUnit, _tableFormula, _buildingBlock);
      }
   }

   public class When_reverting_the_unit_for_a_table_formula : concern_for_SetTableFormulaXUnitCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_table_formula_unit_should_be_reverted_to_original()
      {
         _tableFormula.XDisplayUnit.ShouldBeEqualTo(_oldUnit);
      }

      [Observation]
      public void the_x_display_value_should_be_converted_back()
      {
         _tableFormula.AllPoints.First().X.ShouldBeEqualTo(_xValueInMinutes);
      }
   }

   public class When_updating_the_unit_for_a_table_formula : concern_for_SetTableFormulaXUnitCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_table_formula_unit_should_be_updated()
      {
         _tableFormula.XDisplayUnit.ShouldBeEqualTo(_newUnit);
      }

      [Observation]
      public void the_value_point_should_be_converted()
      {
         _tableFormula.AllPoints.First().X.ShouldBeEqualTo(_xValueInMinutes / 60);
      }
   }
}
