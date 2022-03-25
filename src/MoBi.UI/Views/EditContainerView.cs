﻿using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditContainerView : BaseUserControl, IEditContainerView
   {
      private readonly IImageListRetriever _imageListRetriever;
      protected IEditContainerPresenter _presenter;
      protected ScreenBinder<ContainerDTO> _screenBinder;
      protected bool _readOnly;

      public EditContainerView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
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


         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);
      }

      public void Activate()
      {
         ActiveControl = btName;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         btName.ToolTip = ToolTips.Container.ContainerName;
         tabProperties.InitWith(AppConstants.Captions.Properties, ApplicationIcons.Properties);
         tabParameters.InitWith(AppConstants.Captions.Parameters, ApplicationIcons.Parameter);
      }

      private void onNameSet(ContainerDTO container, PropertyValueSetEventArgs<string> e)
      {
         OnEvent(() => _presenter.SetInitialName(e.NewValue));
      }

      private void onValueUpdating<T>(ContainerDTO container, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }
   
      public void AttachPresenter(IEditContainerPresenter presenter)
      {
         _presenter = presenter;
      }

      public virtual void BindTo(ContainerDTO dto)
      {
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