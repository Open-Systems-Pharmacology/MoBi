using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.HelpersForTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation;

public abstract class concern_for_SimulationUpdatePresenter : ContextSpecification<SimulationUpdatePresenter>
{
   protected ISimulationUpdateView _view;
   protected IMoBiContext _context;
   protected IEventPublisher _eventPublisher;
   protected ISimulationUpdateTask _simulationUpdateTask;
   private SynchronizationContext _originalSynchronizationContext;

   protected override void Context()
   {
      _view = A.Fake<ISimulationUpdateView>();
      _context = A.Fake<IMoBiContext>();
      _eventPublisher = A.Fake<IEventPublisher>();
      _simulationUpdateTask = A.Fake<ISimulationUpdateTask>();

      _originalSynchronizationContext = SynchronizationContext.Current;
      SynchronizationContext.SetSynchronizationContext(new SynchronousSynchronizationContextForSpecs());

      sut = new SimulationUpdatePresenter(_view, _context, _eventPublisher, _simulationUpdateTask, new SimulationToSimulationUpdateStatusDTOMapper());
   }

   public override void Cleanup()
   {
      base.Cleanup();
      SynchronizationContext.SetSynchronizationContext(_originalSynchronizationContext);
   }
}

public class When_updating_multiple_simulations_where_one_cannot_be_configured : concern_for_SimulationUpdatePresenter
{
   private MoBiSimulation _successfulSimulation;
   private MoBiSimulation _failingSimulation;
   private ICommand _result;

   protected override void Context()
   {
      base.Context();
      _successfulSimulation = new MoBiSimulation { Id = "GOOD", Configuration = new SimulationConfiguration(), Model = new Model { Root = new Container() }.WithName("GOOD_MODEL") };
      _failingSimulation = new MoBiSimulation { Id = "BAD", Configuration = new SimulationConfiguration(), Model = new Model { Root = new Container() }.WithName("BAD_MODEL") };

      var model = new Model { Root = new Container() }.WithName("NEW_MODEL");
      var simulationBuilder = A.Fake<SimulationBuilder>();
      A.CallTo(() => simulationBuilder.EntitySources).Returns(new List<SimulationEntitySource>());
      var creationResult = new CreationResult(model, simulationBuilder);

      var results = new List<ModelCreationAndValidationResult>
      {
         new ModelCreationAndValidationResult(_successfulSimulation) { CreationResult = creationResult, ClonedConfiguration = new SimulationConfiguration() },
         new ModelCreationAndValidationResult(_failingSimulation)
      };

      A.CallTo(() => _simulationUpdateTask.ConfigureSimulationsInParallel(A<IReadOnlyList<IMoBiSimulation>>._, A<CancellationToken>._))
         .Returns(Task.FromResult<IReadOnlyList<ModelCreationAndValidationResult>>(results));
   }

   protected override void Because()
   {
      _result = sut.Start([_successfulSimulation, _failingSimulation]);
   }

   [Observation]
   public void only_the_successfully_configured_simulation_should_contribute_a_command()
   {
      (_result as MoBiMacroCommand).Count.ShouldBeEqualTo(1);
   }

   [Observation]
   public void the_command_description_should_count_only_the_updated_simulations()
   {
      (_result as MoBiMacroCommand).Description.ShouldBeEqualTo(AppConstants.Commands.ConfigureSimulationsDescription(1));
   }
}

public class When_cancelling_an_update_with_a_faulted_and_an_unstarted_simulation : concern_for_SimulationUpdatePresenter
{
   private MoBiSimulation _validSimulation;
   private MoBiSimulation _faultedSimulation;
   private MoBiSimulation _unstartedSimulation;
   private List<SimulationUpdateStatusDTO> _boundDTOs;

