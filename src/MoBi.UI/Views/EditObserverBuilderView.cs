using System.Collections.Generic;
using System.Drawing;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditObserverBuilderView : BaseUserControl, IEditObserverBuilderView, IViewWithPopup
   {
      private IEditObserverBuilderPresenter _presenter;
      private ScreenBinder<ObserverBuilderDTO> _screenBinder;
      private Image _errorImage;
      private Image _observerImage;

      public EditObserverBuilderView()
      {
         InitializeComponent();
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_Observers;

      public bool FormulaHasError
      {
         set
         {
            tabFormula.Image = value ? _errorImage : _observerImage;
            tabFormula.Tooltip = value ? AppConstants.Exceptions.ErrorInFormula : string.Empty;
         }
      }

      public void AddMoleculeListView(IView view)
      {
         panelMoleculeList.FillWith(view);
      }

      public void AddDescriptorConditionListView(IView view)
      {
         panelDescriptorConditionList.FillWith(view);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ObserverBuilderDTO>();
         _screenBinder.Bind(dto => dto.Name)
            .To(btName)
            .OnValueSet += onPropertySet;

         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor)
            .OnValueSet += onPropertySet;

         _screenBinder.Bind(dto => dto.Dimension)
            .To(cbDimension)
            .WithValues(dto => allDimensions())
            .OnValueSet += onDimensionSet;

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btName.ButtonClick += btNameButtonClick;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btName.ToolTip = ToolTips.Observer.ObserverName;
         htmlEditor.ToolTip = ToolTips.Description;
         _errorImage = ApplicationIcons.DxError.ToImage();
         _observerImage = ApplicationIcons.Observer.ToImage();
         layoutItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutItemDimension.Text = AppConstants.Captions.Dimension.FormatForLabel();
         layoutItemMoleculeList.TextVisible = false;
         layoutItemDescriptorConditionList.TextVisible = false;
         layoutGroupInContainerWith.Text = AppConstants.Captions.InContainerWith.FormatForLabel();
      }

      public void Activate()
      {
         ActiveControl = btName;
      }

      private IEnumerable<IDimension> allDimensions()
      {
         return _presenter.AllDimensions();
      }

      private void onDimensionSet(ObserverBuilderDTO observerBuilder, PropertyValueSetEventArgs<IDimension> e)
      {
         OnEvent(() => _presenter.UpdateDimension(e.NewValue));
      }

      private void onPropertySet<T>(ObserverBuilderDTO observerBuilder, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      public void AttachPresenter(IEditObserverBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public virtual void BindTo(ObserverBuilderDTO observerBuilderDTO)
      {
         initNameEditControl(observerBuilderDTO.Name);
         _screenBinder.BindToSource(observerBuilderDTO);
      }

      private void initNameEditControl(string name)
      {
         var isInit = name.IsNullOrEmpty();
         btName.Properties.Buttons[0].Visible = !isInit;
         btName.Properties.ReadOnly = !isInit;
      }

      public void SetFormulaView(IView view)
      {
         tabFormula.FillWith(view);
      }

      public BarManager PopupBarManager => barManager;

      private void btNameButtonClick(object sender, ButtonPressedEventArgs e)
      {
         OnEvent(() => _presenter.RenameSubject());
      }

      public override bool HasError => base.HasError || _screenBinder.HasError;
   }
}