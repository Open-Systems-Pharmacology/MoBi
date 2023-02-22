using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_ExpressionProfileToRenameExpressionProfileDTOMapper : ContextSpecification<RenameExpressionProfileDTOCreator>
   {
      protected override void Context()
      {
         sut = new RenameExpressionProfileDTOCreator();
      }
   }

   public class When_mapping_an_expression_profile_to_a_rename_expression_profile_dto : concern_for_ExpressionProfileToRenameExpressionProfileDTOMapper
   {
      private ExpressionProfileBuildingBlock _expressionProfile;
      private RenameExpressionProfileDTO _result;

      protected override void Context()
      {
         base.Context();
         _expressionProfile = new ExpressionProfileBuildingBlock
         {
            Name = "Molecule|Species|Category",
            Type = ExpressionTypes.MetabolizingEnzyme
         };
      }

      protected override void Because()
      {
         _result = sut.Create(_expressionProfile.MoleculeName, _expressionProfile.Species, _expressionProfile.Category, _expressionProfile.Type);
      }

      [Observation]
      public void should_return_a_rename_expression_profile_dto()
      {
         _result.Name.ShouldBeEqualTo(_expressionProfile.Name);
      }

      [Observation]
      public void the_species_should_be_mapped()
      {
         _result.Species.ShouldBeEqualTo(_expressionProfile.Species);
      }

      [Observation]
      public void the_molecule_should_be_mapped()
      {
         _result.MoleculeName.ShouldBeEqualTo(_expressionProfile.MoleculeName);
      }

      [Observation]
      public void the_category_should_be_mapped()
      {
         _result.Category.ShouldBeEqualTo(_expressionProfile.Category);
      }

      [Observation]
      public void the_expression_type_should_be_mapped()
      {
         _result.Type.ShouldBeEqualTo(_expressionProfile.Type.DisplayName);
      }
   }
}
