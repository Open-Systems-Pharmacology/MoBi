using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
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

         sut = new SimulationUpdateTask(_context, _applicationController, _entityPathResolver, _simulationFactory, _cloneManager, _interactionTasksForSimulation, _simulationConfigurationFactory);
      }
   }

   public class When_updating_a_configuration_from_building_blocks : concern_for_SimulationUpdateTask
   {
      private MoBiSimulation _simulationToUpdate;
      private MoBiProject _project;
      private SimulationConfiguration _templateSimulationConfiguration, _clonedSimulationConfiguration;
      private SimulationConfiguration _originalSimulationConfiguration;
      private SimulationSettings _clonedSimulationSettings;

      protected override void Context()
      {
         base.Context();
         _simulationToUpdate = new MoBiSimulation { Model = new Model { Root = new Container() }.WithName("OLD_MODEL") };
         _project = new MoBiProject();
         _originalSimulationConfiguration = new SimulationConfiguration { SimulationSettings = new SimulationSettings() };
         _templateSimulationConfiguration = new SimulationConfiguration();
         _clonedSimulationConfiguration = new SimulationConfiguration();
         _clonedSimulationSettings = new SimulationSettings();

         A.CallTo(() => _cloneManager.Clone(_originalSimulationConfiguration.SimulationSettings)).Returns(_clonedSimulationSettings);

         A.CallTo(() => _simulationConfigurationFactory.Create()).Returns(_templateSimulationConfiguration);

         configureProject();
         A.CallTo(() => _cloneManager.Clone(_templateSimulationConfiguration)).Returns(_clonedSimulationConfiguration);

         configureSimulationConfiguration(_originalSimulationConfiguration);
         _simulationToUpdate.Configuration = _originalSimulationConfiguration;

         configureContext();
      }

      // Configure context to provide the project building blocks for the simulation building blocks
      private void configureContext()
      {
         A.CallTo(() => _interactionTasksForSimulation.TemplateBuildingBlockFor(_simulationToUpdate.Configuration.Individual)).Returns(_project.IndividualsCollection.First());
         A.CallTo(() => _interactionTasksForSimulation.TemplateBuildingBlockFor(_simulationToUpdate.Configuration.ExpressionProfiles.First())).Returns(_project.ExpressionProfileCollection.First());
         A.CallTo(() => _interactionTasksForSimulation.TemplateModuleFor(_simulationToUpdate.Configuration.ModuleConfigurations.First().Module)).Returns(_project.Modules.First());
         A.CallTo(() => _interactionTasksForSimulation.TemplateBuildingBlockFor(_simulationToUpdate.Configuration.ModuleConfigurations.First().SelectedInitialConditions)).Returns(_project.Modules.First().InitialConditionsCollection.First());
         A.CallTo(() => _interactionTasksForSimulation.TemplateBuildingBlockFor(_simulationToUpdate.Configuration.ModuleConfigurations.First().SelectedParameterValues)).Returns(_project.Modules.First().ParameterValuesCollection.First());
      }

      private void configureSimulationConfiguration(SimulationConfiguration simulationConfiguration)
      {
         simulationConfiguration.AddModuleConfiguration(createModuleConfiguration(createModule()));
         simulationConfiguration.Individual = createIndividual();
         simulationConfiguration.AddExpressionProfile(createExpressionProfile());
      }

      // Add project building blocks for each simulation building block
      private void configureProject()
      {
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         _project.AddModule(createModule());
         _project.AddIndividualBuildingBlock(createIndividual());
         _project.AddExpressionProfileBuildingBlock(createExpressionProfile());
      }

      private ExpressionProfileBuildingBlock createExpressionProfile()
      {
         return new ExpressionProfileBuildingBlock().WithName("EXPRESSION_PROFILE");
      }

      private IndividualBuildingBlock createIndividual()
      {
         return new IndividualBuildingBlock().WithName("INDIVIDUAL");
      }

      private ModuleConfiguration createModuleConfiguration(Module simulationModule)
      {
         return new ModuleConfiguration(simulationModule, simulationModule.InitialConditionsCollection.FirstOrDefault(), simulationModule.ParameterValuesCollection.FirstOrDefault());
      }

      private Module createModule()
      {
         var module = new Module().WithName("MODULE");
         module.Add(new SpatialStructure().WithName("SPATIAL_STRUCTURE"));
         module.Add(new ReactionBuildingBlock().WithName("REACTION"));
         module.Add(new InitialConditionsBuildingBlock().WithName("INITIAL_CONDITIONS"));
         module.Add(new ParameterValuesBuildingBlock().WithName("PARAMETER_VALUES"));

         return module;
      }

      protected override void Because()
      {
         sut.UpdateSimulation(_simulationToUpdate);
      }

      [Observation]
      public void the_updated_simulation_should_contain_new_building_blocks_cloned_from_project()
      {
         _simulationToUpdate.Configuration.ShouldBeEqualTo(_clonedSimulationConfiguration);
         var templateModuleConfiguration = _templateSimulationConfiguration.ModuleConfigurations.First();
         var templateModule = templateModuleConfiguration.Module;
         var projectModule = _project.Modules.Single();
         templateModule.ShouldBeEqualTo(projectModule);
         templateModuleConfiguration.SelectedInitialConditions.ShouldBeEqualTo(projectModule.InitialConditionsCollection.First());
         templateModuleConfiguration.SelectedParameterValues.ShouldBeEqualTo(projectModule.ParameterValuesCollection.First());
         _templateSimulationConfiguration.Individual.ShouldBeEqualTo(_project.IndividualsCollection.First());
         _templateSimulationConfiguration.ExpressionProfiles.First().ShouldBeEqualTo(_project.All<ExpressionProfileBuildingBlock>().First());
      }

      [Observation]
      public void the_simulation_settings_should_be_separately_cloned()
      {
         _templateSimulationConfiguration.SimulationSettings.ShouldBeEqualTo(_clonedSimulationSettings);
      }
   }

   public class When_configuration_a_simulation : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulationToConfigure;
      private IModel _model;

      protected override void Context()
      {
         base.Context();
         _simulationToConfigure = new MoBiSimulation { Model = new Model { Root = new Container() }.WithName("OLD_MODEL") };
         _simulationToConfigure.Configuration = new SimulationConfiguration();
         _model = new Model().WithName("NEW MODEL");
         _model.Root = new Container();
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<SimulationConfiguration>._, A<string>._, A<string>._)).Returns(_model);
      }

      protected override void Because()
      {
         sut.ConfigureSimulation(_simulationToConfigure);
      }

      [Observation]
      public void should_start_the_configure_workflow_for_the_user()
      {
         A.CallTo(() => _configurePresenter.CreateBasedOn(_simulationToConfigure, false)).MustHaveHappened();
      }

      [Observation]
      public void should_create_a_new_simulation_using_the_build_configuration_setup_by_the_user()
      {
         _simulationToConfigure.Model.ShouldBeEqualTo(_model);
      }
   }
}