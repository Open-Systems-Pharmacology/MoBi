using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveFormulaUsablePathCommand : ContextSpecification<RemoveFormulaUsablePathCommand>
   {
      protected ExplicitFormula _formula;
      protected FormulaUsablePath _path;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _formula = new ExplicitFormula("A").WithOriginId("Origin");
         _path = new FormulaUsablePath("A", "PATH").WithAlias("A");
         _formula.AddObjectPath(_path);
         _context = A.Fake<IMoBiContext>();

         sut = new RemoveFormulaUsablePathCommand(_formula, _path, A.Fake<IBuildingBlock>());
      }
   }

   public class When_removing_a_formula_usable_path_from_a_formula_with_an_origin : concern_for_RemoveFormulaUsablePathCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_the_path()
      {
         _formula.FormulaUsablePathBy("A").ShouldBeNull();
      }

      [Observation]
      public void the_formula_should_lose_its_origin()
      {
         _formula.OriginId.ShouldBeEqualTo(string.Empty);
      }
   }
}
