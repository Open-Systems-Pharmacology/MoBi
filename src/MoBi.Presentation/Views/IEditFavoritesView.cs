using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditFavoritesView : IView<IEditFavoritesPresenter>
   {
      void AddParametersView(IView view);
   }
}