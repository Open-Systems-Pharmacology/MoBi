using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.UI.Extensions;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Helper;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;

namespace MoBi.UI.Views
{
   public partial class EditQuantityInSimulationView : BaseUserControl, IEditQuantityInSimulationView
   {
      private IEditQuantityInSimulationPresenter _presenter;
      private readonly EditBaseInfoView _baseEditView;
      private bool _readOnly;
      private readonly ScreenBinder<QuantityDTO> _screenBinder;

      public EditQuantityInSimulationView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<QuantityDTO>();
         _baseEditView = new EditBaseInfoView();
         tabInfo.Controls.Add(_baseEditView);
         _baseEditView.Dock = DockStyle.Fill;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(dto => dto.Value).To(valueEdit);
         valueEdit.ValueChanged += (o, valueInGuiUnit) => OnEvent(() => _presenter.SetValue(valueInGuiUnit));
         valueEdit.UnitChanged += (o, unit) => OnEvent(() => _presenter.SetDisplayUnit(unit));
         btnResetToFormulaValue.Click += (o, e) => OnEvent(() => _presenter.ResetValue());
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public void AttachPresenter(IEditQuantityInSimulationPresenter presenter)
      {
         _presenter = presenter;
         _baseEditView.AttachPresenter(presenter);
      }

      public void BindTo(QuantityDTO quantityDTO)
      {
         _baseEditView.BindToSource(quantityDTO);
         _screenBinder.BindToSource(quantityDTO);
      }

      public bool ReadOnly
      {
         get => _readOnly;
         set
         {
            _readOnly = value;
            _baseEditView.ReadOnly = _readOnly;
         }
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
         set { layoutControlItemValue.Text = value.FormatForLabel(); }
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

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemFormula.Text = ObjectTypes.Default.FormatForLabel();
         tabInfo.Text = AppConstants.Captions.Properties;
         tabValue.Text = AppConstants.Captions.InitialValue;
         btnResetToFormulaValue.InitWithImage(ApplicationIcons.Reset, text: AppConstants.Captions.Reset);
         layoutControlItemReset.AdjustButtonSize();
      }
   }
}