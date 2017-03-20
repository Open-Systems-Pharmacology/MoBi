using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Service
{
   public abstract class concern_for_AmoutToConcentrationFormulaMapping : ContextSpecification<IAmoutToConcentrationFormulaMapper>
   {
      protected override void Context()
      {
         sut = new AmoutToConcentrationFormulaMapper();
      }
   }

   public class When_the_formula_mapping_is_asked_if_a_mapping_is_available_for_a_formula  : concern_for_AmoutToConcentrationFormulaMapping
   {
      [Observation]
      public void should_return_true_if_a_mapping_was_defined()
      {
         sut.HasMappingFor(new ExplicitFormula("f_cell * V * CLspec * C_cell * K_water_cell")).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.HasMappingFor(new ExplicitFormula("A+B")).ShouldBeFalse();
      }
   }

   public class When_returning_the_mapped_formula_for_a_given_formula : concern_for_AmoutToConcentrationFormulaMapping
   {
      [Observation]
      public void should_return_the_expected_formula_if_defined()
      {
         sut.MappedFormulaFor(new ExplicitFormula("f_cell * V * CLspec * C_cell * K_water_cell")).ShouldBeEqualTo("CLspec * C_cell * K_water_cell");
      }

      [Observation]
      public void should_throw_an_exception_otherwise()
      {
         The.Action(()=>sut.MappedFormulaFor(new ExplicitFormula("A+B"))).ShouldThrowAn<Exception>();
      }
   }
}	