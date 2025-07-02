using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetTableFormulaYUnitCommand : ContextSpecification<SetTableFormulaYDisplayUnitCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected Unit _newUnit;
      protected Unit _oldUnit;
      protected TableFormula _tableFormula;
      protected IMoBiContext _context;
      protected double _yvalueInMilliMol;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _newUnit = DomainHelperForSpecs.AmountDimension.Unit("µmol");
         _oldUnit = DomainHelperForSpecs.AmountDimension.Unit("mmol");
         _tableFormula = new TableFormula { Dimension = DomainHelperForSpecs.AmountDimension };
         _yvalueInMilliMol = 1.0;
         _tableFormula.AddPoint(new ValuePoint(1.0, _yvalueInMilliMol));
         _context = A.Fake<IMoBiContext>();

         A.CallTo(() => _context.Get<TableFormula>(_tableFormula.Id)).Returns(_tableFormula);

         _tableFormula.YDisplayUnit = _oldUnit;
         sut = new SetTableFormulaYDisplayUnitCommand(_newUnit, _oldUnit, _tableFormula, _buildingBlock);
      }
   }

   public class When_reverting_the_y_unit_for_a_table_formula : concern_for_SetTableFormulaYUnitCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_table_formula_unit_should_be_reverted_to_original()
      {
         _tableFormula.YDisplayUnit.ShouldBeEqualTo(_oldUnit);
      }

      [Observation]
      public void the_y_display_value_should_be_converted_back()
      {
         _tableFormula.AllPoints.First().Y.ShouldBeEqualTo(_yvalueInMilliMol);
      }
   }

   public class When_updating_the_y_unit_for_a_table_formula : concern_for_SetTableFormulaYUnitCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_table_formula_unit_should_be_updated()
      {
         _tableFormula.YDisplayUnit.ShouldBeEqualTo(_newUnit);
      }

      [Observation]
      public void the_value_point_should_be_converted()
      {
         _tableFormula.AllPoints.First().Y.ShouldBeEqualTo(_yvalueInMilliMol / 1000);
      }
   }
}