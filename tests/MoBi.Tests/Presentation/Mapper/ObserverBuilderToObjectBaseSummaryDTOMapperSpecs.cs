using System.Linq;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_ObserverBuilderToObjectBaseSummaryDTOMapper : ContextSpecification<ObserverBuilderToObjectBaseSummaryDTOMapper>
   {
      protected ObserverBuilder _builder;

      protected override void Context()
      {
         _builder = new ObserverBuilder();
         sut = new ObserverBuilderToObjectBaseSummaryDTOMapper(new ObjectTypeResolver());
      }
   }

   public class when_mapping_observer_builder_to_dto : concern_for_ObserverBuilderToObjectBaseSummaryDTOMapper
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         _builder.Name = "Name";
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_builder);
      }

      [Observation]
      public void mapped_fields_in_dto_should_be_correct()
      {
         _result.EntityName.ShouldBeEqualTo("Name");
         _result.ApplicationIcon.ShouldBeEqualTo(ApplicationIcons.Observer);
      }

      [Observation]
      public void mapped_dictionary_should_have_correct_data()
      {
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Type) && x.Value.Equals("Observer")).ShouldBeEqualTo(1);
      }
   }
}
