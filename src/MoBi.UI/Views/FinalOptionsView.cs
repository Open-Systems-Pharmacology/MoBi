using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class FinalOptionsView : BaseUserControl, IFinalOptionsView
   {
      private IFinalOptionsPresenter _presenter;
      private readonly ScreenBinder<FinalOptionsDTO> _screenBinder = new ScreenBinder<FinalOptionsDTO>();

      public FinalOptionsView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.WarnForNonFiniteQuantities).To(chkWarnForNonFiniteQuantities);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.FinalOptions;
         ApplicationIcon = ApplicationIcons.Run;
         chkWarnForNonFiniteQuantities.Text = AppConstants.Captions.WarnForNonFiniteQuantities;
         chkWarnForNonFiniteQuantities.ToolTip = ToolTips.WarnForNonFiniteQuantities;
         layoutGroupValidation.Text = AppConstants.Captions.ValidationOptions;
         layoutGroupValidation.Padding = new Padding(0);
         layoutItemValidationOptions.TextVisible = false;
      }

      public void AttachPresenter(IFinalOptionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetValidationOptionsView(IView view)
      {
         pnlValidationOptions.FillWith(view);
      }

      public void BindTo(FinalOptionsDTO finalOptionsDTO)
      {
         _screenBinder.BindToSource(finalOptionsDTO);
      }

      public override bool HasError => _screenBinder.HasError;
   }
}
