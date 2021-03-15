using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditEventBuilderPresenter : IEditPresenterWithParameters<IEventBuilder>, ICanEditPropertiesPresenter, IPresenterWithFormulaCache
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
      private IEventBuilder _eventBuilder;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      private readonly IEditTaskFor<IEventBuilder> _editTasks;
      private readonly IInteractionTasksForChildren<IEventBuilder, IEventAssignmentBuilder> _interactionTasksForEventAssignmentBuilder;
      private readonly IEditParametersInContainerPresenter _editParametersPresenter;
      private readonly ISelectReferenceAtEventPresenter _selectReferencePresenter;
      private IBuildingBlock _buildingBlock;
      private readonly IEditExplicitFormulaPresenter _editFormulaPresenter;
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly string _formulaPropertyName;
      private readonly IDialogCreator _dialogCreator;

      public EditEventBuilderPresenter(IEditEventBuilderView view, IEventBuilderToEventBuilderDTOMapper eventToEventBuilderMapper,
         IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper,
         IEditTaskFor<IEventBuilder> editTasks, IEditParametersInContainerPresenter editParametersPresenter,
         IInteractionTasksForChildren<IEventBuilder, IEventAssignmentBuilder> interactionTasksForEventAssignmentBuilder,
         IEditExplicitFormulaPresenter editFormulaPresenter, IMoBiContext context,
         ISelectReferenceAtEventPresenter selectReferencePresenter,
         IMoBiApplicationController applicationController, IDialogCreator dialogCreator)
         : base(view)
      {
         _selectReferencePresenter = selectReferencePresenter;
         _applicationController = applicationController;
         _dialogCreator = dialogCreator;
         _context = context;
         _editFormulaPresenter = editFormulaPresenter;
         _interactionTasksForEventAssignmentBuilder = interactionTasksForEventAssignmentBuilder;
         _eventToEventBuilderMapper = eventToEventBuilderMapper;
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
         _editTasks = editTasks;
         _editParametersPresenter = editParametersPresenter;

         _view.SetParametersView(editParametersPresenter.BaseView);
         _formulaPropertyName = _eventBuilder.PropertyName(x => x.Formula);

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
         Edit(objectToEdit.DowncastTo<IEventBuilder>());
      }

      public void Edit(IEventBuilder eventBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _eventBuilder = eventBuilder;
         _editParametersPresenter.Edit(eventBuilder);

         if (eventBuilder.Formula != null && eventBuilder.Formula.IsExplicit())
         {
            _editFormulaPresenter.Edit(eventBuilder.Formula, _eventBuilder);
            _view.SetFormulaView(_editFormulaPresenter.BaseView);
            _view.SetSelectReferenceView(_selectReferencePresenter.View);
         }

         ((ISelectReferencePresenter) _selectReferencePresenter).Init(_eventBuilder, new[] {_eventBuilder.RootContainer}, _eventBuilder);
         var dto = _eventToEventBuilderMapper.MapFrom(eventBuilder);
         dto.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(eventBuilder, existingObjectsInParent));
         _view.Show(dto);
         checkFormulaName(dto.Condition);
      }

      public void Edit(IEventBuilder eventBuilder)
      {
         Edit(eventBuilder, eventBuilder.ParentContainer);
      }

      public object Subject => _eventBuilder;

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _eventBuilder, BuildingBlock).Run(_context));
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

         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(_eventBuilder.PropertyName(x => x.Formula), newFormula, _eventBuilder.Formula, _eventBuilder, BuildingBlock).Run(_context));
         _editFormulaPresenter.Edit(newFormula);
         _view.SetFormulaView(_editFormulaPresenter.BaseView);
         _view.SetSelectReferenceView(_selectReferencePresenter.View);
         checkFormulaName(newFormula.Name);
      }

      public void SetTargetPathFor(EventAssignmentBuilderDTO eventAssignmentBuilderDTO)
      {
         IObjectPath objectPath;
         using (var selectEventAssignmentTargetPresenter = _applicationController.Start<ISelectEventAssingmentTargetPresenter>())
         {
            selectEventAssignmentTargetPresenter.Init(_context.CurrentProject, _eventBuilder.RootContainer);
            objectPath = selectEventAssignmentTargetPresenter.Select();
         }

         if (objectPath == null) 
            return;

         setChantedEntityPath(objectPath, eventAssignmentBuilderDTO);
      }

      public void SetChangedEntityPath(string newPath, EventAssignmentBuilderDTO dto)
      {
         var objectPath = new ObjectPath(newPath.ToPathArray());
         setChantedEntityPath(objectPath, dto);
      }

      private void setChantedEntityPath(IObjectPath objectPath, EventAssignmentBuilderDTO dto)
      {
         var eventAssignmentBuilder = eventAssignmentBuilderFor(dto);
         SetPropertyValueFor(dto, eventAssignmentBuilder.PropertyName(x => x.ObjectPath), objectPath, eventAssignmentBuilder.ObjectPath);
         dto.ChangedEntityPath = objectPath.PathAsString;
      }

      public void SetPropertyValueFor<T>(EventAssignmentBuilderDTO eventAssignmentBuilderDTO, string propertyName, T newValue, T oldValue)
      {
         var eventAssignmentBuilder = eventAssignmentBuilderFor(eventAssignmentBuilderDTO);
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, eventAssignmentBuilder, BuildingBlock).Run(_context));
      }

      private IEventAssignmentBuilder eventAssignmentBuilderFor(EventAssignmentBuilderDTO eventAssignmentBuilderDTO)
      {
         return _eventBuilder.Assignments.Single(dto => dto.Id.Equals(eventAssignmentBuilderDTO.Id));
      }

      public IEnumerable<string> AllFormulaNames()
      {
         return FormulaCache.OrderBy(formula => formula.Name).Select(x => x.Name);
      }

      public void AddConditionFormula()
      {
         string newName = _dialogCreator.AskForInput(AppConstants.Captions.NewName, AppConstants.Captions.EnterNewFormulaName, string.Empty, AllFormulaNames());
         checkFormulaName(newName);
         if (string.IsNullOrEmpty(newName)) return;

         var newFormula = _context.Create<ExplicitFormula>().WithName(newName);
         var newFormulaCommand = new MoBiMacroCommand
         {
            Description = AppConstants.Commands.EditDescription(ObjectTypes.EventBuilder, _formulaPropertyName, string.Empty, newName, _eventBuilder.Name),
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = ObjectTypes.EventBuilder
         };
         newFormulaCommand.AddCommand(new AddFormulaToFormulaCacheCommand(BuildingBlock, newFormula).Run(_context));
         newFormulaCommand.AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(_formulaPropertyName, newFormula, _eventBuilder.Formula, _eventBuilder, BuildingBlock).Run(_context));
         AddCommand(newFormulaCommand);
         Edit(_eventBuilder);
      }

      public void SetFormulaFor(EventAssignmentBuilderDTO eventAssignmentBuilderDTO, FormulaBuilderDTO formulaBuilderDTO)
      {
         var newFormula = _context.Get<ExplicitFormula>(formulaBuilderDTO.Id);
         var eventAssignmentBuilder = _context.Get<IEventAssignmentBuilder>(eventAssignmentBuilderDTO.Id);
         AddCommand(
            new EditObjectBasePropertyInBuildingBlockCommand(eventAssignmentBuilder.PropertyName(b => b.Formula), newFormula, eventAssignmentBuilder.Formula, eventAssignmentBuilder, BuildingBlock).Run(_context));
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
            return _eventBuilder.Parameters.Contains((IParameter) addedObject);

         return addedObject.IsAnImplementationOf<IEventAssignmentBuilder>();
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
         return eventToHandle.RemovedObjects.Any(removedObject => removedObject.IsAnImplementationOf<IEventAssignmentBuilder>() ||
                                                                  removedObject.IsAnImplementationOf<IParameter>());
      }
   }
}