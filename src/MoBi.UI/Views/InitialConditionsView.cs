using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Extensions;
using MoBi.Presentation.Views;

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

      public override string NameColumnCaption => AppConstants.Captions.MoleculeName;

      public void HideIsPresentColumn() => _isPresentColumn.AsHidden().WithShowInColumnChooser(true);
      
      public void AddNegativeValuesNotAllowedSelectionView(IView view) => panelNegativeValuesNotAllowed.FillWith(view);
      
      public void AddIsNotPresentSelectionView(IView view)  => panelIsNotPresent.FillWith(view);
      
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

      public IBuildingBlockWithInitialConditionsPresenter InitialConditionPresenter => _presenter.DowncastTo<IBuildingBlockWithInitialConditionsPresenter>();

      public void AddRefreshSelectionView(IView view) => panelRefresh.FillWith(view);

      public void AddIsPresentSelectionView(IView view) => panelIsPresent.FillWith(view);

      public void AddNegativeValuesAllowedSelectionView(IView view) => panelNegativeValuesAllowed.FillWith(view);

      public void AttachPresenter(IBuildingBlockWithInitialConditionsPresenter presenter) => _presenter = presenter;

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.InitialConditions;
   }
}