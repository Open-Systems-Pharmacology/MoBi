using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public abstract class StartValuePresenter<TView, TPresenter, TBuildingBlock, TStartValueDTO, TStartValue> :
      PathWithValueBuildingBlockPresenter<TView, TPresenter, Module, TBuildingBlock, TStartValue, TStartValueDTO>, IStartValuesPresenter<TStartValueDTO>
      where TView : IView<TPresenter>, IStartValuesView<TStartValueDTO>
      where TPresenter : IPresenter
      where TStartValue : PathAndValueEntity, IUsingFormula
      where TStartValueDTO : StartValueDTO<TStartValue>
      where TBuildingBlock : PathAndValueEntityBuildingBlock<TStartValue>, IBuildingBlock<TStartValue>
   {
      protected readonly IStartValueToStartValueDTOMapper<TStartValue, TStartValueDTO> _valueMapper;

      private readonly IStartValuesTask<TBuildingBlock, TStartValue> _startValuesTask;
      protected BindingList<TStartValueDTO> _startValueDTOs;
      private readonly IEmptyStartValueCreator<TStartValue> _emptyStartValueCreator;
      protected readonly IMoBiContext _context;
      private bool _handleChangedEvents;
      protected ILegendPresenter _legendPresenter;
      private TStartValue _focusedStartValue;
      public Func<TStartValueDTO, Color> BackgroundColorRetriever { get; set; }
      public Func<TStartValueDTO, bool> IsOriginalStartValue { get; set; }
      private readonly List<TStartValue> _originalStartValues;

      public bool IsLatched { get; set; }

      protected StartValuePresenter(
         TView view,
         IStartValueToStartValueDTOMapper<TStartValue, TStartValueDTO> valueMapper,
         IRefreshStartValueFromOriginalBuildingBlockPresenter refreshStartValuesPresenter,
         IStartValuesTask<TBuildingBlock, TStartValue> startValuesTask,
         IEmptyStartValueCreator<TStartValue> emptyStartValueCreator,
         IMoBiContext context,
         ILegendPresenter legendPresenter,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory)
         : base(view, startValuesTask, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _startValuesTask = startValuesTask;
         _valueMapper = valueMapper;
         BackgroundColorRetriever = retrieveBackgroundColor;
         IsOriginalStartValue = isOriginalStartValue;
         _emptyStartValueCreator = emptyStartValueCreator;
         _context = context;
         _legendPresenter = legendPresenter;
         _originalStartValues = new List<TStartValue>();

         refreshStartValuesPresenter.ApplySelectionAction = performRefreshAction;
         deleteStartValuePresenter.ApplySelectionAction = performDeleteAction;
         _view.AddRefreshStartValuesView(refreshStartValuesPresenter.BaseView);
         _view.AddDeleteStartValuesView(deleteStartValuePresenter.BaseView);

         AddSubPresenters(legendPresenter, deleteStartValuePresenter, refreshStartValuesPresenter);
         _handleChangedEvents = true;
         CanCreateNewFormula = true;

         initializeLegend();
      }

      private void initializeLegend()
      {
         _legendPresenter.AddLegendItems(new[]
         {
            new LegendItemDTO { Description = AppConstants.Captions.CouldNotResolveSource, Color = MoBiColors.CannotResolve },
            new LegendItemDTO { Description = AppConstants.Captions.StartValueIsModified, Color = MoBiColors.Modified },
            new LegendItemDTO { Description = AppConstants.Captions.NewlyAddedValues, Color = MoBiColors.Extended }
         });

         _view.AddLegendView(_legendPresenter.View);
      }

      private Color retrieveBackgroundColor(TStartValueDTO startValueDTO)
      {
         if (!IsOriginalStartValue(startValueDTO))
            return MoBiColors.Extended;


         var startValue = startValueDTO.PathWithValueObject;
         if (!_startValuesTask.CanResolve(_buildingBlock, startValue))
            return MoBiColors.CannotResolve;

         return _startValuesTask.IsEquivalentToOriginal(startValue, _buildingBlock) ? MoBiColors.Default : MoBiColors.Modified;
      }

      private bool isOriginalStartValue(TStartValueDTO startValue)
      {
         return _originalStartValues.Contains(startValue.PathWithValueObject);
      }

      public void ExtendStartValues()
      {
         _startValuesTask.ExtendStartValues(_buildingBlock);
      }

      private void initializeColumns()
      {
         _view.ClearPathItems();
         _view.AddPathItems(_startValuesTask.GetContainerPathItemsForBuildingBlock(_buildingBlock).OrderBy(x => x));
         _view.InitializePathColumns();
      }

      public void AddNewEmptyStartValue()
      {
         _startValueDTOs.Insert(0, _valueMapper.MapFrom(
            startValue: _emptyStartValueCreator.CreateEmptyStartValue(_startValuesTask.GetDefaultDimension()),
            buildingBlock: _buildingBlock
         ));
         bindToView();
      }

      public void OnlyShowFilterSelection()
      {
         HideIsPresentView();
         HideNegativeValuesAllowedView();
         hideDeleteView();
         HideLegend();
         HideDeleteColumn();
         HideRefreshStartValuesView();
         _view.HideSubPresenterGrouping();
      }

      public bool CanCreateNewFormula
      {
         set { _view.CanCreateNewFormula = value; }
      }

      private void hideDeleteView()
      {
         _view.HideDeleteView();
      }

      public void HideRefreshStartValuesView()
      {
         _view.HideRefreshStartValuesView();
      }

      public void HideLegend()
      {
         _view.HideLegend();
      }

      protected IReadOnlyList<TStartValueDTO> SelectedStartValueDTOs => _view.SelectedStartValues;

      protected IEnumerable<TStartValue> SelectedStartValues => StartValuesFrom(SelectedStartValueDTOs);

      protected IReadOnlyList<TStartValueDTO> VisibleStartValueDTOs => _view.VisibleStartValues;

      protected IEnumerable<TStartValue> VisibleStartValues => StartValuesFrom(VisibleStartValueDTOs);

      protected IEnumerable<TStartValue> StartValuesFrom(IEnumerable<TStartValueDTO> selectedStartValueDTOs)
      {
         return selectedStartValueDTOs.Select(x => x.PathWithValueObject);
      }

      private void refreshStartValues(IEnumerable<TStartValue> startValues)
      {
         AddCommand(
            _startValuesTask.RefreshStartValuesFromBuildingBlocks(
               _buildingBlock,
               startValues.ToList()));
      }

      private void performDeleteAction(SelectOption selectOption)
      {
         if (selectOption == SelectOption.DeleteSelected)
            deleteSelected();
         else if (selectOption == SelectOption.DeleteSourceNotDefined)
            deleteUnresolved();
      }

      private void deleteSelected()
      {
         bulkRemove(_view.SelectedStartValues);
      }

      private void bulkRemove(IReadOnlyList<TStartValueDTO> startValuesToRemove)
      {
         var macroCommand = new BulkUpdateMacroCommand();

         startValuesToRemove.Each(value => collectRemoveCommands(value, macroCommand));

         macroCommand.CommandType = AppConstants.Commands.DeleteCommand;
         macroCommand.ObjectType = new ObjectTypeResolver().TypeFor<TStartValue>();
         macroCommand.Description = AppConstants.Commands.RemoveMultipleStartValues;

         AddCommand(macroCommand.Run(_context));

         _startValueDTOs.RemoveRange(startValuesToRemove);
      }

      public void RemoveStartValue(TStartValueDTO elementToRemove)
      {
         bulkRemove(new List<TStartValueDTO> { elementToRemove });
      }

      private void deleteUnresolved()
      {
         bulkRemove(_startValueDTOs.Where(dto => !_startValuesTask.CanResolve(_buildingBlock, dto.PathWithValueObject)).ToList());
      }

      private void performRefreshAction(SelectOption option)
      {
         if (option == SelectOption.RefreshSelected)
         {
            refreshStartValues(SelectedStartValues);
         }
         else if (option == SelectOption.RefreshAll)
         {
            refreshStartValues(VisibleStartValues);
         }
      }

      public override void Edit(TBuildingBlock buildingBlock)
      {
         _buildingBlock = buildingBlock;
         _originalStartValues.Clear();

         // Edit null building block happens when creating a simulation
         // and no Start Value Building Block exists
         if (_buildingBlock != null)
            _originalStartValues.AddRange(_buildingBlock);

         reBind(buildingBlock);
      }

      private void reBind(TBuildingBlock buildingBlock)
      {
         if (buildingBlock == null) return;
         _startValueDTOs = buildingBlock.Select(startValue => _valueMapper.MapFrom(startValue, buildingBlock)).OrderBy(startValue => IsOriginalStartValue(startValue)).ToBindingList();
         bindToView();
         initializeColumns();
      }

      private void bindToView()
      {
         _view.BindTo(_startValueDTOs);
      }

      public void SetValueOrigin(TStartValueDTO startValueDTO, ValueOrigin newValueOrigin)
      {
         AddCommand(_startValuesTask.SetValueOrigin(_buildingBlock, newValueOrigin, StartValueFrom(startValueDTO)));
      }

      public void SetValue(TStartValueDTO startValueDTO, double? valueInDisplayUnit)
      {
         AddCommand(_startValuesTask.SetValue(_buildingBlock, valueInDisplayUnit, StartValueFrom(startValueDTO)));
      }

      public Color BackgroundColorFor(TStartValueDTO startValueDTO)
      {
         return BackgroundColorRetriever(startValueDTO);
      }

      private void collectRemoveCommands(TStartValueDTO elementToRemove, BulkUpdateMacroCommand collector)
      {
         var startValue = StartValueFrom(elementToRemove);
         if (_buildingBlock.Contains(startValue))
         {
            collector.AddCommand(_startValuesTask.RemoveStartValueFromBuildingBlockCommand(startValue, _buildingBlock));
         }
      }

      public void UpdateStartValueContainerPath(TStartValueDTO startValueDTO, int indexToUpdate, string newValue)
      {
         AddCommand(_startValuesTask.EditStartValueContainerPath(_buildingBlock, StartValueFrom(startValueDTO), indexToUpdate, newValue));
      }

      public void UpdateStartValueName(TStartValueDTO startValueDTO, string newValue)
      {
         var startValue = StartValueFrom(startValueDTO);

         if (!_buildingBlock.Contains(startValue))
         {
            startValueDTO.UpdateStartValueName(newValue);
            AddCommand(_startValuesTask.AddStartValueToBuildingBlock(_buildingBlock, startValue));
         }
         else
         {
            AddCommand(_startValuesTask.EditStartValueName(_buildingBlock, startValue, newValue));
         }
      }

      public bool ShouldShow(TStartValueDTO startValue)
      {
         return shouldShowForIsNew(startValue) && shouldShowForIsModified(startValue);
      }

      private bool shouldShowForIsModified(TStartValueDTO startValue)
      {
         return !IsModifiedFilterOn || !_startValuesTask.IsEquivalentToOriginal(StartValueFrom(startValue), _buildingBlock);
      }

      private bool shouldShowForIsNew(TStartValueDTO startValue)
      {
         return !IsNewFilterOn || !IsOriginalStartValue(startValue);
      }

      public bool IsColorDefault(Color color)
      {
         return (color == MoBiColors.Default);
      }

      public bool IsNewFilterOn { get; set; }

      public bool IsModifiedFilterOn { get; set; }

      protected TStartValue StartValueFrom(TStartValueDTO startValueDTO)
      {
         return startValueDTO?.PathWithValueObject;
      }

      protected TStartValueDTO StartValueDTOFrom(TStartValue startValue)
      {
         return startValue == null ? null : _startValueDTOs.FirstOrDefault(dto => Equals(StartValueFrom(dto), startValue));
      }

      public override object Subject => _buildingBlock;

      public override void AddCommand(Func<ICommand> commandAction)
      {
         this.DoWithinLatch(() => base.AddCommand(commandAction));
      }

      public override void SetFormula(TStartValueDTO startValueDTO, IFormula formula)
      {
         var startValue = StartValueFrom(startValueDTO);
         SetFormulaInBuilder(startValueDTO, formula, startValue);
      }

      public override void SetUnit(TStartValueDTO startValueDTO, Unit newUnit)
      {
         SetUnit(StartValueFrom(startValueDTO), newUnit);
      }

      public abstract override void AddNewFormula(TStartValueDTO startValueDTO);

      public bool HasAtLeastTwoDistinctValues(int pathElementIndex)
      {
         return _startValueDTOs.HasAtLeastTwoDistinctValues(pathElementIndex);
      }

      public void HandleBuildingBlockEvent(BuildingBlockEvent eventToHandle)
      {
         if (IsLatched) return;
         if (!Equals(_buildingBlock, eventToHandle.BuildingBlock))
            return;

         reBind(_buildingBlock);
      }

      public void Handle(StartValuesBuildingBlockChangedEvent eventToHandle)
      {
         if (_handleChangedEvents)
            HandleBuildingBlockEvent(eventToHandle);
      }

      public void HideDeleteColumn()
      {
         _view.HideDeleteColumn();
      }

      public void HideIsPresentView()
      {
         _view.HideIsPresentView();
      }

      public void HideNegativeValuesAllowedView()
      {
         _view.HideNegativeValuesAllowedView();
      }

      public void Handle(BulkUpdateFinishedEvent eventToHandle)
      {
         _handleChangedEvents = true;
         // Always refresh once the batch update is finished
         reBind(_buildingBlock);
         _view.FocusedStartValue = StartValueDTOFrom(_focusedStartValue);
      }

      public void Handle(BulkUpdateStartedEvent eventToHandle)
      {
         _focusedStartValue = StartValueFrom(_view.FocusedStartValue);
         _handleChangedEvents = false;
      }
   }
}