using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddFormulaUsablePathCommandSpecs : ContextSpecification<AddFormulaUsablePathCommand>
   {
      protected TimePath _timePath;
      protected IFormula _formula;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _formula = new ExplicitFormula();
         _timePath = new TimePath();
         _context = A.Fake<IMoBiContext>();

         sut = new AddFormulaUsablePathCommand(_formula, _timePath, A.Fake<IBuildingBlock>());
      }
   }

   internal class When_invoking_the_inverse_of_the_add_formula_usable_path_command_for_a_time_path : concern_for_AddFormulaUsablePathCommandSpecs
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<IFormula>(A<string>._)).Returns(_formula);

      }
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_remove_the_time_path()
      {
         _formula.FormulaUsablePathBy(Constants.TIME).ShouldBeNull();
      }
   }
}