using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using System.Drawing;

namespace MoBi.UI.Views
{
   public partial class ApplyToSelectionButtonView : BaseUserControl, IApplyToSelectionButtonView
   {
      protected IApplyToSelectionPresenter _presenter;
      protected readonly ScreenBinder<IApplyToSelectionPresenter> _screenBinder;

      public ApplyToSelectionButtonView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<IApplyToSelectionPresenter>();
      }

      public override string Caption
      {
         set => btnSelection.Text = value;
      }
      
      public override void InitializeBinding()
      {
         btnSelection.Click += (o, e) => OnEvent(_presenter.PerformSelectionHandler);
      }

      public void AttachPresenter(IApplyToSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindToSelection()
      {
         _screenBinder.BindToSource(_presenter);
      }

      public void SetButonIcon(ApplicationIcon icon)
      {
         btnSelection.InitWithImage(icon);
      }
   }
}