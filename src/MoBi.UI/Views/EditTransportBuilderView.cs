using System;
using System.Drawing;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditTransportBuilderView : BaseUserControl, IEditTransportBuilderView, IViewWithPopup
   {
      private IEditTransportBuilderPresenter _presenter;
      private ScreenBinder<TransportBuilderDTO> _screenBinder;
      private Image _errorImage;
      private Image _passiveTransportImage;

      public EditTransportBuilderView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      public bool FormulaHasError
      {
         set
         {
            tabKinetic.Image = value ? _errorImage : _passiveTransportImage;
            tabKinetic.Tooltip = value ? AppConstants.Exceptions.ErrorInFormula : String.Empty;
         }
      }

      public void EnableDisablePlotProcessRateParameter(bool enable)
      {
         chkPlotParameter.Enabled = enable;
      }

      public void AddSourceCriteriaView(IView view)
      {
         panelSource.FillWith(view);
      }

      public void AddTargetCriteriaView(IView view)
      {
         panelTarget.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         splitContainerControl1.CollapsePanel = SplitCollapsePanel.Panel1;
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutItemTagKinetic.TextVisible = false;
         chkCreateParameter.Text = AppConstants.Captions.CreateProcessRateParameter;
         chkPlotParameter.Text = AppConstants.Captions.PlotProcessRateParameter;
         tabKinetic.Text = AppConstants.Captions.Kinetic;
         tabParameters.Image = ApplicationIcons.Parameter;
         tabProperties.Image = ApplicationIcons.Properties;
         _errorImage = ApplicationIcons.DxError.ToImage();
         _passiveTransportImage = ApplicationIcons.PassiveTransport.ToImage();
         splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
         layoutItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutItemPanelSource.Text = AppConstants.Captions.Source.FormatForLabel();
         layouytItemPanelTarget.Text = AppConstants.Captions.Target.FormatForLabel();
         htmlEditor.Properties.ShowIcon = false;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<TransportBuilderDTO>();
         _screenBinder.Bind(dto => dto.Name).To(btName).OnValueUpdating += OnValueUpdating;
         _screenBinder.Bind(dto => dto.Description).To(htmlEditor).OnValueUpdating += OnValueUpdating;
         _screenBinder.Bind(dto => dto.CreateProcessRateParameter).To(chkCreateParameter).OnValueUpdating +=
            onCreateParameterSet;
         _screenBinder.Bind(dto => dto.ProcessRateParameterPersistable).To(chkPlotParameter).OnValueUpdating += onPlotParameterSet;

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btName.ButtonClick += btName_ButtonClick;
      }

      private void onPlotParameterSet(TransportBuilderDTO dto, PropertyValueSetEventArgs<bool> e)
      {
         OnEvent(() => _presenter.SetPlotProcessRateParameter(e.NewValue));
      }

      private void onCreateParameterSet(TransportBuilderDTO dto, PropertyValueSetEventArgs<bool> e)
      {
         OnEvent(() => _presenter.SetCreateProcessRateParameter(e.NewValue));
      }

      public override bool HasError
      {
         get { return base.HasError || _screenBinder.HasError; }
      }

      public void Activate()
      {
         ActiveControl = btName;
      }

      private void OnValueUpdating<T>(TransportBuilderDTO transportBuilder, PropertyValueSetEventArgs<T> e)
      {
         this.DoWithinExceptionHandler(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      public void AttachPresenter(IEditTransportBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(TransportBuilderDTO dto)
      {
         _screenBinder.BindToSource(dto);

         var isNewBuilder = dto.Name.IsNullOrEmpty();
         btName.Properties.ReadOnly = !isNewBuilder;
         btName.Properties.Buttons[0].Visible = !isNewBuilder;
      }

      public bool ShowMoleculeList
      {
         set { splitContainerControl1.PanelVisibility = value ? SplitPanelVisibility.Both : SplitPanelVisibility.Panel1; }
      }

      public void AddMoleculeSelectionView(IView view)
      {
         splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
         splitContainerControl1.Panel2.FillWith(view);
      }

      public void SetFormulaView(IView view)
      {
         tabKinetic.FillWith(view);
      }

      public void SetParameterView(IView view)
      {
         tabParameters.FillWith(view);
      }

      public void ShowParamters()
      {
         tabParameters.Show();
      }

      public BarManager PopupBarManager
      {
         get { return barManager; }
      }

      private void btName_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         this.DoWithinExceptionHandler(() => _presenter.RenameSubject());
      }
   }
}