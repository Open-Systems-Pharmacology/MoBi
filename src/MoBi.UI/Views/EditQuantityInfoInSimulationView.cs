using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditQuantityInfoInSimulationView : BaseUserControl, IEditQuantityInfoInSimulationView
   {
      private readonly ScreenBinder<QuantityDTO> _screenBinder = new ScreenBinder<QuantityDTO>();
      private IEditQuantityInfoInSimulationPresenter _presenter;

      public EditQuantityInfoInSimulationView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         nameLayoutControlItem.Text = AppConstants.Captions.Name.FormatForLabel();
         descriptionLayoutControlItem.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutControlItemSource.Text = AppConstants.Captions.Source.FormatForLabel();

         htmlEditor.ReadOnly = true;
         tbName.ReadOnly = true;
         tbSource.ReadOnly = true;

         btnGoToSource.InitWithImage(ApplicationIcons.Search, text: AppConstants.Captions.GoToSource);
         layoutControlItemGoToSource.AdjustControlSize(OSPSuite.UI.UIConstants.Size.BUTTON_WIDTH, layoutControlItemGoToSource.ControlMaxSize.Height);
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(item => item.Description).To(htmlEditor);
         _screenBinder.Bind(item => item.Name).To(tbName);
         _screenBinder.Bind(item => item.SourceDisplayName).To(tbSource);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         btnGoToSource.Click += (o, e) => OnEvent(() => _presenter.NavigateToQuantitySource());
      }

      public void BindTo(QuantityDTO dto)
      {
         _screenBinder.BindToSource(dto);

         layoutControlItemSource.Visibility = LayoutVisibilityConvertor.FromBoolean(dto.SourceReference != null);
         layoutControlItemGoToSource.Visibility = layoutControlItemSource.Visibility;
      }

      public void AttachPresenter(IEditQuantityInfoInSimulationPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}