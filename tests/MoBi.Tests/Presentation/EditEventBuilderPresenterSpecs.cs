using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using OSPSuite.BDDHelper;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditEventBuilderPresenterSpecs : ContextSpecification<IEditEventBuilderPresenter>
   {
      private IEditEventBuilderView _view;
      private IEventBuilderToEventBuilderDTOMapper _eventBuilderMapper;
      private IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      private IEditTaskFor<IEventBuilder> _eventBuilderTasks;
      private IEditParametersInContainerPresenter _parameterPresenter;
      private IInteractionTasksForChildren<IEventBuilder, IEventAssignmentBuilder> _assingmentBuilderTasks;
      private IEditExplicitFormulaPresenter _formulaPresenter;
      protected IMoBiContext _context;
      private ISelectReferenceAtEventPresenter _selectReferencePresenter;
      private IMoBiApplicationController _applicationController;
      private IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _view = A.Fake<IEditEventBuilderView>();
         _eventBuilderMapper=A.Fake<IEventBuilderToEventBuilderDTOMapper>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _eventBuilderTasks = A.Fake<IEditTaskFor<IEventBuilder>>();
         _parameterPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _assingmentBuilderTasks = A.Fake<IInteractionTasksForChildren<IEventBuilder, IEventAssignmentBuilder>>();
         _formulaPresenter = A.Fake<IEditExplicitFormulaPresenter>();
         _context = A.Fake<IMoBiContext>();
         _selectReferencePresenter = A.Fake<ISelectReferenceAtEventPresenter>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new EditEventBuilderPresenter(_view,_eventBuilderMapper,_formulaMapper,_eventBuilderTasks,_parameterPresenter,_assingmentBuilderTasks,_formulaPresenter,_context,_selectReferencePresenter, _applicationController, _dialogCreator);
      }
   }

   class When_setting_a_new_formula_for_an_event_assingment : concern_for_EditEventBuilderPresenterSpecs
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
         A.CallTo(() => _context.Get<IEventAssignmentBuilder>(A<string>._)).Returns(A.Fake<IEventAssignmentBuilder>());
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
         A.CallTo(() => _context.Get<IEventAssignmentBuilder>(_assignmentDTO.Id)).MustHaveHappened();
      }
   }
}	