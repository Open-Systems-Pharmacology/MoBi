using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditReactionBuildingBlockView : IView<IEditReactionBuildingBlockPresenter>,
      IEditBuildingBlockBaseView
   {
      void SetEditReactionView(IView view);
      void SetReactionListView(IView view);
      void SetReactionDiagram(IView view);
      void SetFavoritesReactionView(IView view);
   }
}