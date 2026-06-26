using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;

namespace MoBi.Presentation;

public abstract class concern_for_ParallelSimulationUpdateTask : ContextSpecification<ParallelSimulationUpdateTask>
{
   protected IMoBiContext _context;
   protected ISimulationUpdateTask _simulationUpdateTask;
   protected ISimulationUpdatePresenter _simulationUpdatePresenter;
   protected ICommand _updateCommand;

   protected override void Context()
   {
      _context = A.Fake<IMoBiContext>();
      _simulationUpdateTask = A.Fake<ISimulationUpdateTask>();
      _simulationUpdatePresenter = A.Fake<ISimulationUpdatePresenter>();
      _updateCommand = A.Fake<ICommand>();
      A.CallTo(() => _context.Resolve<ISimulationUpdatePresenter>()).Returns(_simulationUpdatePresenter);
      sut = new ParallelSimulationUpdateTask(_context, _simulationUpdateTask);
   }
}

public class When_updating_a_single_simulation : concern_for_ParallelSimulationUpdateTask
{
   private IMoBiSimulation _simulation;
   private ICommand _result;

   protected override void Context()
   {
      base.Context();
      _simulation = A.Fake<IMoBiSimulation>();
      A.CallTo(() => _simulationUpdateTask.UpdateSimulation(_simulation)).Returns(_updateCommand);
   }

   protected override void Because()
   {
      _result = sut.UpdateSimulations(new List<IMoBiSimulation> { _simulation });
   }

   [Observation]
   public void should_update_the_simulation_through_the_single_simulation_task()
   {
      A.CallTo(() => _simulationUpdateTask.UpdateSimulation(_simulation)).MustHaveHappened();
   }

   [Observation]
   public void should_not_open_the_parallel_feedback_presenter()
   {
      A.CallTo(() => _simulationUpdatePresenter.Start(A<IReadOnlyList<IMoBiSimulation>>._)).MustNotHaveHappened();
   }

   [Observation]
   public void should_return_the_command_produced_by_the_task()
   {
      _result.ShouldBeEqualTo(_updateCommand);
   }
}

public class When_updating_several_simulations : concern_for_ParallelSimulationUpdateTask
{
   private IReadOnlyList<IMoBiSimulation> _simulations;
   private ICommand _result;

   protected override void Context()
   {
      base.Context();
      _simulations = new List<IMoBiSimulation> { A.Fake<IMoBiSimulation>(), A.Fake<IMoBiSimulation>() };
      A.CallTo(() => _simulationUpdatePresenter.Start(_simulations)).Returns(_updateCommand);
   }

   protected override void Because()
   {
      _result = sut.UpdateSimulations(_simulations);
   }

   [Observation]
   public void should_run_the_update_through_the_parallel_feedback_presenter()
   {
      A.CallTo(() => _simulationUpdatePresenter.Start(_simulations)).MustHaveHappened();
   }

   [Observation]
   public void should_not_use_the_single_simulation_task()
   {
      A.CallTo(() => _simulationUpdateTask.UpdateSimulation(A<IMoBiSimulation>._)).MustNotHaveHappened();
   }

   [Observation]
   public void should_return_the_command_produced_by_the_presenter()
   {
      _result.ShouldBeEqualTo(_updateCommand);
   }
}