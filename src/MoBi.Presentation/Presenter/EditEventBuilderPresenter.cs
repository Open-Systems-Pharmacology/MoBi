using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditEventBuilderPresenter : IEditPresenterWithParameters<EventBuilder>, ICanEditPropertiesPresenter, IPresenterWithFormulaCache
      , IListener<AddedEvent>, IListener<RemovedEvent>
   {
      void AddAssignment();
      void RemoveAssignment(EventAssignmentBuilderDTO dtoAssignmentBuilder);
      void SetConditionFormula(string formulaName);
      void SetTargetPathFor(EventAssignmentBuilderDTO eventAssignmentBuilderDTO);
      void SetPropertyValueFor<T>(EventAssignmentBuilderDTO eventAssignmentBuilderDTO, string propertyName, T newValue, T oldValue);
      IEnumerable<string> AllFormulaNames();
      void AddConditionFormula();
      void SetFormulaFor(EventAssignmentBuilderDTO eventAssignmentBuilderDTO, FormulaBuilderDTO formulaBuilderDTO);
      void SetChangedEntityPath(string newPath, EventAssignmentBuilderDTO dto);
   }

   public class EditEventBuilderPresenter : AbstractCommandCollectorPresenter<IEditEventBuilderView, IEditEventBuilderPresenter>, IEditEventBuilderPresenter
   {
      private readonly IEventBuilderToEventBuilderDTOMapper _eventToEventBuilderMapper;
      private EventBuilder _eventBuilder;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      private readonly IEditTaskFor<EventBuilder> _editTasks;
      private readonly IInteractionTasksForChildren<EventBuilder, EventAssignmentBuilder> _interactionTasksForEventAssignmentBuilder;
      private readonly IEditParametersInContainerPresenter _editParametersPresenter;
      private readonly ISelectReferenceAtEventPresenter _selectReferencePresenter;
      private IBuildingBlock _buildingBlock;
      private readonly IEditExplicitFormulaPresenter _editFormulaPresenter;
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private string _formulaPropertyName;
      private readonly IObjectBaseNamingTask _namingTask;

      public EditEventBuilderPresenter(IEditEventBuilderView view, IEventBuilderToEventBuilderDTOMapper eventToEventBuilderMapper,
         IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper,
         IEditTaskFor<EventBuilder> editTasks, IEditParametersInContainerPresenter editParametersPresenter,
         IInteractionTasksForChildren<EventBuilder, EventAssignmentBuilder> interactionTasksForEventAssignmentBuilder,
         IEditExplicitFormulaPresenter editFormulaPresenter, IMoBiContext context,
         ISelectReferenceAtEventPresenter selectReferencePresenter,
         IMoBiApplicationController applicationController, IObjectBaseNamingTask namingTask)
         : base(view)
      {
         _selectReferencePresenter = selectReferencePresenter;
         _applicationController = applicationController;
         _namingTask = namingTask;
         _context = context;
         _editFormulaPresenter = editFormulaPresenter;
         _interactionTasksForEventAssignmentBuilder = interactionTasksForEventAssignmentBuilder;
         _eventToEventBuilderMapper = eventToEventBuilderMapper;
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
         _editTasks = editTasks;
         _editParametersPresenter = editParametersPresenter;

         _view.SetParametersView(editParametersPresenter.BaseView);

         AddSubPresenters(_editFormulaPresenter, _editParametersPresenter, _selectReferencePresenter);
      }

      public void SelectParameter(IParameter parameter)
      {
         if (parameter == null) return;
         _view.ShowParameters();
         _editParametersPresenter.Select(parameter);
      }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<EventBuilder>());
      }

      public void Edit(EventBuilder eventBuilder, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _eventBuilder = eventBuilder;
         _formulaPropertyName = _eventBuilder.PropertyName(x => x.Formula);
         _editParametersPresenter.Edit(eventBuilder);

         if (eventBuilder.Formula != null && eventBuilder.Formula.IsExplicit())
         {
            _editFormulaPresenter.Edit(eventBuilder.Formula, _eventBuilder);
            _view.SetFormulaView(_editFormulaPresenter.BaseView);
            _view.SetSelectReferenceView(_selectReferencePresenter.View);
         }

         ((ISelectReferencePresenter)_selectReferencePresenter).Init(_eventBuilder, new[] { _eventBuilder.RootContainer }, _eventBuilder);
         var dto = _eventToEventBuilderMapper.MapFrom(eventBuilder);
         dto.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(eventBuilder, existingObjectsInParent));
         _view.Show(dto);
         checkFormulaName(dto.Condition);
      }

      public void Edit(EventBuilder eventBuilder)
      {
         Edit(eventBuilder, eventBuilder.ParentContainer?.Children);
      }

      public object Subject => _eventBuilder;

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _eventBuilder, BuildingBlock).RunCommand(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_eventBuilder, _eventBuilder.ParentContainer, BuildingBlock);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return BuildingBlock.FormulaCache.MapAllUsing(_formulaToDTOFormulaMapper);
      }

      public IBuildingBlock BuildingBlock
      {
         get => _buildingBlock;
         set
         {
            _buildingBlock = value;
            _editParametersPresenter.BuildingBlock = value;
            _editFormulaPresenter.BuildingBlock = value;
         }
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public void AddAssignment()
      {
         AddCommand(_interactionTasksForEventAssignmentBuilder.AddNew(_eventBuilder, BuildingBlock));
      }

      public void RemoveAssignment(EventAssignmentBuilderDTO dtoAssignmentBuilder)
      {
         var eventAssignment = eventAssignmentBuilderFor(dtoAssignmentBuilder);
         AddCommand(_interactionTasksForEventAssignmentBuilder.Remove(eventAssignment, _eventBuilder, BuildingBlock));
      }

      public void SetConditionFormula(string formulaName)
      {
         var newFormula = FormulaCache.First(formula => formula.Name.Equals(formulaName));
         if (newFormula == _eventBuilder.Formula)
            return;

         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(_eventBuilder.PropertyName(x => x.Formula), newFormula, _eventBuilder.Formula, _eventBuilder, BuildingBlock).RunCommand(_context));
         _editFormulaPresenter.Edit(newFormula);
         _view.SetFormulaView(_editFormulaPresenter.BaseView);
         _view.SetSelectReferenceView(_selectReferencePresenter.View);
         checkFormulaName(newFormula.Name);
      }

      public void SetTargetPathFor(EventAssignmentBuilderDTO eventAssignmentBuilderDTO)
      {
         ObjectPath objectPath;
         using (var selectEventAssignmentTargetPresenter = _applicationController.Start<ISelectEventAssignmentTargetPresenter>())
         {
            var eventAssignmentBuilder = eventAssignmentBuilderFor(eventAssignmentBuilderDTO);
            selectEventAssignmentTargetPresenter.Init(_eventBuilder.RootContainer, getForbiddenAssignees(eventAssignmentBuilder));
            objectPath = selectEventAssignmentTargetPresenter.Select();
         }

         if (objectPath == null)
            return;

         setChangedEntityPath(objectPath, eventAssignmentBuilderDTO);
      }

      private ICache<IObjectBase, string> getForbiddenAssignees(EventAssignmentBuilder eventAssignmentBuilder)
      {
         var cache = new Cache<IObjectBase, string>();
         if (eventAssignmentBuilder.UseAsValue || eventAssignmentBuilder.Formula == null)
            return cache;

         eventAssignmentBuilder.Formula.ObjectPaths.Select(x => x.TryResolve<IUsingFormula>(eventAssignmentBuilder)).Each(x => cache.Add(x, AppConstants.Captions.AssigningFormulaCreatesCircularReference));
         return cache;
      }

      public void SetChangedEntityPath(string newPath, EventAssignmentBuilderDTO dto)
      {
         var objectPath = new ObjectPath(newPath.ToPathArray());
         setChangedEntityPath(objectPath, dto);
      }

      private void setChangedEntityPath(ObjectPath objectPath, EventAssignmentBuilderDTO dto)
      {
         var eventAssignmentBuilder = eventAssignmentBuilderFor(dto);
         SetPropertyValueFor(dto, eventAssignmentBuilder.PropertyName(x => x.ObjectPath), objectPath, eventAssignmentBuilder.ObjectPath);
         dto.ChangedEntityPath = objectPath.PathAsString;
      }

      public void SetPropertyValueFor<T>(EventAssignmentBuilderDTO eventAssignmentBuilderDTO, string propertyName, T newValue, T oldValue)
      {
         var eventAssignmentBuilder = eventAssignmentBuilderFor(eventAssignmentBuilderDTO);
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, eventAssignmentBuilder, BuildingBlock).RunCommand(_context));
      }

      private EventAssignmentBuilder eventAssignmentBuilderFor(EventAssignmentBuilderDTO eventAssignmentBuilderDTO)
      {
         return _eventBuilder.Assignments.Single(dto => dto.Id.Equals(eventAssignmentBuilderDTO.Id));
      }

      public IEnumerable<string> AllFormulaNames()
      {
         return FormulaCache.OrderBy(formula => formula.Name).Select(x => x.Name);
      }

      public void AddConditionFormula()
      {
         string newName = _namingTask.NewName(AppConstants.Captions.NewName, AppConstants.Captions.EnterNewFormulaName, string.Empty, AllFormulaNames());
         checkFormulaName(newName);
         if (string.IsNullOrEmpty(newName)) return;

         var newFormula = _context.Create<ExplicitFormula>().WithName(newName);
         var newFormulaCommand = new MoBiMacroCommand
         {
            Description = AppConstants.Commands.EditDescription(ObjectTypes.EventBuilder, _formulaPropertyName, string.Empty, newName, _eventBuilder.Name),
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = ObjectTypes.EventBuilder
         };
         newFormulaCommand.AddCommand(new AddFormulaToFormulaCacheCommand(BuildingBlock, newFormula).RunCommand(_context));
         newFormulaCommand.AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(_formulaPropertyName, newFormula, _eventBuilder.Formula, _eventBuilder, BuildingBlock).RunCommand(_context));
         AddCommand(newFormulaCommand);
         Edit(_eventBuilder);
      }

      public void SetFormulaFor(EventAssignmentBuilderDTO eventAssignmentBuilderDTO, FormulaBuilderDTO formulaBuilderDTO)
      {
         var newFormula = _context.Get<ExplicitFormula>(formulaBuilderDTO.Id);
         var eventAssignmentBuilder = _context.Get<EventAssignmentBuilder>(eventAssignmentBuilderDTO.Id);
         AddCommand(
            new EditObjectBasePropertyInBuildingBlockCommand(eventAssignmentBuilder.PropertyName(b => b.Formula), newFormula, eventAssignmentBuilder.Formula, eventAssignmentBuilder, BuildingBlock).RunCommand(_context));
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (shouldShow(eventToHandle.AddedObject))
         {
            Edit(_eventBuilder);
         }
      }

      private bool shouldShow(IObjectBase addedObject)
      {
         if (_eventBuilder == null) return false;
         if (addedObject.IsAnImplementationOf<IParameter>())
            return _eventBuilder.Parameters.Contains((IParameter)addedObject);

         return addedObject.IsAnImplementationOf<EventAssignmentBuilder>();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (canHandle(eventToHandle))
         {
            Edit(_eventBuilder);
         }
      }

      private void checkFormulaName(string formulaName)
      {
         if (formulaName.IsNullOrEmpty())
         {
            _view.NameValidationError(AppConstants.Validation.EmptyFormula);
         }
         else
         {
            _view.ResetNameValidationError();
         }
      }

      private bool canHandle(RemovedEvent eventToHandle)
      {
         if (_eventBuilder == null) return false;
         return eventToHandle.RemovedObjects.Any(removedObject => removedObject.IsAnImplementationOf<EventAssignmentBuilder>() ||
                                                                  removedObject.IsAnImplementationOf<IParameter>());
      }
   }
}