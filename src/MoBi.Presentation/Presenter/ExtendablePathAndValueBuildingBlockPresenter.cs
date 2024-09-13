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
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public abstract class ExtendablePathAndValueBuildingBlockPresenter<TView, TPresenter, TBuildingBlock, TStartValueDTO, TPathAndValueEntity> :
      PathAndValueBuildingBlockPresenter<TView, TPresenter, TBuildingBlock, TPathAndValueEntity, TStartValueDTO>, IExtendablePathAndValueBuildingBlockPresenter<TStartValueDTO>
      where TView : IView<TPresenter>, IPathAndValueEntitiesView<TStartValueDTO>
      where TPresenter : IPresenter
      where TPathAndValueEntity : PathAndValueEntity, IUsingFormula
      where TStartValueDTO : ExtendablePathAndValueEntityDTO<TPathAndValueEntity, TStartValueDTO>
      where TBuildingBlock : class, IBuildingBlock<TPathAndValueEntity>
   {
      protected readonly IPathAndValueEntityToPathAndValueEntityDTOMapper<TPathAndValueEntity, TStartValueDTO> _valueMapper;

      private readonly IInteractionTasksForExtendablePathAndValueEntity<TBuildingBlock, TPathAndValueEntity> _interactionTasksForExtendablePathAndValueEntity;
      protected BindingList<TStartValueDTO> _startValueDTOs;
      private readonly IEmptyStartValueCreator<TPathAndValueEntity> _emptyStartValueCreator;
      protected readonly IMoBiContext _context;
      private bool _handleChangedEvents;
      private TPathAndValueEntity _focusedStartValue;
      public Func<TStartValueDTO, bool> IsOriginalStartValue { get; set; }
      private readonly List<TPathAndValueEntity> _originalStartValues;
      private readonly string _objectType;

      public bool IsLatched { get; set; }

      protected ExtendablePathAndValueBuildingBlockPresenter(TView view,
         IPathAndValueEntityToPathAndValueEntityDTOMapper<TPathAndValueEntity, TStartValueDTO> valueMapper,
         IInteractionTasksForExtendablePathAndValueEntity<TBuildingBlock, TPathAndValueEntity> interactionTasksForExtendablePathAndValueEntity,
         IEmptyStartValueCreator<TPathAndValueEntity> emptyStartValueCreator,
         IMoBiContext context,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory,
         IDistributedPathAndValueEntityPresenter<TStartValueDTO, TBuildingBlock> distributedPathAndValuePresenter)
         : base(view, interactionTasksForExtendablePathAndValueEntity, formulaToValueFormulaDTOMapper, dimensionFactory, distributedPathAndValuePresenter)
      {
         _objectType = new ObjectTypeResolver().TypeFor(_focusedStartValue);
         _interactionTasksForExtendablePathAndValueEntity = interactionTasksForExtendablePathAndValueEntity;
         _valueMapper = valueMapper;
         IsOriginalStartValue = isOriginalStartValue;
         _emptyStartValueCreator = emptyStartValueCreator;
         _context = context;
         _originalStartValues = new List<TPathAndValueEntity>();
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
         _view.AddPathItems(_interactionTasksForExtendablePathAndValueEntity.GetContainerPathItemsForBuildingBlock(_buildingBlock).OrderBy(x => x));
         _view.InitializePathColumns();
      }

      public bool CanCreateNewFormula
      {
         set { _view.CanCreateNewFormula = value; }
      }

      public void Delete(IReadOnlyList<TStartValueDTO> selectedStartValues)
      {
         bulkRemove(selectedStartValues);
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

      public void RemovePathAndValueEntity(TStartValueDTO elementToRemove)
      {
         bulkRemove(new List<TStartValueDTO> { elementToRemove });
      }

      public override void Edit(TBuildingBlock buildingBlock)
      {
         base.Edit(buildingBlock);
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
         _startValueDTOs = ValueDTOsFor(buildingBlock).OrderBy(pathAndValueEntity => IsOriginalStartValue(pathAndValueEntity)).ToBindingList();
         bindToView();
         initializeColumns();
      }

      protected abstract IReadOnlyList<TStartValueDTO> ValueDTOsFor(TBuildingBlock buildingBlock);

      private void bindToView()
      {
         _view.BindTo(_startValueDTOs);
      }

      private void collectRemoveCommands(TStartValueDTO elementToRemove, BulkUpdateMacroCommand collector)
      {
         var pathAndValueEntity = PathAndValueEntityFrom(elementToRemove);
         if (_buildingBlock.Contains(pathAndValueEntity))
         {
            collector.AddCommand(_interactionTasksForExtendablePathAndValueEntity.RemovePathAndValueEntityFromBuildingBlockCommand(pathAndValueEntity, _buildingBlock));
         }
      }

      public void UpdatePathAndValueEntityContainerPath(TStartValueDTO startValueDTO, int indexToUpdate, string newValue)
      {
         AddCommand(_interactionTasksForExtendablePathAndValueEntity.EditPathAndValueEntityContainerPath(_buildingBlock, PathAndValueEntityFrom(startValueDTO), indexToUpdate, newValue));
      }

      public void UpdatePathAndValueEntityName(TStartValueDTO pathAndValueEntityDTO, string newValue)
      {
         var pathAndValueEntity = PathAndValueEntityFrom(pathAndValueEntityDTO);

         if (!_buildingBlock.Contains(pathAndValueEntity))
         {
            pathAndValueEntityDTO.UpdateName(newValue);
            AddCommand(_interactionTasksForExtendablePathAndValueEntity.AddPathAndValueEntityToBuildingBlock(_buildingBlock, pathAndValueEntity));
         }
         else
         {
            AddCommand(_interactionTasksForExtendablePathAndValueEntity.EditPathAndValueEntityName(_buildingBlock, pathAndValueEntity, newValue));
         }
      }

      protected TPathAndValueEntity PathAndValueEntityFrom(TStartValueDTO startValueDTO)
      {
         return startValueDTO?.PathWithValueObject;
      }

      protected TStartValueDTO StartValueDTOFrom(TPathAndValueEntity pathAndValueEntity)
      {
         return pathAndValueEntity == null ? null : _startValueDTOs.FirstOrDefault(dto => Equals(PathAndValueEntityFrom(dto), pathAndValueEntity));
      }

      public override void AddCommand(Func<ICommand> commandAction)
      {
         this.DoWithinLatch(() => base.AddCommand(commandAction));
      }

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


      public void Handle(BulkUpdateFinishedEvent eventToHandle)
      {
         _handleChangedEvents = true;
         // Always refresh once the batch update is finished
         reBind(_buildingBlock);
         _view.FocusedStartValue = StartValueDTOFrom(_focusedStartValue);
      }

      public void Handle(BulkUpdateStartedEvent eventToHandle)
      {
         _focusedStartValue = PathAndValueEntityFrom(_view.FocusedStartValue);
         _handleChangedEvents = false;
      }

      public TStartValueDTO AddNewEmptyPathAndValueEntity()
      {
         var newParameterValue = _emptyStartValueCreator.CreateEmptyStartValue(_interactionTasksForExtendablePathAndValueEntity.GetDefaultDimension());
         var newRecord = _valueMapper.MapFrom(newParameterValue, _buildingBlock);

         _startValueDTOs.Insert(0, newRecord);
         bindToView();

         return newRecord;
      }
   }
}