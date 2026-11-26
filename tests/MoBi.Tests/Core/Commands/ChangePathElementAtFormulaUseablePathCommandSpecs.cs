using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangePathElementAtFormulaUseablePathCommand : ContextSpecification<ChangePathElementAtFormulaUseablePathCommand>
   {
      protected ExplicitFormula _formula;
      protected FormulaUsablePath _path;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _formula = new ExplicitFormula("A").WithOriginId("Origin");
         _path = new FormulaUsablePath("OLD", "PATH").WithAlias("A");
         _formula.AddObjectPath(_path);
         _context = A.Fake<IMoBiContext>();

         sut = new ChangePathElementAtFormulaUseablePathCommand("NEW", _formula, "OLD", _path, A.Fake<IBuildingBlock>());
      }
   }

   public class When_changing_a_path_element_in_a_formula_with_an_origin : concern_for_ChangePathElementAtFormulaUseablePathCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_replace_the_path_element()
      {
         _path.ShouldOnlyContainInOrder("NEW", "PATH");
      }

      [Observation]
      public void the_formula_should_lose_its_origin()
      {
         _formula.OriginId.ShouldBeEqualTo(string.Empty);
      }
   }
}
