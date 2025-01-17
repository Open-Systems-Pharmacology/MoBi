using DevExpress.XtraBars;
using DevExpress.XtraEditors;
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
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditTransportBuilderView : BaseUserControl, IEditTransportBuilderView, IViewWithPopup
   {
      private IEditTransportBuilderPresenter _presenter;
      private readonly ScreenBinder<TransportBuilderDTO> _screenBinder;

      public EditTransportBuilderView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
         _screenBinder = new ScreenBinder<TransportBuilderDTO>();
      }

      public bool FormulaHasError
      {
         set
         {
            tabKinetic.SetImage(value ? ApplicationIcons.DxError : ApplicationIcons.PassiveTransport);
            tabKinetic.Tooltip = value ? AppConstants.Exceptions.ErrorInFormula : string.Empty;
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
         splitContainerControl.CollapsePanel = SplitCollapsePanel.Panel1;
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutItemTagKinetic.TextVisible = false;
         chkCreateParameter.Text = AppConstants.Captions.CreateProcessRateParameter;
         chkPlotParameter.Text = AppConstants.Captions.PlotProcessRateParameter;
         tabKinetic.InitWith(AppConstants.Captions.Kinetic, ApplicationIcons.PassiveTransport);
         tabParameters.InitWith(AppConstants.Captions.Parameters, ApplicationIcons.Parameter);
         tabProperties.InitWith(AppConstants.Captions.Properties, ApplicationIcons.Properties);
         splitContainerControl.PanelVisibility = SplitPanelVisibility.Panel1;
         layoutItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutItemPanelSource.Text = AppConstants.Captions.Source.FormatForLabel();
         layouytItemPanelTarget.Text = AppConstants.Captions.Target.FormatForLabel();
         htmlEditor.Properties.ShowIcon = false;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(dto => dto.Name)
            .To(btName)
            .OnValueUpdating += OnValueUpdating;

         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor)
            .OnValueUpdating += OnValueUpdating;

         var checkEditElementBinder = _screenBinder.Bind(dto => dto.CreateProcessRateParameter)
            .To(chkCreateParameter);
         checkEditElementBinder.OnValueUpdating += onCreateParameterSet;

         _screenBinder.Bind(dto => dto.ProcessRateParameterPersistable)
            .To(chkPlotParameter)
            .OnValueUpdating += onPlotParameterSet;

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);
      }

      private void onPlotParameterSet(TransportBuilderDTO dto, PropertyValueSetEventArgs<bool> e)
      {
         OnEvent(() => _presenter.SetPlotProcessRateParameter(e.NewValue));
      }

      private void onCreateParameterSet(TransportBuilderDTO dto, PropertyValueSetEventArgs<bool> e)
      {
         OnEvent(() => _presenter.SetCreateProcessRateParameter(e.NewValue));
         chkCreateParameter.Checked = dto.CreateProcessRateParameter;
      }

      public override bool HasError => base.HasError || _screenBinder.HasError;

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
         set => splitContainerControl.PanelVisibility = value ? SplitPanelVisibility.Both : SplitPanelVisibility.Panel1;
      }

      public void AddMoleculeSelectionView(IView view)
      {
         splitContainerControl.PanelVisibility = SplitPanelVisibility.Both;
         splitContainerControl.Panel2.FillWith(view);
      }

      public void SetFormulaView(IView view)
      {
         tabKinetic.FillWith(view);
      }

      public void SetParameterView(IView view)
      {
         tabParameters.FillWith(view);
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }

      public BarManager PopupBarManager => barManager;
   }
}