using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using static DevExpress.XtraLayout.Utils.LayoutVisibility;

namespace MoBi.UI.Views
{
   public partial class EditIndividualParameterView : BaseUserControl, IEditIndividualParameterView
   {
      private IEditIndividualParameterPresenter _presenter;
      private readonly ScreenBinder<IndividualParameterDTO> _screenBinder = new ScreenBinder<IndividualParameterDTO>();

      public EditIndividualParameterView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemValue.Text = Captions.Value.FormatForLabel();
         layoutControlItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutControlGroupProperties.Text = AppConstants.Captions.Properties;
         layoutControlGroupFormula.Text = AppConstants.Captions.Formula;
         layoutControlItemDimension.Text = AppConstants.Captions.Dimension.FormatForLabel();
         layoutControlItemUnits.TextVisible = false;
         layoutItemValueOrigin.AdjustControlHeight(layoutControlItemDimension.Control.Height);
         layoutItemValueOrigin.Text = Captions.ValueOrigin.FormatForLabel();
         layoutItemValueOrigin.TextVisible = true;
         btnCreateFormula.InitWithImage(ApplicationIcons.Add, AppConstants.Captions.CreateFormula);
         layoutControlItemCreateFormula.AdjustButtonSize(uxLayoutControl);
         lblWarning.AllowHtmlString = true;
         layoutControlGroupWarning.Text = AppConstants.Captions.Warning;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(dto => dto.Name)
            .To(textEditName);
         textEditName.Properties.ReadOnly = true;

         _screenBinder.Bind(dto => dto.Dimension)
            .To(textEditDimension);

         _screenBinder.Bind(dto => dto.DisplayUnit)
            .To(cbDisplayUnit)
            .WithValues(dto => dto.AllUnits)
            .OnValueUpdated += (o, e) => OnEvent(() => _presenter.UpdateUnit(e));

         _screenBinder.Bind(dto => dto.Value)
            .To(textEditValue)
            .OnValueUpdated += (o, e) => OnEvent(() => _presenter.UpdateValue(e));
         textEditValue.EnterMoveNextControl = true;

         textEditDimension.Properties.ReadOnly = true;

         btnCreateFormula.Click += (o, e) => OnEvent(() => _presenter.CreateConstantFormula());
      }

      public void AttachPresenter(IEditIndividualParameterPresenter presenter) => _presenter = presenter;

      public void BindTo(IndividualParameterDTO individualParameterDTO) => _screenBinder.BindToSource(individualParameterDTO);

      public void AddValueOriginView(IView view) => panelOriginView.FillWith(view);

      public void AddFormulaView(IView view) => panelFormula.FillWith(view);

      public void HideFormulaEdit()
      {
         formulaButtonVisibility(Always);
         layoutControlGroupFormula.Visibility = Never;
      }

      private void formulaButtonVisibility(LayoutVisibility layoutVisibility)
      {
         layoutControlItemCreateFormula.Visibility = layoutVisibility;
         emptySpaceItem.Visibility = layoutVisibility;
      }

      public void ShowFormulaEdit()
      {
         formulaButtonVisibility(Never);
         layoutControlGroupFormula.Visibility = Always;
      }

      public void ShowWarningFor(string buildingBlockName) => 
         lblWarning.Text = AppConstants.Warnings.EditsHereChangeTheIndividualBuildingblock(buildingBlockName).FormatForDescription();

      private void disposeBinders() => _screenBinder?.Dispose();
   }
}