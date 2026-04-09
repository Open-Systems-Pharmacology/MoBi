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

   public class When_cloning_validation_settings : concern_for_ValidationSettings
   {
      private ValidationSettings _clone;

      protected override void Context()
      {
         base.Context();
         sut.CheckDimensions = true;
         sut.ShowPKSimDimensionProblemWarnings = true;
         sut.ShowCannotCalcErrors = true;
         sut.ShowPKSimObserverMessages = true;
         sut.CheckRules = true;
         sut.CheckCircularReference = true;
         sut.ShowUnresolvedEndosomesWarningsForInitialConditions = true;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_create_a_clone_with_all_properties_matching_the_original()
      {
         _clone.CheckDimensions.ShouldBeEqualTo(sut.CheckDimensions);
         _clone.ShowPKSimDimensionProblemWarnings.ShouldBeEqualTo(sut.ShowPKSimDimensionProblemWarnings);
         _clone.ShowCannotCalcErrors.ShouldBeEqualTo(sut.ShowCannotCalcErrors);
         _clone.ShowPKSimObserverMessages.ShouldBeEqualTo(sut.ShowPKSimObserverMessages);
         _clone.CheckRules.ShouldBeEqualTo(sut.CheckRules);
         _clone.CheckCircularReference.ShouldBeEqualTo(sut.CheckCircularReference);
         _clone.ShowUnresolvedEndosomesWarningsForInitialConditions.ShouldBeEqualTo(sut.ShowUnresolvedEndosomesWarningsForInitialConditions);
      }

      [Observation]
      public void should_not_return_the_same_instance()
      {
         _clone.ShouldNotBeEqualTo(sut);
      }
   }

   public class When_updating_validation_settings_from_another_instance : concern_for_ValidationSettings
   {
      private ValidationSettings _source;

      protected override void Context()
      {
         base.Context();
         _source = new ValidationSettings
         {
            CheckDimensions = true,
            ShowPKSimDimensionProblemWarnings = true,
            ShowCannotCalcErrors = true,
            ShowPKSimObserverMessages = true,
            CheckRules = true,
            CheckCircularReference = true,
            ShowUnresolvedEndosomesWarningsForInitialConditions = true
         };
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_source);
      }

      [Observation]
      public void should_update_all_properties_from_source()
      {
         sut.CheckDimensions.ShouldBeEqualTo(_source.CheckDimensions);
         sut.ShowPKSimDimensionProblemWarnings.ShouldBeEqualTo(_source.ShowPKSimDimensionProblemWarnings);
         sut.ShowCannotCalcErrors.ShouldBeEqualTo(_source.ShowCannotCalcErrors);
         sut.ShowPKSimObserverMessages.ShouldBeEqualTo(_source.ShowPKSimObserverMessages);
         sut.CheckRules.ShouldBeEqualTo(_source.CheckRules);
         sut.CheckCircularReference.ShouldBeEqualTo(_source.CheckCircularReference);
         sut.ShowUnresolvedEndosomesWarningsForInitialConditions.ShouldBeEqualTo(_source.ShowUnresolvedEndosomesWarningsForInitialConditions);
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