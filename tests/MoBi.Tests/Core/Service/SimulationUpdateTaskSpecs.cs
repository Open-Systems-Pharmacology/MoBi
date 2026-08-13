using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SimulationUpdateTask : ContextSpecification<SimulationUpdateTask>
   {
      protected IMoBiContext _context;
      private IMoBiApplicationController _applicationController;

      protected ICreateSimulationConfigurationPresenter _configurePresenter;
      protected ISimulationFactory _simulationFactory;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected ISimulationConfigurationFactory _simulationConfigurationFactory;
      private IConcurrencyManager _concurrencyManager;
      private ICoreUserSettings _coreUserSettings;
      protected IRemovedNeighborhoodsDialogTask _removedNeighborhoodsDialogTask;
      private SynchronizationContext _originalSynchronizationContext;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _configurePresenter = A.Fake<ICreateSimulationConfigurationPresenter>();
         _simulationFactory = A.Fake<ISimulationFactory>();
         A.CallTo(() => _applicationController.Start<ICreateSimulationConfigurationPresenter>()).Returns(_configurePresenter);
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _simulationConfigurationFactory = A.Fake<ISimulationConfigurationFactory>();
         _coreUserSettings = A.Fake<ICoreUserSettings>();

         _concurrencyManager = new ConcurrencyManagerForSpecs();

         A.CallTo(() => _context.Resolve<ISimulationConfigurationFactory>()).Returns(_simulationConfigurationFactory);
         A.CallTo(() => _context.Resolve<ISimulationFactory>()).Returns(_simulationFactory);
         A.CallTo(() => _context.Resolve<ICloneManagerForBuildingBlock>()).Returns(_cloneManager);

         _originalSynchronizationContext = SynchronizationContext.Current;
         SynchronizationContext.SetSynchronizationContext(new SynchronousSynchronizationContextForSpecs());

         _removedNeighborhoodsDialogTask = A.Fake<IRemovedNeighborhoodsDialogTask>();

         sut = new SimulationUpdateTask(_context, _applicationController, _simulationFactory, _cloneManager, createHeavyWorkManager(), _concurrencyManager, _coreUserSettings, _removedNeighborhoodsDialogTask);
      }

      protected virtual IHeavyWorkManager createHeavyWorkManager() => new HeavyWorkManagerForSpecs();

      public override void Cleanup()
      {
         base.Cleanup();
         SynchronizationContext.SetSynchronizationContext(_originalSynchronizationContext);
      }
   }

   public class When_updating_simulation_settings : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulation;
      private SimulationSettings _clonedSimulationSettings;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         _simulation.Configuration = new SimulationConfiguration
         {
            SimulationSettings = new SimulationSettings()
         };

         _clonedSimulationSettings = new SimulationSettings();
         A.CallTo(() => _cloneManager.Clone(_context.CurrentProject.SimulationSettings)).Returns(_clonedSimulationSettings);
      }

      protected override void Because()
      {
         sut.UpdateSimulationSolverAndSchema(_simulation);
      }

      [Observation]
      public void the_simulation_should_have_a_new_simulation_settings_object()
      {
         _simulation.Configuration.SimulationSettings.Solver.ShouldBeEqualTo(_clonedSimulationSettings.Solver);
         _simulation.Configuration.SimulationSettings.OutputSchema.ShouldBeEqualTo(_clonedSimulationSettings.OutputSchema);
      }

      [Observation]
      public void the_event_should_be_published()
      {
         A.CallTo(() => _context.PublishEvent(A<SimulationReloadEvent>.That.Matches(x => x.Simulation.Equals(_simulation)))).MustHaveHappened();
      }
   }

   public class When_updating_a_simulation : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _moBiSimulation;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         _moBiSimulation = new MoBiSimulation { Model = new Model { Root = new Container() }.WithName("OLD_MODEL") };
         _simulationConfiguration = new SimulationConfiguration();
         _moBiSimulation.Configuration = _simulationConfiguration;
      }

      protected override void Because()
      {
         sut.UpdateSimulation(_moBiSimulation);
      }

      [Observation]
      public void the_simulation_configuration_factory_creates_the_new_configuration()
      {
         A.CallTo(() => _simulationConfigurationFactory.CreateFromProjectTemplatesBasedOn(_simulationConfiguration)).MustHaveHappened();
      }

      [Observation]
      public void the_notification_area_should_be_cleared()
      {
         A.CallTo(() => _context.PublishEvent(A<ClearNotificationsEvent>.That.Matches(x => x.MessageOrigin.Equals(MessageOrigin.Simulation)))).MustHaveHappened();
      }

      [Observation]
      public void the_update_should_use_the_validating_create_path_that_throws_on_invalid()
      {
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._, true)).MustHaveHappened();
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._, false)).MustNotHaveHappened();
      }
   }

   public class When_configuring_a_simulation_fails : concern_for_SimulationUpdateTask
   {
      private SimulationConfiguration _simulationConfiguration;
      private MoBiSimulation _simulation;
      private ICommand _result;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _simulation = new MoBiSimulation
         {
            Configuration = _simulationConfiguration,
            Model = new Model()
         };

         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._)).Returns(null);
      }

      protected override void Because()
      {
         _result = sut.UpdateSimulation(_simulation);
      }

      [Observation]
      public void the_command_to_update_should_be_empty()
      {
         (_result as MoBiMacroCommand).Count.ShouldBeEqualTo(0);
      }
   }

   public class When_configuring_several_simulations_from_their_building_blocks_in_parallel : concern_for_SimulationUpdateTask
   {
      private MoBiSimulation _successfulSimulation;
      private MoBiSimulation _failingSimulation;
      private IReadOnlyList<ModelCreationAndValidationResult> _results;

      protected override void Context()
      {
         base.Context();
         _successfulSimulation = new MoBiSimulation { Id = "GOOD", Configuration = new SimulationConfiguration(), Model = new Model { Root = new Container() }.WithName("GOOD_MODEL") };
         _failingSimulation = new MoBiSimulation { Id = "BAD", Configuration = new SimulationConfiguration(), Model = new Model { Root = new Container() }.WithName("BAD_MODEL") };

         var model = new Model { Root = new Container() }.WithName("NEW_MODEL");
         var simulationBuilder = A.Fake<SimulationBuilder>();
         A.CallTo(() => simulationBuilder.EntitySources).Returns(new List<SimulationEntitySource>());
         var creationResult = new CreationResult(model, simulationBuilder);

         //CreateModelAndValidate(throwOnInvalid: false) returns a non-null invalid result when the model can't be created
         var invalidCreationResult = A.Fake<CreationResult>();
         A.CallTo(() => invalidCreationResult.IsInvalid).Returns(true);

         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, "GOOD_MODEL", false)).Returns(creationResult);
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, "BAD_MODEL", false)).Returns(invalidCreationResult);
      }

      protected override void Because()
      {
         _results = sut.ConfigureSimulationsInParallel([_successfulSimulation, _failingSimulation], CancellationToken.None).Result;
      }

      [Observation]
      public void should_return_a_valid_configuration_result_for_the_simulation_that_could_be_configured()
      {
         _results.Single(x => x.Simulation == _successfulSimulation).IsValid.ShouldBeTrue();
      }

      [Observation]
      public void should_return_an_invalid_configuration_result_for_the_simulation_that_could_not_be_configured()
      {
         _results.Single(x => x.Simulation == _failingSimulation).IsValid.ShouldBeFalse();
      }
   }

   public class When_configuring_and_adding_a_simulation : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulationToConfigure;
      private IModel _model;
      private MoBiProject _moBiProject;
      private SimulationEntitySource _entitySource;

      protected override void Context()
      {
         base.Context();
         _moBiProject = new MoBiProject();
         _simulationToConfigure = new MoBiSimulation { Model = new Model { Root = new Container() }.WithName("OLD_MODEL") };
         _simulationToConfigure.AddOriginalQuantityValue(new OriginalQuantityValue { Path = "a path" });
         _simulationToConfigure.Configuration = new SimulationConfiguration();
         _model = new Model().WithName("NEW MODEL");
         var simulationBuilder = A.Fake<SimulationBuilder>();
         _entitySource = new SimulationEntitySource("SimPath", "BBName", "bbType", "moduleName", "sourcePath");
         A.CallTo(() => simulationBuilder.EntitySources).Returns(new List<SimulationEntitySource> { _entitySource });
         var creationResults = new CreationResult(_model, simulationBuilder);
         _model.Root = new Container();
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._)).Returns(creationResults);
         A.CallTo(() => _context.CurrentProject).Returns(_moBiProject);
      }

      protected override void Because()
      {
         sut.ConfigureSimulationAndAddToProject(_simulationToConfigure);
      }

      [Observation]
      public void the_simulation_is_added_to_the_project()
      {
         _moBiProject.Simulations.ShouldContain(_simulationToConfigure);
      }

      [Observation]
      public void the_notification_area_should_be_cleared()
      {
         A.CallTo(() => _context.PublishEvent(A<ClearNotificationsEvent>.That.Matches(x => x.MessageOrigin.Equals(MessageOrigin.Simulation)))).MustHaveHappened();
      }

      [Observation]
      public void should_start_the_configure_workflow_for_the_user()
      {
         A.CallTo(() => _configurePresenter.CreateBasedOn(_simulationToConfigure, false)).MustHaveHappened();
      }

      [Observation]
      public void the_original_quantities_should_be_cleared()
      {
         _simulationToConfigure.OriginalQuantityValues.ShouldBeEmpty();
      }

      [Observation]
      public void should_create_a_new_simulation_using_the_build_configuration_setup_by_the_user()
      {
         _simulationToConfigure.Model.ShouldBeEqualTo(_model);
      }

      [Observation]
      public void should_update_the_entity_source_in_the_simulation()
      {
         _simulationToConfigure.EntitySources.ShouldContain(_entitySource);
      }
   }

   public class When_configuring_and_adding_a_simulation_whose_model_cannot_be_created : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _clonedSimulation;
      private MoBiProject _moBiProject;
      private ICommand _result;

      protected override IHeavyWorkManager createHeavyWorkManager() => new ExceptionSwallowingHeavyWorkManagerForSpecs();

      protected override void Context()
      {
         base.Context();
         _moBiProject = new MoBiProject();
         _clonedSimulation = new MoBiSimulation
         {
            Model = new Model { Root = new Container() }.WithName("CLONED_MODEL"),
            Configuration = new SimulationConfiguration()
         };

         A.CallTo(() => _context.CurrentProject).Returns(_moBiProject);
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._, true))
            .Throws(new ValidationFailedMoBiException("Could not create simulation", new ValidationResult()));
      }

      protected override void Because()
      {
         _result = sut.ConfigureSimulationAndAddToProject(_clonedSimulation);
      }

      [Observation]
      public void the_simulation_should_not_be_added_to_the_project()
      {
         _moBiProject.Simulations.ShouldNotContain(_clonedSimulation);
      }

      [Observation]
      public void the_returned_command_should_be_empty()
      {
         _result.IsEmpty().ShouldBeTrue();
      }
   }

   public class When_configuring_and_adding_a_simulation_and_the_user_cancels_the_configuration : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _clonedSimulation;
      private MoBiProject _moBiProject;
      private ICommand _result;

      protected override void Context()
      {
         base.Context();
         _moBiProject = new MoBiProject();
         _clonedSimulation = new MoBiSimulation
         {
            Model = new Model { Root = new Container() }.WithName("CLONED_MODEL"),
            Configuration = new SimulationConfiguration()
         };

         A.CallTo(() => _context.CurrentProject).Returns(_moBiProject);
         A.CallTo(() => _configurePresenter.CreateBasedOn(_clonedSimulation, false)).Returns(null);
      }

      protected override void Because()
      {
         _result = sut.ConfigureSimulationAndAddToProject(_clonedSimulation);
      }

      [Observation]
      public void the_simulation_should_not_be_added_to_the_project()
      {
         _moBiProject.Simulations.ShouldNotContain(_clonedSimulation);
      }

      [Observation]
      public void the_returned_command_should_be_empty()
      {
         _result.IsEmpty().ShouldBeTrue();
      }
   }

   public class When_configuration_a_simulation : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulationToConfigure;
      private IModel _model;
      private IContainerMergeTask _containerMergeTask;
      private ICloneManagerForModel _cloneManagerForModel;

      protected override void Context()
      {
         base.Context();
         _containerMergeTask = new ContainerMergeTask();
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _simulationToConfigure = new MoBiSimulation { Model = new Model { Root = new Container() }.WithName("OLD_MODEL") };
         _simulationToConfigure.AddOriginalQuantityValue(new OriginalQuantityValue { Path = "a path" });
         _simulationToConfigure.Configuration = new SimulationConfiguration();
         _model = new Model().WithName("NEW MODEL");
         _model.Root = new Container();

         var simulationBuilder = new SimulationBuilder(_cloneManagerForModel, _containerMergeTask);
         var creationResults = new CreationResult(_model, simulationBuilder);
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._)).Returns(creationResults);
      }

      protected override void Because()
      {
         sut.ConfigureSimulation(_simulationToConfigure);
      }

      [Observation]
      public void the_notification_area_should_be_cleared()
      {
         A.CallTo(() => _context.PublishEvent(A<ClearNotificationsEvent>.That.Matches(x => x.MessageOrigin.Equals(MessageOrigin.Simulation)))).MustHaveHappened();
      }

      [Observation]
      public void should_start_the_configure_workflow_for_the_user()
      {
         A.CallTo(() => _configurePresenter.CreateBasedOn(_simulationToConfigure, false)).MustHaveHappened();
      }

      [Observation]
      public void the_original_quantities_should_be_cleared()
      {
         _simulationToConfigure.OriginalQuantityValues.ShouldBeEmpty();
      }

      [Observation]
      public void should_create_a_new_simulation_using_the_build_configuration_setup_by_the_user()
      {
         _simulationToConfigure.Model.ShouldBeEqualTo(_model);
      }
   }
}