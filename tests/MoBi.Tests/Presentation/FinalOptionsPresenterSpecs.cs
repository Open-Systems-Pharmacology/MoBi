using FakeItEasy;
using MoBi.Core;
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

      protected override void Context()
      {
         _view = A.Fake<IFinalOptionsView>();
         _validationOptionsPresenter = A.Fake<IValidationOptionsPresenter>();
         _userSettings = A.Fake<IUserSettings>();
         _userValidationSettings = new ValidationSettings { CheckDimensions = true, CheckCircularReference = true, CheckRules = false };
         A.CallTo(() => _userSettings.ValidationSettings).Returns(_userValidationSettings);
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
      private ValidationSettings _editedValidationSettings;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _validationOptionsPresenter.Edit(A<ValidationSettings>._))
            .Invokes(x => _editedValidationSettings = x.GetArgument<ValidationSettings>(0));
      }

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
   }

   public class When_the_user_changes_the_final_options_without_saving : concern_for_FinalOptionsPresenter
   {
      private ValidationSettings _editedValidationSettings;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _validationOptionsPresenter.Edit(A<ValidationSettings>._))
            .Invokes(x => _editedValidationSettings = x.GetArgument<ValidationSettings>(0));
         sut.Edit(new SimulationConfiguration());
      }

      protected override void Because()
      {
         _editedValidationSettings.CheckCircularReference = false;
         _editedValidationSettings.CheckRules = true;
      }

      [Observation]
      public void should_not_change_the_user_settings()
      {
         _userValidationSettings.CheckCircularReference.ShouldBeTrue();
         _userValidationSettings.CheckRules.ShouldBeFalse();
      }
   }

   public class When_saving_the_final_options : concern_for_FinalOptionsPresenter
   {
      private ValidationSettings _editedValidationSettings;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _validationOptionsPresenter.Edit(A<ValidationSettings>._))
            .Invokes(x => _editedValidationSettings = x.GetArgument<ValidationSettings>(0));
         sut.Edit(new SimulationConfiguration());
         _editedValidationSettings.CheckCircularReference = false;
         _editedValidationSettings.CheckRules = true;
      }

      protected override void Because()
      {
         sut.SaveOptions();
      }

      [Observation]
      public void should_update_the_user_settings_with_the_edited_options()
      {
         _userValidationSettings.CheckCircularReference.ShouldBeFalse();
         _userValidationSettings.CheckRules.ShouldBeTrue();
         _userValidationSettings.CheckDimensions.ShouldBeTrue();
      }
   }
}
