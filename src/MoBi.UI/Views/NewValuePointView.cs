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
      private ValueEdit _veX;
      private ValueEdit _veY;

      public NewValuePointView(IMainView mainView)
         : base(mainView)
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ValuePointDTO>();
         initializeValueEdits();
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.X).To(_veX);
         _screenBinder.Bind(x => x.Y).To(_veY);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemX.Text = "X";
         _veX.ToolTip = ToolTips.Formula.X;
         layoutControlItemY.Text = "Y";
         _veY.ToolTip = ToolTips.Formula.Y;
         Text = AppConstants.Captions.NewValuePoint;
      }

      public void BindTo(ValuePointDTO valuePointDTO)
      {
         _screenBinder.BindToSource(valuePointDTO);
      }

      private void initializeValueEdits()
      {
         _veX = new ValueEdit();
         _veX.Name = "veX";
         _veX.Size = new System.Drawing.Size(433, 20);
         _veX.TabIndex = 4;
         _veX.Location = new System.Drawing.Point(109, 12);
         layoutControl1.Controls.Add(_veX);
         layoutControlItemX.Control = _veX;

         _veY = new ValueEdit();
         layoutControl1.Controls.Add(_veY);
         _veY.Name = "veY";
         _veY.Size = new System.Drawing.Size(433, 20);
         _veY.Location = new System.Drawing.Point(109, 36);
         _veY.TabIndex = 5;
         layoutControlItemY.Control = _veY;
      }

      public void AttachPresenter(INewValuePointPresenter presenter)
      {
      }
   }
}