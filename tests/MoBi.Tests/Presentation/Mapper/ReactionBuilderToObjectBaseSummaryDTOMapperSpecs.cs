using System.Linq;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_ReactionBuilderToObjectBaseSummaryDTOMapper : ContextSpecification<ReactionBuilderToObjectBaseSummaryDTOMapper>
   {
      protected ReactionBuilder _reactionBuilder;

      protected override void Context()
      {
         _reactionBuilder = new ReactionBuilder();
         sut = new ReactionBuilderToObjectBaseSummaryDTOMapper(new StoichiometricStringCreator(new ReactionPartnerToReactionPartnerBuilderMapper()), new ObjectTypeResolver());
      }
   }

   public class When_mapping_reaction_builder_to_dto : concern_for_ReactionBuilderToObjectBaseSummaryDTOMapper
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         _reactionBuilder.Name = "Name";
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_reactionBuilder);
      }

      [Observation]
      public void dto_should_have_correctly_initialized_fields()
      {
         _result.EntityName.ShouldBeEqualTo("Name");
         _result.ApplicationIcon.ShouldBeEqualTo(ApplicationIcons.Reaction);
      }

      [Observation]
      public void dictionary_should_have_correct_values()
      {
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Type) && x.Value.Equals("Reaction")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.NumberOfParameters) && x.Value.Equals("0")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Stoichiometry)).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Kinetic)).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Type)).ShouldBeEqualTo(1);
      }

   }
}
