using MoBi.Core;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IValidationOptionsPresenter : IPresenter<IValidationOptionsView>
   {
      void ValidateDimensionsChanged(bool newValue);
      void Edit(ValidationSettings validationOptions);
   }

   public class ValidationOptionsPresenter : AbstractPresenter<IValidationOptionsView, IValidationOptionsPresenter>, IValidationOptionsPresenter
   {
      public ValidationOptionsPresenter(IValidationOptionsView view) : base(view)
      {
      }

      public void ValidateDimensionsChanged(bool newValue)
      {
         _view.EnableDisableValidationSubOptions(newValue);
      }

      public void Edit(ValidationSettings validationOptions)
      {
         _view.EnableDisableValidationSubOptions(validationOptions.CheckDimensions);
         _view.Show(validationOptions);
      }
   }
}