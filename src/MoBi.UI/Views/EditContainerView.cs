using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditContainerView : BaseUserControl, IEditContainerView
   {
      protected IEditContainerPresenter _presenter;
      protected GridViewBinder<TagDTO> _gridBinder;
      protected ScreenBinder<ContainerDTO> _screenBinder;
      protected bool _readOnly;

      public EditContainerView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ContainerDTO>();
         _screenBinder.Bind(dto => dto.Mode).To(cbContainerMode)
            .WithValues(dto => _presenter.AllContainerModes())
            .AndDisplays(mode => _presenter.ContainerModeDisplayFor(mode))
            .OnValueUpdating += (o, e) => OnEvent((() => _presenter.SetContainerMode(e.NewValue)));

         _screenBinder.Bind(dto => dto.ContainerType)
            .To(cbContainerType)
            .WithValues(dto => _presenter.AllContainerTypes())
            .OnValueUpdating += onValueUpdating;

         _screenBinder.Bind(dto => dto.Name)
            .To(btName)
            .OnValueUpdating += onNameSet;

         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor)
            .OnValueUpdating += onValueUpdating;

         _gridBinder = new GridViewBinder<TagDTO>(gridView);
         _gridBinder.Bind(dto => dto.Value)
            .WithCaption(AppConstants.Captions.Tag)
            .AsReadOnly();

         var buttonRepository = createAddRemoveButtonRepository();
         buttonRepository.ButtonClick += (o, e) => OnEvent(() => onButtonClicked(e, _gridBinder.FocusedElement));

         _gridBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => buttonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH * 2);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btAddTag.Click += (o, e) => OnEvent(_presenter.AddNewTag);
         btName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);
      }

      public void Activate()
      {
         ActiveControl = btName;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         btAddTag.InitWithImage(ApplicationIcons.Add, text: AppConstants.Captions.AddTag, toolTip: ToolTips.Container.AddTag);
         btName.ToolTip = ToolTips.Container.ContainerName;
         layoutItemContainerTags.Text = AppConstants.Captions.ContainerTags.FormatForLabel();
         tabProperties.Image = ApplicationIcons.Properties;
         tabParameters.Image = ApplicationIcons.Parameter;
      }

      private void onNameSet(ContainerDTO container, PropertyValueSetEventArgs<string> e)
      {
         OnEvent(() => _presenter.SetInitialName(e.NewValue));
      }

      private void onValueUpdating<T>(ContainerDTO container, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      private RepositoryItemButtonEdit createAddRemoveButtonRepository()
      {
         var buttonRepository = new RepositoryItemButtonEdit {TextEditStyle = TextEditStyles.HideTextEditor};
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Plus;
         buttonRepository.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
         return buttonRepository;
      }

      private void onButtonClicked(ButtonPressedEventArgs buttonPressedEventArgs, TagDTO tagDTO)
      {
         var pressedButton = buttonPressedEventArgs.Button;
         if (pressedButton.Kind.Equals(ButtonPredefines.Plus))
            _presenter.AddNewTag();
         else
            _presenter.RemoveTag(tagDTO);
      }

      public void AttachPresenter(IEditContainerPresenter presenter)
      {
         _presenter = presenter;
      }

      public virtual void BindTo(ContainerDTO dto)
      {
         _gridBinder.BindToSource(dto.Tags);
         _screenBinder.BindToSource(dto);
         initNameControl(dto);
      }

      private void initNameControl(ContainerDTO dto)
      {
         var isInit = dto.Name.IsNullOrEmpty();
         editNameButton.Enabled = !isInit;
         editNameButton.Visible = !isInit && !_readOnly;
         btName.Properties.ReadOnly = !isInit;
      }

      private EditorButton editNameButton => btName.Properties.Buttons[0];

      public void SetParameterView(IView view)
      {
         tabParameters.FillWith(view);
      }

      public override bool HasError => base.HasError || _screenBinder.HasError || _gridBinder.HasError;

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
         get => cbContainerType.Enabled;
         set
         {
            cbContainerType.Enabled = value;
            cbContainerMode.Enabled = value;
            btName.Enabled = value;
            if (value) return;
            editNameButton.Visible = false;
         }
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }
   }
}