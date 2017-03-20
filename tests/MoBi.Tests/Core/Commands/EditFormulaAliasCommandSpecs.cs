using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditFormulaAliasCommand : ContextSpecification<EditFormulaAliasCommand>
   {
      protected IBuildingBlock _buildingBlock;
      protected ExplicitFormula _formula;
      protected string _newAlias;
      protected string _oldAlias;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _formula = new ExplicitFormula();
         _newAlias = "newAlias";
         _oldAlias = "oldAlias";
         _formula.AddObjectPath(new FormulaUsablePath { Alias = _oldAlias });
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IFormula>(_formula.Id)).Returns(_formula);
         sut = new EditFormulaAliasCommand(_formula, _newAlias, _oldAlias, _buildingBlock);
      }
   }

   public class When_reverting_the_alias_of_a_formula_usable_path : concern_for_EditFormulaAliasCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_alias_should_be_reverted()
      {
         _formula.FormulaUsablePathBy("oldAlias").ShouldNotBeNull();
         _formula.FormulaUsablePathBy("newAlias").ShouldBeNull();
      }
   }

   public class When_changing_the_alias_of_a_formula_usable_path : concern_for_EditFormulaAliasCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_alias_should_be_changed()
      {
         _formula.FormulaUsablePathBy("oldAlias").ShouldBeNull();
         _formula.FormulaUsablePathBy("newAlias").ShouldNotBeNull();
      }
   }


}
