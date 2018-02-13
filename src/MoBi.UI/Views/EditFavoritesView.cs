using DevExpress.XtraEditors;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditFavoritesView : BaseUserControl, IEditFavoritesView
   {
      private IEditFavoritesPresenter _presenter;

      public EditFavoritesView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditFavoritesPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddParametersView(IView view)
      {
         panelParameters.FillWith(view);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         buttonMoveUp.Click += (o, e) => OnEvent(_presenter.MoveUp);
         buttonMoveDown.Click += (o, e) => OnEvent(_presenter.MoveDown);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         buttonMoveUp.InitWithImage(ApplicationIcons.Up, imageLocation: ImageLocation.MiddleCenter, toolTip: ToolTips.ParameterList.MoveUp);
         buttonMoveDown.InitWithImage(ApplicationIcons.Down, imageLocation: ImageLocation.MiddleCenter, toolTip: ToolTips.ParameterList.MoveDown);
         layoutItemButtonMoveUp.AdjustButtonSizeWithImageOnly();
         layoutItemButtonMoveDown.AdjustButtonSizeWithImageOnly();
      }
   }
}