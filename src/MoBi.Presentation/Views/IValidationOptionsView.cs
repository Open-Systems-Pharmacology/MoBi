using MoBi.Core;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IValidationOptionsView : IView<IValidationOptionsPresenter>
   {
      void EnableDisableValidationSubOptions(bool enabled);
      void Show(ValidationSettings validationOptions);
   }
}