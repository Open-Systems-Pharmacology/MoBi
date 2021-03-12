using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditEventGroupView : BaseUserControl, IEditEventGroupView, IViewWithPopup
   {
      private IEditEventGroupPresenter _presenter;
      private ScreenBinder<EventGroupBuilderDTO> _screenBinder;

      public EditEventGroupView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<EventGroupBuilderDTO>();
         _screenBinder.Bind(dto => dto.Name).To(btName).OnValueUpdating += onPropertySet;
         _screenBinder.Bind(dto => dto.Description).To(htmlEditor).OnValueUpdating += onPropertySet;
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         tabParameters.Text = AppConstants.Captions.Parameters;
         tabParameters.Image = ApplicationIcons.Parameter;
         tabProperties.Text = AppConstants.Captions.Properties;
         tabProperties.Image = ApplicationIcons.Properties;
         tabTags.Text = AppConstants.Captions.Tags;
         tabTags.Image = ApplicationIcons.Tag;
      }

      public void Activate()
      {
         ActiveControl = btName;
      }

      private void onPropertySet<T>(EventGroupBuilderDTO eventGroupBuilder, PropertyValueSetEventArgs<T> e)
      {
         this.DoWithinExceptionHandler(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      public void AttachPresenter(IEditEventGroupPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddParametersView(IView subView)
      {
         tabParameters.FillWith(subView);
      }

      public void BindTo(EventGroupBuilderDTO eventGroupBuilderDTO)
      {
         initNameEdit(eventGroupBuilderDTO);
         _screenBinder.BindToSource(eventGroupBuilderDTO);
      }

      private void initNameEdit(EventGroupBuilderDTO eventGroupBuilder)
      {
         var name = eventGroupBuilder.Name;
         var nameIsSet = name.IsNullOrEmpty();
         btName.Properties.ReadOnly = !nameIsSet;
         btName.Properties.Buttons[0].Visible = !nameIsSet;
      }

      public bool EnableDescriptors
      {
         get => grpContainerDescriptor.Enabled;
         set => grpContainerDescriptor.Enabled = value;
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }

      public void AddDescriptorConditionListView(IView view)
      {
         panelDescriptorCriteria.FillWith(view);
      }

      public void AddTagsView(IView view)
      {
         tabTags.FillWith(view);
      }

      public override bool HasError => base.HasError || _screenBinder.HasError;

      public BarManager PopupBarManager => barManager;

      private void btName_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         _presenter.RenameSubject();
      }

      
   }
}