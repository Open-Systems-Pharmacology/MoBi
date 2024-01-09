using System;
using System.Collections.Generic;
using System.ComponentModel;
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
   public abstract class PathAndValueBuildingBlockPresenter<TView, TPresenter, TBuildingBlock, TStartValueDTO, TPathAndValueEntity> :
      PathAndValueBuildingBlockPresenter<TView, TPresenter, Module, TBuildingBlock, TPathAndValueEntity, TStartValueDTO>, IStartValuesPresenter<TStartValueDTO>
      where TView : IView<TPresenter>, IPathAndValueEntitiesView<TStartValueDTO>
      where TPresenter : IPresenter
      where TPathAndValueEntity : PathAndValueEntity, IUsingFormula
      where TStartValueDTO : StartValueDTO<TPathAndValueEntity>
      where TBuildingBlock : class, IBuildingBlock<TPathAndValueEntity>
   {
      protected readonly IPathAndValueEntityToPathAndValueEntityDTOMapper<TPathAndValueEntity, TStartValueDTO> _valueMapper;

      private readonly IStartValuesTask<TBuildingBlock, TPathAndValueEntity> _startValuesTask;
      protected BindingList<TStartValueDTO> _startValueDTOs;
      private readonly IEmptyStartValueCreator<TPathAndValueEntity> _emptyStartValueCreator;
      protected readonly IMoBiContext _context;
      private bool _handleChangedEvents;
      private TPathAndValueEntity _focusedStartValue;
      public Func<TStartValueDTO, bool> IsOriginalStartValue { get; set; }
      private readonly List<TPathAndValueEntity> _originalStartValues;
      private readonly string _objectType;

      public bool IsLatched { get; set; }

      protected PathAndValueBuildingBlockPresenter(
         TView view,
         IPathAndValueEntityToPathAndValueEntityDTOMapper<TPathAndValueEntity, TStartValueDTO> valueMapper,
         IStartValuesTask<TBuildingBlock, TPathAndValueEntity> startValuesTask,
         IEmptyStartValueCreator<TPathAndValueEntity> emptyStartValueCreator,
         IMoBiContext context,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory)
         : base(view, startValuesTask, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _objectType = new ObjectTypeResolver().TypeFor<TPathAndValueEntity>();
         _startValuesTask = startValuesTask;
         _valueMapper = valueMapper;
         IsOriginalStartValue = isOriginalStartValue;
         _emptyStartValueCreator = emptyStartValueCreator;
         _context = context;
         _originalStartValues = new List<TPathAndValueEntity>();

         deleteStartValuePresenter.ApplySelectionAction = performDeleteAction;
         _view.AddDeleteStartValuesView(deleteStartValuePresenter.BaseView);

         AddSubPresenters(deleteStartValuePresenter);
         _handleChangedEvents = true;
         CanCreateNewFormula = true;
      }

      private bool isOriginalStartValue(TStartValueDTO pathAndValueEntity)
      {
         return _originalStartValues.Contains(pathAndValueEntity.PathWithValueObject);
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
            pathAndValueEntity: _emptyStartValueCreator.CreateEmptyStartValue(_startValuesTask.GetDefaultDimension()),
            buildingBlock: _buildingBlock
         ));
         bindToView();
      }

      public bool CanCreateNewFormula
      {
         set { _view.CanCreateNewFormula = value; }
      }

      protected IReadOnlyList<TStartValueDTO> SelectedStartValueDTOs => _view.SelectedStartValues;

      protected IEnumerable<TPathAndValueEntity> SelectedStartValues => StartValuesFrom(SelectedStartValueDTOs);

      protected IReadOnlyList<TStartValueDTO> VisibleStartValueDTOs => _view.VisibleStartValues;

      protected IEnumerable<TPathAndValueEntity> VisibleStartValues => StartValuesFrom(VisibleStartValueDTOs);

      protected IEnumerable<TPathAndValueEntity> StartValuesFrom(IEnumerable<TStartValueDTO> selectedStartValueDTOs)
      {
         return selectedStartValueDTOs.Select(x => x.PathWithValueObject);
      }

      private void performDeleteAction(SelectOption selectOption)
      {
         if (selectOption == SelectOption.DeleteSelected)
            deleteSelected();
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
         macroCommand.ObjectType = _objectType;
         macroCommand.Description = RemoveCommandDescription();

         AddCommand(macroCommand.Run(_context));

         _startValueDTOs.RemoveRange(startValuesToRemove);
      }

      protected abstract string RemoveCommandDescription();

      public void RemoveStartValue(TStartValueDTO elementToRemove)
      {
         bulkRemove(new List<TStartValueDTO> { elementToRemove });
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
         _startValueDTOs = buildingBlock.Select(pathAndValueEntity => _valueMapper.MapFrom(pathAndValueEntity, buildingBlock)).OrderBy(pathAndValueEntity => IsOriginalStartValue(pathAndValueEntity)).ToBindingList();
         bindToView();
         initializeColumns();
      }

      private void bindToView()
      {
         _view.BindTo(_startValueDTOs);
      }

      public void SetValue(TStartValueDTO startValueDTO, double? valueInDisplayUnit)
      {
         AddCommand(_startValuesTask.SetValue(_buildingBlock, valueInDisplayUnit, StartValueFrom(startValueDTO)));
      }

      private void collectRemoveCommands(TStartValueDTO elementToRemove, BulkUpdateMacroCommand collector)
      {
         var pathAndValueEntity = StartValueFrom(elementToRemove);
         if (_buildingBlock.Contains(pathAndValueEntity))
         {
            collector.AddCommand(_startValuesTask.RemovePathAndValueEntityFromBuildingBlockCommand(pathAndValueEntity, _buildingBlock));
         }
      }

      public void UpdateStartValueContainerPath(TStartValueDTO startValueDTO, int indexToUpdate, string newValue)
      {
         AddCommand(_startValuesTask.EditPathAndValueEntityContainerPath(_buildingBlock, StartValueFrom(startValueDTO), indexToUpdate, newValue));
      }

      public void UpdateStartValueName(TStartValueDTO startValueDTO, string newValue)
      {
         var pathAndValueEntity = StartValueFrom(startValueDTO);

         if (!_buildingBlock.Contains(pathAndValueEntity))
         {
            startValueDTO.UpdateName(newValue);
            AddCommand(_startValuesTask.AddPathAndValueEntityToBuildingBlock(_buildingBlock, pathAndValueEntity));
         }
         else
         {
            AddCommand(_startValuesTask.EditPathAndValueEntityName(_buildingBlock, pathAndValueEntity, newValue));
         }
      }

      protected TPathAndValueEntity StartValueFrom(TStartValueDTO startValueDTO)
      {
         return startValueDTO?.PathWithValueObject;
      }

      protected TStartValueDTO StartValueDTOFrom(TPathAndValueEntity pathAndValueEntity)
      {
         return pathAndValueEntity == null ? null : _startValueDTOs.FirstOrDefault(dto => Equals(StartValueFrom(dto), pathAndValueEntity));
      }

      public override object Subject => _buildingBlock;

      public override void AddCommand(Func<ICommand> commandAction)
      {
         this.DoWithinLatch(() => base.AddCommand(commandAction));
      }

      public override void SetFormula(TStartValueDTO startValueDTO, IFormula formula)
      {
         var pathAndValueEntity = StartValueFrom(startValueDTO);
         SetFormulaInBuilder(startValueDTO, formula, pathAndValueEntity);
      }

      public override void SetUnit(TStartValueDTO startValueDTO, Unit newUnit)
      {
         SetUnit(StartValueFrom(startValueDTO), newUnit);
      }

      public abstract override void AddNewFormula(TStartValueDTO startValueDTO);

      public bool HasAtLeastOneValue(int pathElementIndex)
      {
         return _startValueDTOs.HasAtLeastOneValue(pathElementIndex);
      }

      public void HandleBuildingBlockEvent(BuildingBlockEvent eventToHandle)
      {
         if (IsLatched) return;
         if (!Equals(_buildingBlock, eventToHandle.BuildingBlock))
            return;

         reBind(_buildingBlock);
      }

      public void Handle(PathAndValueEntitiesBuildingBlockChangedEvent eventToHandle)
      {
         if (_handleChangedEvents)
            HandleBuildingBlockEvent(eventToHandle);
      }

      public void HideValueOriginColumn()
      {
         _view.HideValueOriginColumn();
      }

      public void HideDeleteView()
      {
         _view.HideDeleteView();
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