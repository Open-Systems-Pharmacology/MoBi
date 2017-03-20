using System.Windows.Forms;
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
         var activatableView = subView as IActivatableView;
         activatableView?.Activate();
      }

      public new bool Show()
      {
         Display();
         return !Canceled;
      }

      public DialogResult ShowInformation()
      {
         btCanncel.Visible = false;
         btOK.Visible = false;
         btClose.Visible = true;
         return ShowDialog();
      }

      public DialogResult ShowOKCancel()
      {
         btCanncel.Visible = true;
         btOK.Visible = true;
         btClose.Visible = false;
         return ShowDialog();
      }

      public void AttachPresenter(IModalPresenter presenter)
      {
      }
   }
}