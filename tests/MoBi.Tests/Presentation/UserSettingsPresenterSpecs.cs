using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation
{
   public abstract class concern_for_UserSettingsPresenter : ContextSpecification<IUserSettingsPresenter>
   {
      protected IUserSettingsView _view;
      protected IDiagramOptionsPresenter _diagramOptionsPresenter;
      protected IChartOptionsPresenter _chartOptionsPresenter;
      protected IValidationOptionsPresenter _validationOptionsPresenter;
      protected IDisplayUnitsPresenter _displayUnitsPresenter;
      protected IApplicationSettingsPresenter _applicationSettingsPresenter;
      protected IApplicationSettings _applicationSettings;
      protected ICloneableUserSettings _userSettings;
      protected IUserSettings _clone;

      protected override void Context()
      {
         _view = A.Fake<IUserSettingsView>();
         _diagramOptionsPresenter = A.Fake<IDiagramOptionsPresenter>();
         _chartOptionsPresenter = A.Fake<IChartOptionsPresenter>();
         _validationOptionsPresenter = A.Fake<IValidationOptionsPresenter>();
         _displayUnitsPresenter = A.Fake<IDisplayUnitsPresenter>();
         _applicationSettingsPresenter = A.Fake<IApplicationSettingsPresenter>();
         _applicationSettings = A.Fake<IApplicationSettings>();

         A.CallTo(() => _applicationSettings.PKSimPath).Returns("original_path");

         _userSettings = A.Fake<ICloneableUserSettings>();
         _clone = A.Fake<ICloneableUserSettings>();

         A.CallTo(() => _clone.DiagramOptions).Returns(new DiagramOptions());
         A.CallTo(() => _clone.ChartOptions).Returns(new ChartOptions());
         A.CallTo(() => _clone.ValidationSettings).Returns(new ValidationSettings());
         A.CallTo(() => _userSettings.Clone()).Returns(_clone);

         sut = new UserSettingsPresenter(
            _view,
            _diagramOptionsPresenter,
            _chartOptionsPresenter,
            _validationOptionsPresenter,
            _displayUnitsPresenter,
            _applicationSettingsPresenter,
            _applicationSettings);
      }
   }

   public class When_editing_user_settings_and_user_cancels : concern_for_UserSettingsPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.Edit(_userSettings);
      }

      [Observation]
      public void should_not_update_the_original_user_settings()
      {
         A.CallTo(() => _userSettings.UpdatePropertiesFrom(A<IUserSettings>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_update_application_settings()
      {
         A.CallToSet(() => _applicationSettings.PKSimPath).MustNotHaveHappened();
      }

      [Observation]
      public void should_bind_the_clone_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_clone)).MustHaveHappened();
      }

      [Observation]
      public void should_edit_cloned_sub_objects_not_originals()
      {
         A.CallTo(() => _diagramOptionsPresenter.Edit(_clone.DiagramOptions)).MustHaveHappened();
         A.CallTo(() => _chartOptionsPresenter.Edit(_clone.ChartOptions)).MustHaveHappened();
         A.CallTo(() => _validationOptionsPresenter.Edit(_clone.ValidationSettings)).MustHaveHappened();
      }

      [Observation]
      public void should_edit_application_settings_dto_in_sub_presenter()
      {
         A.CallTo(() => _applicationSettingsPresenter.Edit(A<ApplicationSettingsDTO>._)).MustHaveHappened();
      }
   }

   public class When_editing_user_settings_and_user_confirms : concern_for_UserSettingsPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);

         A.CallTo(() => _applicationSettingsPresenter.Edit(A<ApplicationSettingsDTO>._))
            .Invokes((ApplicationSettingsDTO dto) => dto.PKSimPath = "modified_path");
      }

      protected override void Because()
      {
         sut.Edit(_userSettings);
      }

      [Observation]
      public void should_update_the_original_user_settings_from_the_clone()
      {
         A.CallTo(() => _userSettings.UpdatePropertiesFrom(_clone)).MustHaveHappened();
      }

      [Observation]
      public void should_update_application_settings_from_dto()
      {
         A.CallToSet(() => _applicationSettings.PKSimPath).To("modified_path").MustHaveHappened();
      }

      [Observation]
      public void should_bind_the_clone_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_clone)).MustHaveHappened();
      }
   }
}
