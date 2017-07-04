using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation
{
   public abstract class concern_for_FormulaUsablePathDTO : ContextSpecification<FormulaUsablePathDTO>
   {
      protected IFormula _formula;
      protected IFormulaUsablePath _formulaUsablePath;

      protected override void Context()
      {
         _formulaUsablePath = new FormulaUsablePath("path") {Alias = "alias"};
         _formula = new ExplicitFormula();
         _formula.AddObjectPath(_formulaUsablePath);
         sut = new FormulaUsablePathDTO(_formulaUsablePath, _formula);
      }
   }

   public class should_pass_validation_When_rules_are_not_broken : concern_for_FormulaUsablePathDTO
   {
      protected override void Context()
      {
         base.Context();
         sut = new FormulaUsablePathDTO(new FormulaUsablePath("A|B|C") {Alias = "alias"}, _formula);
      }

      [Observation]
      public void passes_validation()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class should_pass_validation_When_the_formula_usable_path_does_not_belong_in_the_formula : concern_for_FormulaUsablePathDTO
   {
      [Observation]
      public void passes_validation()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_a_dto_where_the_path_is_empty : concern_for_FormulaUsablePathDTO
   {
      protected override void Context()
      {
         base.Context();
         var invalidPath = new FormulaUsablePath("") {Alias = "invalaid"};
         _formula.AddObjectPath(invalidPath);
         sut = new FormulaUsablePathDTO(invalidPath, _formula);
      }

      [Observation]
      public void fails_validation()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_renaming_and_validating_a_dto_where_the_new_name_is_null_or_empty : concern_for_FormulaUsablePathDTO
   {
      protected override void Context()
      {
         base.Context();
         _formulaUsablePath.Alias = null;
      }

      protected override void Because()
      {
         sut.Validate();
      }

      [Observation]
      public void should_fail_validation()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_renaming_a_validating_a_dto_where_an_another_formula_usable_path_has_the_same_name : concern_for_FormulaUsablePathDTO
   {
      protected override void Context()
      {
         base.Context();
         _formula.AddObjectPath(new FormulaUsablePath {Alias = "alias"});
      }

      protected override void Because()
      {
         sut.Validate();
      }

      [Observation]
      public void should_fail_validation()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}