using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditSumFormulaView : BaseUserControl, IEditSumFormulaView
   {
      private IEditSumFormulaPresenter _presenter;
      private ScreenBinder<SumFormulaDTO> _screenBinder;
      private bool _readOnly;

      public EditSumFormulaView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemVariableName.Text = AppConstants.Captions.VariableName;
         layoutControlItemCriteria.Text = AppConstants.Captions.Criteria;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<SumFormulaDTO>();
         _screenBinder.Bind(dto => dto.Variable).To(txtVariableName)
            .OnValueUpdating += (dto, eventArgs) => _presenter.ChangeVariableName(eventArgs.NewValue, eventArgs.OldValue);

         _screenBinder.Bind(dto => dto.FormulaString).To(lblFormula);
      }

      public void AttachPresenter(IEditSumFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(SumFormulaDTO sumFormulaDTO)
      {
         _screenBinder.BindToSource(sumFormulaDTO);
      }

      public void AddDescriptorConditionListView(IView view)
      {
         panelCriteria.FillWith(view);
      }

      public bool ReadOnly
      {
         get { return _readOnly; }
         set
         {
            _readOnly = value;
            txtVariableName.Properties.ReadOnly = _readOnly;
            panelCriteria.Enabled = !value;
         }
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_ParametersSumFormulas;
   }
}