using System.Drawing;
using System.Windows.Forms;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SplashScreen : BaseView, ISplashScreen
   {
      private ISplashScreenPresenter _presenter;

      public SplashScreen()
      {
         InitializeComponent();

         progressBar.Properties.ShowTitle = true;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = FormBorderStyle.None;
         StartPosition = FormStartPosition.CenterScreen;
         ClientSize = new Size(BackgroundImage.Size.Width, BackgroundImage.Size.Height);
         ShowInTaskbar = false;
         TopMost = true;
         Opacity = 0.9;
         progressBar.Properties.ShowTitle = true;
         lblCaption.Text = string.Empty;
         Icon = ApplicationIcons.MoBi;
      }

      public void StopProgress()
      {
         ShowProgress(100, "Finished");
      }

      public void StartProgress(int startingProgress, string caption)
      {
         ShowProgress(startingProgress, caption);
      }

      public void ShowProgress(int progress, string caption)
      {
         lblCaption.Text = caption;
         progressBar.EditValue = progress;
      }

      public void AttachPresenter(ISplashScreenPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}