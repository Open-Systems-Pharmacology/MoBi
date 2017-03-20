using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_EventGroupBuilderToObjectBaseSummaryDTOMapper : ContextSpecification<EventGroupBuilderToObjectBaseSummaryDTOMapper>
   {
      protected IEventGroupBuilder _builder;
      protected override void Context()
      {
         _builder = A.Fake<IEventGroupBuilder>();
         sut = new EventGroupBuilderToObjectBaseSummaryDTOMapper(new ObjectTypeResolver());
      }
   }

   public class when_mapping_event_builder : concern_for_EventGroupBuilderToObjectBaseSummaryDTOMapper
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _builder.Name).Returns("Name");
         A.CallTo(() => _builder.Children).Returns(new List<IEntity>{A.Fake<IEntity>(), A.Fake<IEntity>()});
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_builder);
      }

      [Observation]
      public void dto_should_have_correct_field_values()
      {
         _result.EntityName.ShouldBeEqualTo("Name");
         _result.ApplicationIcon.ShouldBeEqualTo(ApplicationIcons.EventGroup);
      }

      [Observation]
      public void dictionary_should_have_correct_field_values()
      {
         _result.Dictionary.FirstOrDefault(x => x.Key.Equals(AppConstants.Captions.Type) && x.Value.Equals("Event Group")).ShouldNotBeNull();
         _result.Dictionary.FirstOrDefault(x => x.Key.Equals(AppConstants.Captions.NumberOfChildren) && x.Value.Equals(2.ToString(CultureInfo.InvariantCulture))).ShouldNotBeNull();
      }
   }
}
