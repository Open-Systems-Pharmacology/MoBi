﻿using OSPSuite.DataBinding.DevExpress;
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
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
   public partial class InitialConditionsView : BasePathAndValueEntityView<InitialConditionDTO, InitialCondition>, IInitialConditionsView
   {
      private readonly UxComboBoxUnit<InitialConditionDTO> _unitControl;
      private readonly UxRepositoryItemCheckEdit _checkItemRepository;
      private IGridViewBoundColumn<InitialConditionDTO, bool> _isPresentColumn;
      
      public InitialConditionsView(ValueOriginBinder<InitialConditionDTO> valueOriginBinder) : base(valueOriginBinder)
      {
         InitializeComponent();
         _unitControl = new UxComboBoxUnit<InitialConditionDTO>(gridControl);
         _checkItemRepository = new UxRepositoryItemCheckEdit(gridView);
      }

      protected override void DoInitializeBinding()
      {
         base.DoInitializeBinding();
         _unitControl.ParameterUnitSet += setParameterUnit;

         _gridViewBinder.AutoBind(dto => dto.Value)
            .WithCaption(AppConstants.Captions.Value)
            .WithFormat(dto => dto.InitialConditionFormatter())
            .WithEditorConfiguration(configureRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithOnValueUpdating((o, e) => OnEvent(() => InitialConditionPresenter.SetValue(o, e.NewValue)));

         InitializeValueOriginBinding();

         _gridViewBinder.AutoBind(dto => dto.ScaleDivisor)
            .WithCaption(AppConstants.Captions.ScaleDivisor)
            .WithOnValueUpdating((o, e) => OnEvent(() => InitialConditionPresenter.SetScaleDivisor(o, e.NewValue)));

         _isPresentColumn = _gridViewBinder.Bind(dto => dto.IsPresent)
            .WithCaption(AppConstants.Captions.IsPresent)
            .WithRepository(dto => _checkItemRepository)
            .WithOnValueUpdating((o, e) => OnEvent(() => onSetIsPresent(o, e.NewValue)));

         _gridViewBinder.Bind(dto => dto.NegativeValuesAllowed)
            .WithCaption(AppConstants.Captions.NegativeValuesAllowed)
            .WithRepository(dto => _checkItemRepository)
            .WithOnValueUpdating((o, e) => OnEvent(() => onSetNegativeValueAllowed(o, e.NewValue)));


         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => InitialConditionPresenter.SetFormula(o, e.NewValue.Formula));

         gridView.HiddenEditor += (o, e) => hideEditor();
      }

      public void HideIsPresentColumn()
      {
         _isPresentColumn.AsHidden().WithShowInColumnChooser(true);
      }

      private void onSetIsPresent(InitialConditionDTO dto, bool isPresent)
      {
         InitialConditionPresenter.SetIsPresent(dto, isPresent);
      }

      private void onSetNegativeValueAllowed(InitialConditionDTO dto, bool negativeValuesAllowed)
      {
         InitialConditionPresenter.SetNegativeValuesAllowed(dto, negativeValuesAllowed);
      }

      private void setParameterUnit(InitialConditionDTO initialCondition, Unit unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridView.CloseEditor();
            InitialConditionPresenter.SetUnit(initialCondition, unit);
         });
      }

      public IBuildingBlockWithInitialConditionsPresenter InitialConditionPresenter => _presenter.DowncastTo<IBuildingBlockWithInitialConditionsPresenter>();

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      private void configureRepository(BaseEdit activeEditor, InitialConditionDTO initialCondition)
      {
         _unitControl.UpdateUnitsFor(activeEditor, initialCondition);
      }

      public void AddIsPresentSelectionView(IView view)
      {
         panelIsPresent.FillWith(view);
      }

      public void AddNegativeValuesAllowedSelectionView(IView view)
      {
         panelNegativeValuesAllowed.FillWith(view);
      }

      public void AttachPresenter(IBuildingBlockWithInitialConditionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.InitialConditions;
   }
}