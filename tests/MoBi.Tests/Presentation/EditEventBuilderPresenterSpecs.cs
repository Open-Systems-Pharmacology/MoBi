using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using OSPSuite.BDDHelper;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Collections;
using System.Runtime.InteropServices;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditEventBuilderPresenterSpecs : ContextSpecification<EditEventBuilderPresenter>
   {
      private IEditEventBuilderView _view;
      private IEventBuilderToEventBuilderDTOMapper _eventBuilderMapper;
      private IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      private IEditTaskFor<EventBuilder> _eventBuilderTasks;
      private IEditParametersInContainerPresenter _parameterPresenter;
      private IInteractionTasksForChildren<EventBuilder, EventAssignmentBuilder> _assignmentBuilderTasks;
      private IEditExplicitFormulaPresenter _formulaPresenter;
      protected IMoBiContext _context;
      protected ISelectReferenceAtEventPresenter _selectReferencePresenter;
      protected IMoBiApplicationController _applicationController;
      private IObjectBaseNamingTask _namingTask;

      protected override void Context()
      {
         _view = A.Fake<IEditEventBuilderView>();
         _eventBuilderMapper = A.Fake<IEventBuilderToEventBuilderDTOMapper>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _eventBuilderTasks = A.Fake<IEditTaskFor<EventBuilder>>();
         _parameterPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _assignmentBuilderTasks = A.Fake<IInteractionTasksForChildren<EventBuilder, EventAssignmentBuilder>>();
         _formulaPresenter = A.Fake<IEditExplicitFormulaPresenter>();
         _context = A.Fake<IMoBiContext>();
         _selectReferencePresenter = A.Fake<ISelectReferenceAtEventPresenter>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _namingTask = A.Fake<IObjectBaseNamingTask>();
         sut = new EditEventBuilderPresenter(_view, _eventBuilderMapper, _formulaMapper, _eventBuilderTasks, _parameterPresenter, _assignmentBuilderTasks, _formulaPresenter, _context, _selectReferencePresenter, _applicationController, _namingTask);
      }
   }

   public class When_selecting_assignment_for_an_event : concern_for_EditEventBuilderPresenterSpecs
   {
      private EventAssignmentBuilderDTO _assignmentDTO;
      private ISelectEventAssignmentTargetPresenter _selectEventAssignmentTargetPresenter;
      private EventBuilder _eventBuilder;
      private ICommandCollector _commandRegister;
      private Parameter _forbiddenObject;

      protected override void Context()
      {
         base.Context();
         var eventAssignmentBuilder = new EventAssignmentBuilder
         {
            UseAsValue = false,
            Formula = A.Fake<ExplicitFormula>(),
            ObjectPath = new ObjectPath("..", "..", "Second")
         };

         A.CallTo(() => eventAssignmentBuilder.Formula.ObjectPaths).Returns(new List<FormulaUsablePath> { new FormulaUsablePath("..", "..", "Second") });
         _assignmentDTO = new EventAssignmentBuilderDTO(eventAssignmentBuilder)
         {
            NewFormula = new FormulaBuilderDTO(eventAssignmentBuilder.Formula)
            {
               ObjectPaths = eventAssignmentBuilder.Formula.ObjectPaths.Select(x => new FormulaUsablePathDTO(x, eventAssignmentBuilder.Formula)).ToList()
            }
         };
         _selectEventAssignmentTargetPresenter = A.Fake<ISelectEventAssignmentTargetPresenter>();
         A.CallTo(() => _applicationController.Start<ISelectEventAssignmentTargetPresenter>()).Returns(_selectEventAssignmentTargetPresenter);

         _forbiddenObject = new Parameter().WithName("Second");
         _eventBuilder = new EventBuilder();
         _eventBuilder.AddAssignment(eventAssignmentBuilder);
         new Event
         {
            _eventBuilder,
            _forbiddenObject
         };
         sut.Edit(_eventBuilder);
         _commandRegister = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandRegister);
      }

      protected override void Because()
      {
         sut.SetTargetPathFor(_assignmentDTO);
      }

      [Observation]
      public void should_forbid_selection_of_formula_target()
      {
         A.CallTo(() => _selectEventAssignmentTargetPresenter.Init(A<IContainer>._, A<ICache<IObjectBase, string>>.That.Matches(x => hasForbiddenObjects(x)))).MustHaveHappened();
      }

      private bool hasForbiddenObjects(ICache<IObjectBase, string> cache)
      {
         return cache.Keys.ToList().Contains(_forbiddenObject);
      }
   }


public class When_setting_a_new_formula_for_an_event_assignment : concern_for_EditEventBuilderPresenterSpecs
   {
      private EventAssignmentBuilderDTO _assignmentDTO;
      private FormulaBuilderDTO _formulaDTO;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
         _assignmentDTO = new EventAssignmentBuilderDTO(new EventAssignmentBuilder().WithId("AA"));
         _formulaDTO =new FormulaBuilderDTO(new ExplicitFormula().WithId("B"));
         A.CallTo(() => _context.Get<ExplicitFormula>(A<string>._)).Returns(A.Fake<ExplicitFormula>());
         A.CallTo(() => _context.Get<EventAssignmentBuilder>(A<string>._)).Returns(A.Fake<EventAssignmentBuilder>());
      }

      protected override void Because()
      {
         sut.SetFormulaFor(_assignmentDTO, _formulaDTO);
      }

      [Observation]
      public void should_Add_Edit_command_toi_command_collector()
      {
         A.CallTo(() => _commandCollector.AddCommand(A<EditObjectBasePropertyInBuildingBlockCommand>._)).MustHaveHappened();
      }

      [Observation]
      public void should_retrieve_domain_objects()
      {
         A.CallTo(() => _context.Get<ExplicitFormula>(_formulaDTO.Id)).MustHaveHappened();
         A.CallTo(() => _context.Get<EventAssignmentBuilder>(_assignmentDTO.Id)).MustHaveHappened();
      }
   }
}	