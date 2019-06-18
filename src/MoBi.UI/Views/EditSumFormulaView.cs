using System;
using DevExpress.XtraEditors.DXErrorProvider;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditSumFormulaView : BaseUserControl, IEditSumFormulaView
   {
      private IEditSumFormulaPresenter _presenter;
      private readonly ScreenBinder<SumFormulaDTO> _screenBinder;
      private bool _readOnly;
      private readonly DXErrorProvider _warningProvider;

      public EditSumFormulaView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<SumFormulaDTO>();
         _warningProvider = new DXErrorProvider(this);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemVariableName.Text = AppConstants.Captions.VariableName.FormatForLabel();
         layoutControlItemCriteria.Text = AppConstants.Captions.Criteria.FormatForLabel();
         layoutItemFormulaString.Text = AppConstants.Captions.SumOfAll;
         lblDescription.AsDescription();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(dto => dto.Variable)
            .To(txtVariableName)
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.ChangeVariableName(e.NewValue));

         txtFormulaString.TextChanged += (o, e) => OnEvent(formulaStringChanging, e);

         _screenBinder.Bind(item => item.FormulaString)
            .To(txtFormulaString)
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.SetFormulaString(e.NewValue));
      }

      private async void formulaStringChanging(EventArgs e)
      {
         await txtFormulaString.Debounce(formulaStringChanged);
      }

      private void formulaStringChanged()
      {
         _presenter.Validate(txtFormulaString.Text);
      }

      public void AttachPresenter(IEditSumFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(SumFormulaDTO sumFormulaDTO)
      {
         _screenBinder.BindToSource(sumFormulaDTO);
         lblDescription.Text = AppConstants.Captions.SumFormulaDescription(sumFormulaDTO.VariablePattern).FormatForDescription();

      }

      public void AddDescriptorConditionListView(IView view)
      {
         panelCriteria.FillWith(view);
      }

      public void AddFormulaPathListView(IView view)
      {
         panelFormulaUsablePath.FillWith(view);
      }

      public void SetValidationMessage(string parserError)
      {
         if (string.IsNullOrEmpty(parserError))
            _warningProvider.SetError(txtFormulaString, null);
         else
            _warningProvider.SetError(txtFormulaString, parserError, ErrorType.Critical);

         _presenter.ViewChanged();
      }

      public bool ReadOnly
      {
         get => _readOnly;
         set
         {
            _readOnly = value;
            txtVariableName.Properties.ReadOnly = _readOnly;
            panelCriteria.Enabled = !value;
         }
      }
   }
}