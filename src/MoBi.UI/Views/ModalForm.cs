using System.Drawing;
using MoBi.Presentation;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ModalForm : BaseModalView, IContainerModalView
   {
      public ModalForm()
      {
         InitializeComponent();
      }

      public void AddSubView(IView subView)
      {
         pnlControl.FillWith(subView);
         var view = subView as IActivatableView;
         view?.Activate();
      }

      public new bool Show()
      {
         Display();
         return !Canceled;
      }

      public bool Show(Size modalSize)
      {
         Size = modalSize;
         return Show();
      }

      public void AttachPresenter(IModalPresenter presenter)
      {
      }
   }
}