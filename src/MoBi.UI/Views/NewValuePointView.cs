using MoBi.Assets;
using OSPSuite.DataBinding;

using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Helper;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class NewValuePointView : BaseModalView, INewValuePointView
   {
      private readonly ScreenBinder<ValuePointDTO> _screenBinder;

      public NewValuePointView(IMainView mainView)
         : base(mainView)
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ValuePointDTO>();
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.X).To(veX);
         _screenBinder.Bind(x => x.Y).To(veY);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemX.Text = "X";
         veX.ToolTip = ToolTips.Formula.X;
         layoutControlItemY.Text = "Y";
         veY.ToolTip = ToolTips.Formula.Y;
         Text = AppConstants.Captions.NewValuePoint;
      }

      public void BindTo(ValuePointDTO valuePointDTO)
      {
         _screenBinder.BindToSource(valuePointDTO);
      }

      public void AttachPresenter(INewValuePointPresenter presenter)
      {
      }
   }
}