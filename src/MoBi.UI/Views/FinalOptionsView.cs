using OSPSuite.Assets;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class FinalOptionsView : BaseUserControl, IFinalOptionsView
   {
      private IFinalOptionsPresenter _presenter;

      public FinalOptionsView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.FinalOptions;
         layoutGroupValidation.Text = AppConstants.Captions.ValidationOptions;
         layoutGroupValidation.Padding = new Padding(0);
         layoutItemValidationOptions.TextVisible = false;
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return _presenter.Icon; }
      }

      public void AttachPresenter(IFinalOptionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetValidationOptionsView(IView view)
      {
         pnlValidationOptions.FillWith(view);
      }
   }
}