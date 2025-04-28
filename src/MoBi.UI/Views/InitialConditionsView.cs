using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Binders;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class InitialConditionsView : BasePathAndValueEntityView<InitialConditionDTO, InitialCondition>, IInitialConditionsView
   {
      private readonly UxRepositoryItemCheckEdit _checkItemRepository;
      private IGridViewBoundColumn<InitialConditionDTO, bool> _isPresentColumn;

      public InitialConditionsView(ValueOriginBinder<InitialConditionDTO> valueOriginBinder) : base(valueOriginBinder)
      {
         InitializeComponent();
         _checkItemRepository = new UxRepositoryItemCheckEdit(gridView);
      }

      protected override void DoInitializeBinding()
      {
         base.DoInitializeBinding();
         _unitControl.ParameterUnitSet += setParameterUnit;

         BindValueColumn(dto => dto.Value)
            .WithCaption(AppConstants.Captions.Value)
            .WithFormat(dto => dto.InitialConditionFormatter())
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
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnRefresh.ItemClick += (o,e) => OnEvent(() => InitialConditionPresenter.Refresh(SelectedStartValues));
         btnPresent.ItemClick += (o,e) => OnEvent(() => InitialConditionPresenter.AsPresent(SelectedStartValues));
         btnNotPresent.ItemClick += (o,e) => OnEvent(() => InitialConditionPresenter.AsNotPresent(SelectedStartValues));
         btnAllowNegativeValues.ItemClick += (o,e) => OnEvent(() => InitialConditionPresenter.AllowNegativeValues(SelectedStartValues));
         btnNotAllowNegativeValues.ItemClick += (o,e) => OnEvent(() => InitialConditionPresenter.DoNotAllowNegativeValues(SelectedStartValues));
      }

      public override string NameColumnCaption => AppConstants.Captions.MoleculeName;

      private void onSetIsPresent(InitialConditionDTO dto, bool isPresent) => InitialConditionPresenter.SetIsPresent(dto, isPresent);

      private void onSetNegativeValueAllowed(InitialConditionDTO dto, bool negativeValuesAllowed) => InitialConditionPresenter.SetNegativeValuesAllowed(dto, negativeValuesAllowed);

      private void setParameterUnit(InitialConditionDTO initialCondition, Unit unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridView.CloseEditor();
            InitialConditionPresenter.SetUnit(initialCondition, unit);
         });
      }

      public override void HideElements(HidablePathAndValuesViewElement elementsToHide)
      {
         base.HideElements(elementsToHide);
         if (elementsToHide.IsSet(HidablePathAndValuesViewElement.IsPresentColumn))
            _isPresentColumn.AsHidden().WithShowInColumnChooser(true);
      }

      public void Select(InitialConditionDTO dto)
      {
         gridView.FocusedRowHandle = _gridViewBinder.RowHandleFor(dto);
         gridView.SelectRow(gridView.FocusedRowHandle);
      }

      public IBuildingBlockWithInitialConditionsPresenter InitialConditionPresenter => _presenter.DowncastTo<IBuildingBlockWithInitialConditionsPresenter>();

      public void AttachPresenter(IBuildingBlockWithInitialConditionsPresenter presenter) => _presenter = presenter;
   }
}