using FakeItEasy;
using OSPSuite.BDDHelper;
using MoBi.Presentation.UICommand;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public class concern_for_UpdateSimulationUICommand : ContextSpecification<UpdateSimulationUICommand>
   {
      protected ISimulationUpdateTask _simulationUpdateTask;
      protected IMoBiSimulation _simulation;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _simulationUpdateTask = A.Fake<ISimulationUpdateTask>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _context = A.Fake<IMoBiContext>();
         sut = new UpdateSimulationUICommand(_simulationUpdateTask, _context, _activeSubjectRetriever);

         _simulation = A.Fake<IMoBiSimulation>();
         sut.Subject = _simulation;
      }
   }

   public class When_executing_the_update_simulation_ui_command : concern_for_UpdateSimulationUICommand
   {
      private ICommand _updateCommand;

      protected override void Context()
      {
         base.Context();
         _updateCommand = A.Fake<ICommand>();
         A.CallTo(_simulationUpdateTask).WithReturnType<ICommand>().Returns(_updateCommand);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_call_the_expected_update_simulation_method_for_the_provided_simulation()
      {
         A.CallTo(() => _simulationUpdateTask.UpdateSimulation(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_command_to_the_history()
      {
         A.CallTo(() => _context.AddToHistory(_updateCommand)).MustHaveHappened();
      }
   }
}
