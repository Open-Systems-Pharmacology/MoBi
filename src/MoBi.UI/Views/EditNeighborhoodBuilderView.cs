using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditNeighborhoodBuilderView : BaseUserControl, IEditNeighborhoodBuilderView
   {
      protected ScreenBinder<NeighborhoodBuilderDTO> _screenBinder;
      protected bool _readOnly;
      private readonly UserLookAndFeel _lookAndFeel;
      private IEditNeighborhoodBuilderPresenter _presenter;

      public EditNeighborhoodBuilderView(UserLookAndFeel lookAndFeel)
      {
         _lookAndFeel = lookAndFeel;
         InitializeComponent();
      }

      public void AttachPresenter(IEditNeighborhoodBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<NeighborhoodBuilderDTO>();

         _screenBinder.Bind(dto => dto.Name)
            .To(tbName)
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.SetInitialName(e.NewValue));
         ;

         _screenBinder.Bind(dto => dto.FirstNeighborPath)
            .To(tbFirstNeighborPath)
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.SetFirstNeighborPath(e.NewValue));
         ;

         _screenBinder.Bind(dto => dto.SecondNeighborPath)
            .To(tbSecondNeighborPath)
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.SetSecondNeighborPath(e.NewValue));
         ;

         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor)
            .OnValueUpdating += onValueUpdating;


         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         tbName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);
         tbFirstNeighborPath.ButtonClick += (o, e) => OnEvent(_presenter.SelectFirstNeighbor);
         tbSecondNeighborPath.ButtonClick += (o, e) => OnEvent(_presenter.SelectSecondNeighbor);
      }

      public void Activate()
      {
         ActiveControl = tbName;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         tbName.ToolTip = ToolTips.Container.ContainerName;
         tabProperties.InitWith(AppConstants.Captions.Properties, ApplicationIcons.Properties);
         tabParameters.InitWith(AppConstants.Captions.Parameters, ApplicationIcons.Parameter);
         layoutItemFirstNeighborPath.Text = AppConstants.Captions.FirstNeighbor.FormatForLabel();
         layoutItemSecondNeighborPath.Text = AppConstants.Captions.SecondNeighbor.FormatForLabel();
         layoutControl.InitializeDisabledColors(_lookAndFeel);
      }

      private void onValueUpdating<T>(ContainerDTO container, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      private void initControls()
      {
         enableButton(tbFirstNeighborPath);
         enableButton(tbName);
         enableButton(tbSecondNeighborPath);

         tbName.ReadOnly = true;
         tbFirstNeighborPath.ReadOnly = false;
         tbSecondNeighborPath.ReadOnly = false;
      }

      private EditorButton getButton(ButtonEdit button) => button.Properties.Buttons[0];

      private void enableButton(ButtonEdit buttonEdit)
      {
         var button = getButton(buttonEdit);
         button.Enabled = true;
         editNameButton.Visible = !_readOnly;
         //enabled true otherwise the button cannot be clicked
         buttonEdit.Enabled = true;
      }

      private EditorButton editNameButton => getButton(tbName);
      private EditorButton changeFirstNeighborButton => getButton(tbFirstNeighborPath);
      private EditorButton changeSecondNeighborButton => getButton(tbSecondNeighborPath);

      public void AddParameterView(IView view)
      {
         tabParameters.FillWith(view);
      }

      public void AddTagsView(IView view)
      {
         panelTags.FillWith(view);
      }

      public override bool HasError => base.HasError || _screenBinder.HasError;

      public virtual bool ReadOnly
      {
         get => _readOnly;
         set
         {
            _readOnly = value;
            var enabled = !_readOnly;
            layoutControl.Enabled = enabled;
            tabProperties.Enabled = enabled;
         }
      }

      public bool ContainerPropertiesEditable
      {
         get => tbName.Enabled;
         set
         {
            tbName.Enabled = value;
            if (value) return;
            editNameButton.Visible = false;
            changeFirstNeighborButton.Visible = false;
            changeSecondNeighborButton.Visible = false;
         }
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }

      public void BindTo(NeighborhoodBuilderDTO neighborhoodBuilderDTO)
      {
         _screenBinder.BindToSource(neighborhoodBuilderDTO);
         initControls();
      }
   }
}