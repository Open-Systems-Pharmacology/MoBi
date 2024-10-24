using System.Drawing;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IContainerModalView : IModalView<IModalPresenter>
   {
      void AddSubView(IView subView);
      bool Show();
      bool Show(Size modalSize);
      bool CanClose { get; set; }
   }
}