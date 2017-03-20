using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditFormulaStringCommand : ContextSpecification<EditFormulaStringCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected string _newFormulaString;
      protected string _oldFormulaString;
      protected ExplicitFormula _formula;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _newFormulaString = "newFormulaString";
         _oldFormulaString = "oldFormulaString";
         _formula = new ExplicitFormula(_oldFormulaString);
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<ExplicitFormula>(_formula.Id)).Returns(_formula);

         sut = new EditFormulaStringCommand(_newFormulaString, _oldFormulaString, _formula, _buildingBlock);
      }
   }

   public class When_updating_the_formula_string_in_a_formula : concern_for_EditFormulaStringCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_formula_string_should_be_updated()
      {
         _formula.FormulaString.ShouldBeEqualTo(_newFormulaString);
      }
   }

   public class When_reverting_the_formula_string_in_a_formula : concern_for_EditFormulaStringCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_formula_string_should_be_reverted()
      {
         _formula.FormulaString.ShouldBeEqualTo(_oldFormulaString);
      }
   }
}
