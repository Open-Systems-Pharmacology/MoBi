using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditEventGroupBuildingBlockView : IView<IEditEventGroupBuildingBlockPresenter>, IEditBuildingBlockBaseView
   {
      void SetListView(IView view);
      void SetEditView(IView editEventView);
   }
}