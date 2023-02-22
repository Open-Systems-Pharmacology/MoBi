using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public class concern_for_RenameExpressionProfileDTO : ContextSpecification<RenameExpressionProfileDTO>
   {
      private ExpressionProfileBuildingBlock _expressionProfile;
      protected string _originalBuildingBlockName;

      protected override void Context()
      {
         _originalBuildingBlockName = "Molecule|Species|Category";
         _expressionProfile = new ExpressionProfileBuildingBlock
         {
            Name = _originalBuildingBlockName,
            Type = ExpressionTypes.MetabolizingEnzyme
         };

         sut = new RenameExpressionProfileDTO
         {
            Species = _expressionProfile.Species,
            MoleculeName = _expressionProfile.MoleculeName,
            Category = _expressionProfile.Category,
            Type = _expressionProfile.Type.DisplayName
         };
      }
   }

   public class When_creating_a_rename_expression_profile_dto_without_forbidden_names : concern_for_RenameExpressionProfileDTO
   {
      [Observation]
      public void the_dto_is_not_valid_if_the_name_is_in_the_forbidden_list()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_adding_the_building_block_name_to_the_forbidden_names_list : concern_for_RenameExpressionProfileDTO
   {
      protected override void Because()
      {
         sut.AddForbiddenNames(new []{ _originalBuildingBlockName });
      }

      [Observation]
      public void the_dto_is_not_valid_if_the_name_is_in_the_forbidden_list()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_the_species_component_is_empty : concern_for_RenameExpressionProfileDTO
   {
      protected override void Because()
      {
         sut.Species = string.Empty;
      }

      [Observation]
      public void the_dto_is_not_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_the_category_component_is_empty : concern_for_RenameExpressionProfileDTO
   {
      protected override void Because()
      {
         sut.Category = string.Empty;
      }

      [Observation]
      public void the_dto_is_not_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_the_molecule_name_component_is_empty : concern_for_RenameExpressionProfileDTO
   {
      protected override void Because()
      {
         sut.MoleculeName = string.Empty;
      }

      [Observation]
      public void the_dto_is_not_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}
