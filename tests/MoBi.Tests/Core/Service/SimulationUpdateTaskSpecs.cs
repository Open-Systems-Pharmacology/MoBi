using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SimulationUpdateTask : ContextSpecification<SimulationUpdateTask>
   {
      protected IObjectPathFactory _objectPathFactory;
      protected IMoBiContext _context;
      private IMoBiApplicationController _applicationController;

      protected IEntityPathResolver _entityPathResolver;
      protected ICreateSimulationConfigurationPresenter _configurePresenter;
      protected ISimulationFactory _simulationFactory;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected IInteractionTasksForSimulation _interactionTasksForSimulation;
      protected ISimulationConfigurationFactory _simulationConfigurationFactory;

      protected override void Context()
      {
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _context = A.Fake<IMoBiContext>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _entityPathResolver = new EntityPathResolverForSpecs();
         _configurePresenter = A.Fake<ICreateSimulationConfigurationPresenter>();
         _simulationFactory = A.Fake<ISimulationFactory>();
         A.CallTo(() => _applicationController.Start<ICreateSimulationConfigurationPresenter>()).Returns(_configurePresenter);
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _interactionTasksForSimulation = A.Fake<IInteractionTasksForSimulation>();
         _simulationConfigurationFactory = A.Fake<ISimulationConfigurationFactory>();

         sut = new SimulationUpdateTask(_context, _applicationController, _simulationFactory, _cloneManager, _simulationConfigurationFactory);
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
         _moBiSimulation = new MoBiSimulation {Model = new Model {Root = new Container()}.WithName("OLD_MODEL")};
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
         _simulationToConfigure = new MoBiSimulation {Model = new Model {Root = new Container()}.WithName("OLD_MODEL")};
         _simulationToConfigure.AddOriginalQuantityValue(new OriginalQuantityValue {Path = "a path"});
         _simulationToConfigure.Configuration = new SimulationConfiguration();
         _model = new Model().WithName("NEW MODEL");
         var simulationBuilder = A.Fake<SimulationBuilder>();
         _entitySource =new SimulationEntitySource("SimPath", "BBName", "bbType", "moduleName", "sourcePath");
         A.CallTo(() => simulationBuilder.EntitySources).Returns(new List<SimulationEntitySource> {_entitySource});
         var creationResults = new CreationResult(_model, simulationBuilder);
         _model.Root = new Container();
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._, A<string>._)).Returns(creationResults);
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

   public class When_configuration_a_simulation : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulationToConfigure;
      private IModel _model;

      protected override void Context()
      {
         base.Context();
         _simulationToConfigure = new MoBiSimulation {Model = new Model {Root = new Container()}.WithName("OLD_MODEL")};
         _simulationToConfigure.AddOriginalQuantityValue(new OriginalQuantityValue {Path = "a path"});
         _simulationToConfigure.Configuration = new SimulationConfiguration();
         _model = new Model().WithName("NEW MODEL");
         _model.Root = new Container();

         var simulationBuilder = new SimulationBuilder(_simulationToConfigure.Configuration);
         var creationResults = new CreationResult(_model, simulationBuilder);
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._, A<string>._)).Returns(creationResults);
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