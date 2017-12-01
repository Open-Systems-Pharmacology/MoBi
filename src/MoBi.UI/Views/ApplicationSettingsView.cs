using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class ApplicationSettingsView : BaseUserControl, IApplicationSettingsView
   {
      private IApplicationSettingsPresenter _presenter;
      private readonly ScreenBinder<ApplicationSettingsDTO> _screenBinder = new ScreenBinder<ApplicationSettingsDTO>();

      public ApplicationSettingsView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IApplicationSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ApplicationSettingsDTO applicationSettingsDTO)
      {
         _screenBinder.BindToSource(applicationSettingsDTO);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.PKSimPath)
            .To(buttonPKSimPath);

         _screenBinder.Bind(x => x.UseWatermark)
            .To(chkUseWatermark)
            .WithCaption(AppConstants.Captions.UseWatermark);

         _screenBinder.Bind(x => x.WatermarkText)
            .To(textWatermark);

         buttonPKSimPath.ButtonClick += (o, e) => OnEvent(_presenter.SelectPKSimPath);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemPKSimPath.Text = AppConstants.Captions.PKSimPath.FormatForLabel(checkCase: false);
         layoutItemTextWatermark.Text = AppConstants.Captions.WatermarkText.FormatForLabel();
      }
   }
}