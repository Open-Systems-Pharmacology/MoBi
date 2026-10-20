using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core
{
   public abstract class concern_for_SimulationValidationOptions : ContextSpecification<SimulationValidationOptions>
   {
      protected ICoreUserSettings _userSettings;

      protected override void Context()
      {
         _userSettings = A.Fake<ICoreUserSettings>();
         _userSettings.CheckDimensions = true;
         _userSettings.ShowPKSimDimensionProblemWarnings = true;
         _userSettings.ShowCannotCalcErrors = true;
         _userSettings.ShowPKSimObserverMessages = true;
         _userSettings.CheckRules = true;
         _userSettings.CheckCircularReference = true;
         _userSettings.ShowUnresolvedEndosomeMessagesForInitialConditions = true;
         _userSettings.WarnForNonFiniteQuantities = true;
         sut = new SimulationValidationOptions();
      }
   }

   public class When_resolving_validation_options_that_are_not_set : concern_for_SimulationValidationOptions
   {
      private ValidationSettings _result;

      protected override void Because()
      {
         _result = sut.ValidationSettingsWith(_userSettings);
      }

      [Observation]
      public void should_use_the_user_settings_for_all_options()
      {
         _result.CheckDimensions.ShouldBeTrue();
         _result.ShowPKSimDimensionProblemWarnings.ShouldBeTrue();
         _result.ShowCannotCalcErrors.ShouldBeTrue();
         _result.ShowPKSimObserverMessages.ShouldBeTrue();
         _result.CheckRules.ShouldBeTrue();
         _result.CheckCircularReference.ShouldBeTrue();
         _result.ShowUnresolvedEndosomesWarningsForInitialConditions.ShouldBeTrue();
         sut.WarnForNonFiniteQuantitiesWith(_userSettings).ShouldBeTrue();
      }
   }

   public class When_resolving_validation_options_that_are_set : concern_for_SimulationValidationOptions
   {
      private ValidationSettings _result;

      protected override void Context()
      {
         base.Context();
         sut.CheckRules = false;
         sut.ShowPKSimObserverMessages = false;
         sut.WarnForNonFiniteQuantities = false;
      }

      protected override void Because()
      {
         _result = sut.ValidationSettingsWith(_userSettings);
      }

      [Observation]
      public void should_use_the_options_that_are_set()
      {
         _result.CheckRules.ShouldBeFalse();
         _result.ShowPKSimObserverMessages.ShouldBeFalse();
         sut.WarnForNonFiniteQuantitiesWith(_userSettings).ShouldBeFalse();
      }

      [Observation]
      public void should_use_the_user_settings_for_the_other_options()
      {
         _result.CheckDimensions.ShouldBeTrue();
         _result.CheckCircularReference.ShouldBeTrue();
      }
   }

   public class When_creating_validation_options_from_validation_settings : concern_for_SimulationValidationOptions
   {
      private ValidationSettings _validationSettings;

      protected override void Context()
      {
         base.Context();
         _validationSettings = new ValidationSettings
         {
            CheckDimensions = true,
            ShowPKSimDimensionProblemWarnings = false,
            ShowCannotCalcErrors = true,
            ShowPKSimObserverMessages = false,
            CheckRules = true,
            CheckCircularReference = false,
            ShowUnresolvedEndosomesWarningsForInitialConditions = true
         };
      }

      protected override void Because()
      {
         sut = SimulationValidationOptions.From(_validationSettings, warnForNonFiniteQuantities: false);
      }

      [Observation]
      public void should_set_all_options()
      {
         sut.CheckDimensions.ShouldBeEqualTo(true);
         sut.ShowPKSimDimensionProblemWarnings.ShouldBeEqualTo(false);
         sut.ShowCannotCalcErrors.ShouldBeEqualTo(true);
         sut.ShowPKSimObserverMessages.ShouldBeEqualTo(false);
         sut.CheckRules.ShouldBeEqualTo(true);
         sut.CheckCircularReference.ShouldBeEqualTo(false);
         sut.ShowUnresolvedEndosomesWarningsForInitialConditions.ShouldBeEqualTo(true);
         sut.WarnForNonFiniteQuantities.ShouldBeEqualTo(false);
      }
   }

   public class When_cloning_validation_options : concern_for_SimulationValidationOptions
   {
      private SimulationValidationOptions _clone;

      protected override void Context()
      {
         base.Context();
         sut.CheckRules = true;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
         sut.CheckRules = false;
      }

      [Observation]
      public void should_return_an_independent_copy()
      {
         _clone.ShouldNotBeEqualTo(sut);
         _clone.CheckRules.ShouldBeEqualTo(true);
         _clone.CheckDimensions.ShouldBeNull();
      }
   }
}
