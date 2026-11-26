using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SwapFormulaUsablePathCommand : ContextSpecification<SwapFormulaUsablePathCommand>
   {
      protected ExplicitFormula _formula;
      protected FormulaUsablePath _oldPath;
      protected FormulaUsablePath _newPath;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _formula = new ExplicitFormula("t").WithId("id").WithOriginId("Origin");
         _oldPath = new FormulaUsablePath("Organism", "P").WithAlias("t");
         _newPath = new TimePath().WithAlias("t");
         _formula.AddObjectPath(_oldPath);
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IFormula>(_formula.Id)).Returns(_formula);

         sut = new SwapFormulaUsablePathCommand(_formula, _oldPath, _newPath, A.Fake<IBuildingBlock>());
      }
   }

   public class When_swapping_a_formula_usable_path_in_a_formula_with_an_origin : concern_for_SwapFormulaUsablePathCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_replace_the_path_used_by_the_alias()
      {
         _formula.ObjectPaths.ShouldOnlyContain(_newPath);
         _formula.FormulaUsablePathBy("t").ShouldBeAnInstanceOf<TimePath>();
      }

      [Observation]
      public void the_formula_should_lose_its_origin()
      {
         _formula.OriginId.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_reverting_the_swap_of_a_formula_usable_path : concern_for_SwapFormulaUsablePathCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_restore_the_original_path()
      {
         _formula.ObjectPaths.ShouldOnlyContain(_oldPath);
      }
   }
}
