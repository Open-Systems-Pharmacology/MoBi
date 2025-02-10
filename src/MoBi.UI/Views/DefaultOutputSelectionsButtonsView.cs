using System;
using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class DefaultOutputSelectionsButtonsView : BaseUserControl, IDefaultOutputSelectionsButtonsView
   {
      public DefaultOutputSelectionsButtonsView()
      {
         InitializeComponent();
         btnMakeProjectDefaults.Click += (o, e) => makeProjectDefaultsClicked();
         btnLoadProjectDefaults.Click += (o, e) => loadProjectDefaultsClicked();

         btnMakeProjectDefaults.Text = AppConstants.Captions.MakeDefault;
         btnLoadProjectDefaults.Text = AppConstants.Captions.LoadFromDefaults;
      }

      private void loadProjectDefaultsClicked()
      {
         LoadProjectDefaultsClicked();
      }

      private void makeProjectDefaultsClicked()
      {
         MakeProjectDefaultsClicked();
      }

      public Action MakeProjectDefaultsClicked { get; set; } = () => { };
      public Action LoadProjectDefaultsClicked { get; set; } = () => { };

   }
}
