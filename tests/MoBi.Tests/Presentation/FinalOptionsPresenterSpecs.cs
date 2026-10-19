using FakeItEasy;
using MoBi.Core;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_FinalOptionsPresenter : ContextSpecification<FinalOptionsPresenter>
   {
      protected IFinalOptionsView _view;
      protected IValidationOptionsPresenter _validationOptionsPresenter;
      protected IUserSettings _userSettings;
      protected ValidationSettings _userValidationSettings;
      protected ValidationSettings _editedValidationSettings;
      protected FinalOptionsDTO _finalOptionsDTO;

      protected override void Context()
      {
         _view = A.Fake<IFinalOptionsView>();
         _validationOptionsPresenter = A.Fake<IValidationOptionsPresenter>();
         _userSettings = A.Fake<IUserSettings>();
         _userValidationSettings = new ValidationSettings { CheckDimensions = true, CheckCircularReference = true, CheckRules = false };
         A.CallTo(() => _userSettings.ValidationSettings).Returns(_userValidationSettings);
         _userSettings.WarnForNonFiniteQuantities = true;

         A.CallTo(() => _validationOptionsPresenter.Edit(A<ValidationSettings>._))
            .Invokes(x => _editedValidationSettings = x.GetArgument<ValidationSettings>(0));
         A.CallTo(() => _view.BindTo(A<FinalOptionsDTO>._))
            .Invokes(x => _finalOptionsDTO = x.GetArgument<FinalOptionsDTO>(0));

         sut = new FinalOptionsPresenter(_view, _validationOptionsPresenter, _userSettings);
      }
   }

   public class When_creating_the_final_options_presenter : concern_for_FinalOptionsPresenter
   {
      [Observation]
      public void should_add_the_validation_options_view_to_the_view()
      {
         A.CallTo(() => _view.SetValidationOptionsView(_validationOptionsPresenter.View)).MustHaveHappened();
      }
   }

   public class When_the_final_options_presenter_is_editing_a_simulation_configuration : concern_for_FinalOptionsPresenter
   {
      protected override void Because()
      {
         sut.Edit(new SimulationConfiguration());
      }

      [Observation]
      public void should_edit_a_copy_of_the_user_validation_settings()
      {
         _editedValidationSettings.ShouldNotBeNull();
         _editedValidationSettings.ShouldNotBeEqualTo(_userValidationSettings);
         _editedValidationSettings.CheckDimensions.ShouldBeTrue();
         _editedValidationSettings.CheckCircularReference.ShouldBeTrue();
         _editedValidationSettings.CheckRules.ShouldBeFalse();
      }

      [Observation]
      public void should_bind_the_view_to_the_user_setting_for_non_finite_quantity_warnings()
      {
         _finalOptionsDTO.ShouldNotBeNull();
         _finalOptionsDTO.WarnForNonFiniteQuantities.ShouldBeTrue();
      }
   }

   public class When_the_user_changes_the_final_options_without_saving : concern_for_FinalOptionsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(new SimulationConfiguration());
      }

      protected override void Because()
      {
         _editedValidationSettings.CheckCircularReference = false;
         _editedValidationSettings.CheckRules = true;
         _finalOptionsDTO.WarnForNonFiniteQuantities = false;
      }

      [Observation]
      public void should_not_change_the_user_validation_settings()
      {
         _userValidationSettings.CheckCircularReference.ShouldBeTrue();
         _userValidationSettings.CheckRules.ShouldBeFalse();
      }

      [Observation]
      public void should_not_change_the_user_setting_for_non_finite_quantity_warnings()
      {
         _userSettings.WarnForNonFiniteQuantities.ShouldBeTrue();
      }
   }

   public class When_saving_the_final_options : concern_for_FinalOptionsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(new SimulationConfiguration());
         _editedValidationSettings.CheckCircularReference = false;
         _editedValidationSettings.CheckRules = true;
         _finalOptionsDTO.WarnForNonFiniteQuantities = false;
      }

      protected override void Because()
      {
         sut.SaveOptions();
      }

      [Observation]
      public void should_update_the_user_validation_settings_with_the_edited_options()
      {
         _userValidationSettings.CheckCircularReference.ShouldBeFalse();
         _userValidationSettings.CheckRules.ShouldBeTrue();
         _userValidationSettings.CheckDimensions.ShouldBeTrue();
      }

      [Observation]
      public void should_update_the_user_setting_for_non_finite_quantity_warnings()
      {
         _userSettings.WarnForNonFiniteQuantities.ShouldBeFalse();
      }
   }
}
