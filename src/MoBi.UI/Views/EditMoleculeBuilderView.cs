using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using MoBi.UI.Services;
using OSPSuite.Assets;
using OSPSuite.Presentation;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditMoleculeBuilderView : BaseUserControl, IEditMoleculeBuilderView
   {
      private ScreenBinder<MoleculeBuilderDTO> _screenBinder;
      private IEditMoleculeBuilderPresenter _presenter;
      private GridViewBinder<UsedCalculationMethodDTO> _gridBinder;
      private readonly IToolTipCreator _toolTipCreator;

      public EditMoleculeBuilderView(IToolTipCreator toolTipCreator)
      {
         InitializeComponent();
         _toolTipCreator = toolTipCreator;
         _toolTipController.AllowHtmlText = true;
         _toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
         grdCalculationMethodsView.OptionsView.ShowGroupPanel = false;
         grdCalculationMethodsView.GridControl.ToolTipController = _toolTipController;
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != grdCalculationMethodsView.GridControl) return;

         var hi = grdCalculationMethodsView.CalcHitInfo(e.ControlMousePosition);
         if (hi.Column == null) return;
         if (!hi.InRowCell) return;

         var usedCalculationMethodDTO = _gridBinder.ElementAt(hi.RowHandle);
         if (usedCalculationMethodDTO == null) return;

         //check if subclass want to display a tool tip as well
         var superToolTip = _toolTipCreator.ToolTipFor(usedCalculationMethodDTO);
         if (superToolTip == null)
            return;

         //An object that uniquely identifies a row cell
         e.Info = new ToolTipControlInfo(usedCalculationMethodDTO, string.Empty) {SuperTip = superToolTip, ToolTipType = ToolTipType.SuperTip};
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<MoleculeBuilderDTO>();
         _screenBinder.Bind(dto => dto.Stationary).To(chkIsFloating).OnValueUpdating += onIsPresentValueSet;
         _screenBinder.Bind(dto => dto.Name).To(btName).OnValueUpdating += OnValueUpdating;
         _screenBinder.Bind(dto => dto.Description).To(htmlEditor).OnValueUpdating += OnValueUpdating;
         _screenBinder.Bind(dto => dto.MoleculeType).To(cbMoleculeType)
            .WithValues(dto => _presenter.GetMoleculeTypes())
            .OnValueUpdating += (builder, args) => _presenter.SetMoleculeType(args.NewValue, args.OldValue);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         _gridBinder = new GridViewBinder<UsedCalculationMethodDTO>(grdCalculationMethodsView);
         _gridBinder.Bind(dto => dto.Category).AsReadOnly();
         _gridBinder.Bind(dto => dto.CalculationMethodName)
            .WithCaption(AppConstants.Captions.CalculationMethod)
            .WithEditRepository(getComboboxRepositoryItem)
            .OnValueUpdating += onCalculationMethodChanged;

         btName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         chkIsFloating.ToolTip = ToolTips.Molecules.IsStationary;
         chkIsFloating.Text = AppConstants.Captions.IsStationary;
         UpdateStartAmountDisplay(AppConstants.Captions.DefaultStartAmount);
         layoutControlItemFormula.TextVisible = false;
         layoutControlItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         btName.ToolTip = ToolTips.Molecules.MoleculeName;
         layoutControlItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         htmlEditor.ToolTip = ToolTips.Description;
         htmlEditor.Properties.ShowIcon = false;
         layoutControlItemMoleculeType.Text = AppConstants.Captions.MoleculeType.FormatForLabel();
         layoutControlItemCalculationMethod.Text = AppConstants.Captions.UsedCalculationMethods;
         cbMoleculeType.ToolTip = ToolTips.Molecules.MoleculeType;
         var size = layoutControlItemCalculationMethod.Size;
         size.Height = grdCalculationMethodsView.ColumnPanelRowHeight + 26 * 3;
         layoutControlItemCalculationMethod.Size = size;
         tabProperties.InitWith(AppConstants.Captions.Properties, ApplicationIcons.Properties);
         tabParameters.InitWith(AppConstants.Captions.Parameters, ApplicationIcons.Parameter);

      }

      public void UpdateStartAmountDisplay(string amountOrConcentrationText)
      {
         grpFormula.Text = amountOrConcentrationText;
      }

      public void Activate()
      {
         ActiveControl = btName;
      }

      private void onCalculationMethodChanged(UsedCalculationMethodDTO dto, PropertyValueSetEventArgs<string> e)
      {
         this.DoWithinExceptionHandler(() => _presenter.SetCalculationMethodForCategory(dto.Category, e.NewValue, e.OldValue));
      }

      private RepositoryItem getComboboxRepositoryItem(UsedCalculationMethodDTO dto)
      {
         var comboBox = new UxRepositoryItemComboBox(grdCalculationMethodsView);
         comboBox.FillComboBoxRepositoryWith(_presenter.GetCalculationMethodsForCategory(dto.Category));
         return comboBox;
      }

      private void onIsPresentValueSet(MoleculeBuilderDTO moleculeBuilder, PropertyValueSetEventArgs<bool> value)
      {
         _presenter.SetStationaryProperty(value.NewValue, value.OldValue);
      }

      private void OnValueUpdating<T>(MoleculeBuilderDTO moleculeBuilder, PropertyValueSetEventArgs<T> value)
      {
         _screenBinder.Validate();
         this.DoWithinExceptionHandler(() => _presenter.SetPropertyValueFromView(value.PropertyName, value.NewValue, value.OldValue));
      }

      public void SetFormulaView(IView subView)
      {
         grpFormula.FillWith(subView);
      }

      public void SetParametersView(IView subView)
      {
         tabParameters.FillWith(subView);
      }

      public void Show(MoleculeBuilderDTO moleculeBuilder)
      {
         _screenBinder.BindToSource(moleculeBuilder);
         _gridBinder.BindToSource(moleculeBuilder.UsedCalculationMethods);
         initNameEdit(moleculeBuilder);
         var size = layoutControlItemCalculationMethod.Size;
         size.Height = grdCalculationMethodsView.ColumnPanelRowHeight + 3 * 24;
         layoutControlItemCalculationMethod.MaxSize = size;
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }

      private void initNameEdit(MoleculeBuilderDTO moleculeBuilder)
      {
         var name = moleculeBuilder.Name;
         var nameIsSet = name.IsNullOrEmpty();
         btName.Properties.ReadOnly = !nameIsSet;
         btName.Properties.Buttons[0].Visible = !nameIsSet;
      }

      public void AttachPresenter(IEditMoleculeBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public override bool HasError => base.HasError || _screenBinder.HasError || _gridBinder.HasError;
   }
}