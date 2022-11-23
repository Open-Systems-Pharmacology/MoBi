using FakeItEasy;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_StartValueFormatter : ContextSpecification<NotAvailableValueFormatter>
   {
      protected IStartValueDTO _startValueDTO;

      protected override void Context()
      {
         _startValueDTO = A.Fake<IStartValueDTO>();
         sut = new NotAvailableValueFormatter(_startValueDTO);
      }
   }

   public class When_formatting_a_start_value : concern_for_StartValueFormatter
   {
      [Observation]
      public void should_return_start_value_not_available_if_the_value_is_null()
      {
         sut.Format(null).ShouldBeEqualTo(AppConstants.Captions.ValueNotAvailable);
      }

      [Observation]
      public void should_return_start_value_not_available_if_the_value_is_nan()
      {
         sut.Format(double.NaN).ShouldBeEqualTo(AppConstants.Captions.ValueNotAvailable);
      }

      [Observation]
      public void should_return_the_formatted_value_otherwise()
      {
         sut.Format(5).ShouldBeEqualTo(new NullableWithDisplayUnitFormatter(_startValueDTO).Format(5));
      }
   }
}