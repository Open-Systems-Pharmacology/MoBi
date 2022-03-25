using System;
using System.Windows.Forms;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SplashScreen : BaseView, ISplashScreen
   {
      public SplashScreen()
      {
         InitializeComponent();
         this.labelCopyright.Text = $"Copyright © 2002-{DateTime.Now.Year} - Open Systems Pharmacology Community";
         FormBorderStyle = FormBorderStyle.None;
         StartPosition = FormStartPosition.CenterScreen;
         ShowInTaskbar = false;
         Opacity = 0.98;
         progressBarControl.Properties.ShowTitle = true;
         Text = AppConstants.Captions.LoadingApplication;
         ApplicationIcon = ApplicationIcons.MoBi;
      }

      public void AttachPresenter(ISplashScreenPresenter presenter)
      {
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
         labelStatus.Text = caption;
         progressBarControl.EditValue = progress;
      }

      public override void InitializeResources()
      {
         labelStatus.Text = string.Empty;
      }
   }
}