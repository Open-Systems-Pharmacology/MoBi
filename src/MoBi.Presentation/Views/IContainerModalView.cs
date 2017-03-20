using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IContainerModalView : IModalView<IModalPresenter>
   {
      void AddSubView(IView subView);
      bool Show();
   }
}