using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_FormulaToValueFormulaDTOMapper : ContextSpecification<FormulaToValueFormulaDTOMapper>
   {
      protected override void Context()
      {
         sut = new FormulaToValueFormulaDTOMapper();
      }
   }

   public class When_mapping_a_explicit_formula : concern_for_FormulaToValueFormulaDTOMapper
   {
      private ValueFormulaDTO _result;
      private IFormula _formula;
      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("the formula");
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_formula);
      }

      [Observation]
      public void the_result_should_be_a_dto_with_appropriate_properties()
      {
         _result.Formula.ShouldBeEqualTo(_formula);
      }
   }

   public class When_mapping_a_non_explicit_formula : concern_for_FormulaToValueFormulaDTOMapper
   {
      private ValueFormulaDTO _result;
      private IFormula _formula;
      protected override void Context()
      {
         base.Context();
         _formula = new TableFormula();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_formula);
      }

      [Observation]
      public void the_result_should_be_a_dto_with_appropriate_properties()
      {
         _result.ShouldBeAnInstanceOf<EmptyFormulaDTO>();
      }
   }
}
