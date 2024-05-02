using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditEventAssignmentBuilderView : IView<IEditAssignmentBuilderPresenter>,  IActivatableView
   {
      void Show(EventAssignmentBuilderDTO eventAssignmentBuilderDTO);
      void SetFormulaView(IView subView);
   }
}