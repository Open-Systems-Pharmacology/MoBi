using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class MoleculeStartValuesView : BaseStartValuesView<MoleculeStartValueDTO>, IMoleculeStartValuesView
   {
      private readonly UxComboBoxUnit<MoleculeStartValueDTO> _unitControl;
      private readonly UxRepositoryItemCheckEdit _checkItemRepository;

      public MoleculeStartValuesView(ValueOriginBinder<MoleculeStartValueDTO> valueOriginBinder):base(valueOriginBinder)
      {
         InitializeComponent();
         _unitControl = new UxComboBoxUnit<MoleculeStartValueDTO>(gridControl);
         _checkItemRepository = new UxRepositoryItemCheckEdit(gridView);
      }

      protected override void DoInitializeBinding()
      {
         _unitControl.ParameterUnitSet += setParameterUnit;

         var colName = _gridViewBinder.AutoBind(dto => dto.Name)
            .WithCaption(AppConstants.Captions.MoleculeName)
            .WithOnValueUpdating((o, e) => OnEvent(() => OnNameSet(o, e)));

         //to put the name in the first column
         colName.XtraColumn.VisibleIndex = 0;

         _gridViewBinder.AutoBind(dto => dto.StartValue)
            .WithCaption(AppConstants.Captions.StartValue)
            .WithFormat(dto => dto.MoleculeStartValueFormatter())
            .WithEditorConfiguration(configureRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithOnValueUpdating((o, e) => OnEvent(() => moleculeStartValuePresenter.SetValue(o, e.NewValue)));

         InitializeValueOriginBinding();

         _gridViewBinder.AutoBind(dto => dto.ScaleDivisor)
            .WithCaption(AppConstants.Captions.ScaleDivisor)
            .WithOnValueUpdating((o, e) => OnEvent(() => moleculeStartValuePresenter.SetScaleDivisor(o, e.NewValue)));

         _gridViewBinder.Bind(dto => dto.IsPresent)
            .WithCaption(AppConstants.Captions.IsPresent)
            .WithRepository(dto => _checkItemRepository)
            .WithOnValueUpdating((o, e) => OnEvent(() => onSetIsPresent(o, e.NewValue)));

         _gridViewBinder.Bind(dto => dto.NegativeValuesAllowed)
            .WithCaption(AppConstants.Captions.NegativeValuesAllowed)
            .WithRepository(dto => _checkItemRepository)
            .WithOnValueUpdating((o, e) => OnEvent(() => onSetNegativeValueAllowed(o, e.NewValue)));


         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => moleculeStartValuePresenter.SetFormula(o, e.NewValue.Formula));

         gridView.HiddenEditor += (o, e) => hideEditor();
      }

      private void onSetIsPresent(MoleculeStartValueDTO dto, bool isPresent)
      {
         moleculeStartValuePresenter.SetIsPresent(dto, isPresent);
      }

      private void onSetNegativeValueAllowed(MoleculeStartValueDTO dto, bool negativeValuesAllowed)
      {
         moleculeStartValuePresenter.SetNegativeValuesAllowed(dto, negativeValuesAllowed);
      }

      private void setParameterUnit(MoleculeStartValueDTO moleculeStartValue, Unit unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridView.CloseEditor();
            moleculeStartValuePresenter.SetUnit(moleculeStartValue, unit);
         });
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      private void configureRepository(BaseEdit activeEditor, MoleculeStartValueDTO moleculeStartValue)
      {
         _unitControl.UpdateUnitsFor(activeEditor, moleculeStartValue);
      }

      public void AttachPresenter(IMoleculeStartValuesPresenter presenter)
      {
         _presenter = presenter;
      }

      private IMoleculeStartValuesPresenter moleculeStartValuePresenter
      {
         get { return _presenter.DowncastTo<IMoleculeStartValuesPresenter>(); }
      }

      public void AddIsPresentSelectionView(IView view)
      {
         panelIsPresent.FillWith(view);
      }

      public void AddNegativeValuesAllowedSelectionView(IView view)
      {
         panelNegativeValuesAllowed.FillWith(view);
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return ApplicationIcons.MoleculeStartValues; }
      }
   }
}