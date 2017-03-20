using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectAndEditContainerView : IView<ISelectAndEditPresenter>
   {
      void AddEditView(IView view);
      string Description { set; }
      void AddLegendView(IView view);
   }
}