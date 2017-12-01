using FakeItEasy;
using MoBi.Core;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;

namespace MoBi.Presentation
{
   public abstract class concern_for_ValidationOptionsPresenter : ContextSpecification<IValidationOptionsPresenter>
   {
      protected IValidationOptionsView _view;

      protected override void Context()
      {
         _view= A.Fake<IValidationOptionsView>();
         sut = new ValidationOptionsPresenter(_view);
      }
   }

   public class When_the_validation_option_presenter_is_editing_the_validation_settings : concern_for_ValidationOptionsPresenter
   {
      private ValidationSettings _validationOptions;

      protected override void Context()
      {
         base.Context();
         _validationOptions= new ValidationSettings();
      }

      protected override void Because()
      {
         sut.Edit(_validationOptions);   
      }

      [Observation]
      public void should_bind_the_view_to_the_validation_settings()
      {
         A.CallTo(() => _view.BindTo(_validationOptions)).MustHaveHappened();
      }
   }
}	