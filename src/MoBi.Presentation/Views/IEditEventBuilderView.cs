using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditEventBuilderView : IView<IEditEventBuilderPresenter>, IActivatableView
   {
      void Show(EventBuilderDTO dtoEventBuilder);
      void SetParametersView(IView view);
      void SetFormulaView(IView view);
      void SetSelectReferenceView(IView view);

      void NameValidationError(string message);
      void ResetNameValidationError();
      void ShowParameters();
   }
}