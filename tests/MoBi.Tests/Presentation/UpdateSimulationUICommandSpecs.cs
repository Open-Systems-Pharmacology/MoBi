using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public class concern_for_UpdateSimulationUICommand : ContextSpecification<UpdateSimulationUICommand>
   {
      protected IParallelSimulationUpdateTask _parallelSimulationUpdateTask;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IMoBiContext _context;
      protected ICommand _updateCommand;
      protected IReadOnlyList<IMoBiSimulation> _simulations;

      protected override void Context()
      {
         _parallelSimulationUpdateTask = A.Fake<IParallelSimulationUpdateTask>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _context = A.Fake<IMoBiContext>();
         _updateCommand = A.Fake<ICommand>();
         _simulations = new List<IMoBiSimulation> { A.Fake<IMoBiSimulation>() };
         sut = new UpdateSimulationUICommand(_parallelSimulationUpdateTask, _context, _activeSubjectRetriever);
         sut.Subject = _simulations;
         A.CallTo(() => _parallelSimulationUpdateTask.UpdateSimulations(_simulations)).Returns(_updateCommand);
      }
   }

   public class When_executing_the_update_simulation_ui_command : concern_for_UpdateSimulationUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_delegate_the_update_to_the_parallel_simulation_updater()
      {
         A.CallTo(() => _parallelSimulationUpdateTask.UpdateSimulations(_simulations)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_resulting_command_to_the_history()
      {
         A.CallTo(() => _context.AddToHistory(_updateCommand)).MustHaveHappened();
      }
   }
}