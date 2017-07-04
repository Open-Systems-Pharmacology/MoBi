using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core
{
   public abstract class concern_for_ValidationSettings : ContextSpecification<ValidationSettings>
   {
      protected override void Context()
      {
         sut = new ValidationSettings();
      }
   }

   public class When_setting_the_property_check_dimension_to_false_in_the_validation_settings : concern_for_ValidationSettings
   {
      protected override void Context()
      {
         base.Context();
         sut.CheckDimensions = true;
         sut.ShowPKSimDimensionProblemWarnings = true;
         sut.ShowCannotCalcErrors = true;
      }

      protected override void Because()
      {
         sut.CheckDimensions = false;
      }

      [Observation]
      public void should_also_set_the_properties_show_cannot_calculate_error_and_validate_pksim_dimensions_to_false()
      {
         sut.ShowPKSimDimensionProblemWarnings.ShouldBeFalse();
         sut.ShowCannotCalcErrors.ShouldBeFalse();
      }
   }
}	