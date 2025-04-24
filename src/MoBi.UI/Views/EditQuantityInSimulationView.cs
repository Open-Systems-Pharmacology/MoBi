using OSPSuite.DataBinding;
using OSPSuite.UI.Extensions;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using MoBi.UI.Helper;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.DataBinding.DevExpress;

namespace MoBi.UI.Views
{
   public partial class EditQuantityInSimulationView : BaseUserControl, IEditQuantityInSimulationView
   {
      private IEditQuantityInSimulationPresenter _presenter;
      private readonly ScreenBinder<QuantityDTO> _screenBinder;

      public EditQuantityInSimulationView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<QuantityDTO>();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemFormula.Text = ObjectTypes.Default.FormatForLabel();
         tabProperties.InitWith(AppConstants.Captions.Properties, ApplicationIcons.Properties);
         tabValue.InitWith(AppConstants.Captions.InitialValue, ApplicationIcons.Formula);
         btnResetToFormulaValue.InitWithImage(ApplicationIcons.Reset, text: AppConstants.Captions.Reset);
         sourceTextEdit.ReadOnly = true;
         btnGoToSource.InitWithImage(ApplicationIcons.Search, AppConstants.Captions.GoToSource);
         layoutControlItemSource.Text = AppConstants.Captions.Source.FormatForLabel();
         layoutControlItemGoToSource.AdjustControlSize(OSPSuite.UI.UIConstants.Size.BUTTON_WIDTH, layoutControlItemGoToSource.ControlMaxSize.Height);
         layoutControlItemReset.AdjustControlSize(OSPSuite.UI.UIConstants.Size.BUTTON_WIDTH, layoutControlItemGoToSource.ControlMaxSize.Height);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(dto => dto.Value).To(valueEdit);

         _screenBinder.Bind(dto => dto.SourceDisplayName).To(sourceTextEdit);
         valueEdit.ValueChanged += (o, valueInGuiUnit) => OnEvent(() => _presenter.SetValue(valueInGuiUnit));
         valueEdit.UnitChanged += (o, unit) => OnEvent(() => _presenter.SetDisplayUnit(unit));
         btnResetToFormulaValue.Click += (o, e) => OnEvent(() => _presenter.ResetValue());
         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btnGoToSource.Click += (o, e) => OnEvent(_presenter.NavigateToSource);
      }

      public void AttachPresenter(IEditQuantityInSimulationPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(QuantityDTO quantityDTO)
      {
         _screenBinder.BindToSource(quantityDTO);

         var visibility = LayoutVisibilityConvertor.FromBoolean(quantityDTO.SourceReference != null);
         layoutControlItemSource.Visibility = visibility;
         layoutControlItemGoToSource.Visibility = visibility;

      }

      public bool AllowValueChange
      {
         get => LayoutVisibilityConvertor.ToBoolean(layoutControlItemValue.Visibility);
         set
         {
            layoutControlItemValue.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            layoutControlItemReset.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
         }
      }

      public string SetInitialValueLabel
      {
         set => layoutControlItemValue.Text = value.FormatForLabel();
      }

      public void SetFormulaView(IView view)
      {
         pnlFormula.FillWith(view);
      }

      public void SetParametersView(IView view)
      {
         tabParameters.Controls.Clear();
         tabParameters.FillWith(view);
         tabControl.TabPages.Add(tabParameters);
      }

      public void HideParametersView()
      {
         tabParameters.Visible = false;
         tabControl.TabPages.Remove(tabParameters);
      }

      public void SetWarning(string message)
      {
         valueEdit.SetWarning(message);
      }

      public void ClearWarning()
      {
         SetWarning(string.Empty);
      }

      public void EnableResetButton(bool enable)
      {
         btnResetToFormulaValue.Enabled = enable;
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }

      public void SetQuantityInfoView(IView view)
      {
         tabProperties.FillWith(view);
      }
   }
}