   protected override void Context()
   {
      base.Context();
      _validSimulation = new MoBiSimulation { Id = "VALID", Configuration = new SimulationConfiguration(), Model = new Model { Root = new Container() }.WithName("VALID_MODEL") };
      _faultedSimulation = new MoBiSimulation { Id = "FAULTED", Configuration = new SimulationConfiguration(), Model = new Model { Root = new Container() }.WithName("FAULTED_MODEL") };
      _unstartedSimulation = new MoBiSimulation { Id = "UNSTARTED", Configuration = new SimulationConfiguration(), Model = new Model { Root = new Container() }.WithName("UNSTARTED_MODEL") };

      var model = new Model { Root = new Container() }.WithName("NEW_MODEL");
      var simulationBuilder = A.Fake<SimulationBuilder>();
      A.CallTo(() => simulationBuilder.EntitySources).Returns(new List<SimulationEntitySource>());
      var creationResult = new CreationResult(model, simulationBuilder);

      var results = new List<ModelCreationAndValidationResult>
      {
         new ModelCreationAndValidationResult(_validSimulation) { CreationResult = creationResult, ClonedConfiguration = new SimulationConfiguration() },
         //configured (its building blocks were processed) but produced no valid model
         new ModelCreationAndValidationResult(_faultedSimulation) { ClonedConfiguration = new SimulationConfiguration() },
         //never configured - cancellation stopped it before it started
         new ModelCreationAndValidationResult(_unstartedSimulation)
      };

      A.CallTo(() => _view.BindTo(A<IEnumerable<SimulationUpdateStatusDTO>>._))
         .Invokes((IEnumerable<SimulationUpdateStatusDTO> dtos) => _boundDTOs = dtos.ToList());

      //the user cancels during configuration before the results are applied
      A.CallTo(() => _simulationUpdateTask.ConfigureSimulationsInParallel(A<IReadOnlyList<IMoBiSimulation>>._, A<CancellationToken>._))
         .ReturnsLazily(() =>
         {
            sut.Cancel();
            return Task.FromResult<IReadOnlyList<ModelCreationAndValidationResult>>(results);
         });
   }

   protected override void Because()
   {
      sut.Start([_validSimulation, _faultedSimulation, _unstartedSimulation]);
   }

   private RunStatus statusOf(string simulationId) => _boundDTOs.Single(x => x.SimulationId == simulationId).Status;

   [Observation]
   public void the_configured_valid_simulation_should_complete()
   {
      statusOf("VALID").ShouldBeEqualTo(RunStatus.RanToCompletion);
   }

   [Observation]
   public void a_configured_but_invalid_simulation_should_be_faulted_not_cancelled()
   {
      statusOf("FAULTED").ShouldBeEqualTo(RunStatus.Faulted);
   }

   [Observation]
   public void a_simulation_that_never_started_configuring_should_be_cancelled()
   {
      statusOf("UNSTARTED").ShouldBeEqualTo(RunStatus.Canceled);
   }
}

public abstract class concern_for_a_running_simulation_update : concern_for_SimulationUpdatePresenter
{
   protected MoBiSimulation _simulation;
   protected List<SimulationUpdateStatusDTO> _boundDTOs;

   protected override void Context()
   {
      base.Context();
      _simulation = new MoBiSimulation { Id = "SIM", Configuration = new SimulationConfiguration(), Model = new Model { Root = new Container() }.WithName("SIM_MODEL") };

      A.CallTo(() => _view.BindTo(A<IEnumerable<SimulationUpdateStatusDTO>>._))
         .Invokes((IEnumerable<SimulationUpdateStatusDTO> dtos) => _boundDTOs = dtos.ToList());

      //a task that never completes, so the results are not applied while we exercise the configuration events
      A.CallTo(() => _simulationUpdateTask.ConfigureSimulationsInParallel(A<IReadOnlyList<IMoBiSimulation>>._, A<CancellationToken>._))
         .Returns(new TaskCompletionSource<IReadOnlyList<ModelCreationAndValidationResult>>().Task);

      sut.Start([_simulation]);
   }

   protected RunStatus statusOfSimulation => _boundDTOs.Single(x => x.SimulationId == "SIM").Status;
}

public class When_a_simulation_configuration_starts : concern_for_a_running_simulation_update
{
   protected override void Because()
   {
      sut.Handle(new SimulationConfigurationStartedEvent(_simulation));
   }

   [Observation]
   public void should_show_the_simulation_as_updating()
   {
      statusOfSimulation.ShouldBeEqualTo(RunStatus.Running);
   }
}

public class When_a_simulation_configuration_succeeds : concern_for_a_running_simulation_update
{
   protected override void Because()
   {
      sut.Handle(new SimulationConfigurationSucceededEvent(_simulation));
   }

   [Observation]
   public void should_show_the_simulation_as_pending()
   {
      statusOfSimulation.ShouldBeEqualTo(RunStatus.Created);
   }
}

public class When_a_simulation_configuration_fails : concern_for_a_running_simulation_update
{
   protected override void Because()
   {
      sut.Handle(new SimulationConfigurationFailedEvent(_simulation));
   }

   [Observation]
   public void should_show_the_simulation_as_faulted()
   {
      statusOfSimulation.ShouldBeEqualTo(RunStatus.Faulted);
   }
}