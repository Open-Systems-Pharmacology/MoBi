using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_ConfigureSimulationUICommand : ContextSpecification<ConfigureSimulationUICommand>
   {
      protected IEditTasksForSimulation _simulationTask;
      protected IMoBiSimulation _simulation;
      protected IActiveSubjectRetriever _activeSubjectRetriever;

      protected override void Context()
      {
         _simulationTask = A.Fake<IEditTasksForSimulation>();
         _activeSubjectRetriever= A.Fake<IActiveSubjectRetriever>();
         sut = new ConfigureSimulationUICommand(_simulationTask, _activeSubjectRetriever);

         _simulation = A.Fake<IMoBiSimulation>();
         sut.Subject = _simulation;
      }
   }

   public class When_executing_the_configure_simulation_ui_command : concern_for_ConfigureSimulationUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_call_the_expected_configure_simulation_method_for_the_provided_simulation()
      {
         A.CallTo(() => _simulationTask.Configure(_simulation)).MustHaveHappened();
      }
   }
}