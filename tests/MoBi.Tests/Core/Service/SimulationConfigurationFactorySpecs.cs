using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public class concern_for_SimulationConfigurationFactory : ContextSpecification<SimulationConfigurationFactory>
   {
      protected ICoreCalculationMethodRepository _calculationMethodRepository;
      protected ICloneManagerForBuildingBlock _cloneManager;
      private IMoBiProjectRetriever _projectRetriever;
      protected SimulationSettings _clonedSimulationSettings;
      private MoBiProject _currentProject;
      protected CoreCalculationMethod _method3;
      protected CoreCalculationMethod _method2;
      protected CoreCalculationMethod _method1;
      protected ITemplateResolverTask _templateResolverTask;

      protected override void Context()
      {
         _calculationMethodRepository = new CoreCalculationMethodRepository();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _templateResolverTask = A.Fake<ITemplateResolverTask>();

         _clonedSimulationSettings = new SimulationSettings();
         _currentProject = new MoBiProject
         {
            SimulationSettings = new SimulationSettings()
         };

         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_currentProject);
         A.CallTo(() => _projectRetriever.Current).Returns(_currentProject);
         A.CallTo(() => _cloneManager.Clone(_currentProject.SimulationSettings)).Returns(_clonedSimulationSettings);

         _method1 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(_method1);
         _method2 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(_method2);
         _method3 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(_method3);

         sut = new SimulationConfigurationFactory(_calculationMethodRepository, _cloneManager, _projectRetriever, _templateResolverTask);
      }
   }

   public class When_creating_a_new_simulation_configuration : concern_for_SimulationConfigurationFactory
   {
      private SimulationConfiguration _simulationConfiguration;

      protected override void Because()
      {
         _simulationConfiguration = sut.Create();
      }

      [Observation]
      public void the_simulation_configuration_should_contain_the_original_calculation_methods()
      {
         _simulationConfiguration.AllCalculationMethods.ShouldOnlyContain(_method1, _method2, _method3);
      }

      [Observation]
      public void the_simulation_configuration_should_have_a_clone_of_project_default_simulation_settings()
      {
         _simulationConfiguration.SimulationSettings.ShouldBeEqualTo(_clonedSimulationSettings);
      }
   }

   public class When_creating_a_new_simulation_configuration_with_specified_settings : concern_for_SimulationConfigurationFactory
   {
      private SimulationConfiguration _simulationConfiguration;
      private SimulationSettings _existingSimulationSettings;
      protected override void Context()
      {
         base.Context();
         _existingSimulationSettings = new SimulationSettings();
      }

      protected override void Because()
      {
         _simulationConfiguration = sut.Create(_existingSimulationSettings);
      }

      [Observation]
      public void the_configuration_should_have_the_existing_settings()
      {
         _simulationConfiguration.SimulationSettings.ShouldBeEqualTo(_existingSimulationSettings);
      }
   }

   public class When_updating_a_configuration_from_building_blocks : concern_for_SimulationConfigurationFactory
   {
      private MoBiSimulation _simulationToUpdate;
      private MoBiProject _project;
      private SimulationConfiguration _templateSimulationConfiguration;
      private SimulationConfiguration _originalSimulationConfiguration;

      protected override void Context()
      {
         base.Context();
         _simulationToUpdate = new MoBiSimulation { Model = new Model { Root = new Container() }.WithName("OLD_MODEL") };
         _project = new MoBiProject();
         _originalSimulationConfiguration = new SimulationConfiguration { SimulationSettings = new SimulationSettings() };
         _clonedSimulationSettings = new SimulationSettings();

         A.CallTo(() => _cloneManager.Clone(_originalSimulationConfiguration.SimulationSettings)).Returns(_clonedSimulationSettings);


         configureProject();

         configureSimulationConfiguration(_originalSimulationConfiguration);
         _simulationToUpdate.Configuration = _originalSimulationConfiguration;

         configureContext();
      }

      // Configure context to provide the project building blocks for the simulation building blocks
      private void configureContext()
      {
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_simulationToUpdate.Configuration.Individual)).Returns(_project.IndividualsCollection.First());
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_simulationToUpdate.Configuration.ExpressionProfiles.First())).Returns(_project.ExpressionProfileCollection.First());
         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_simulationToUpdate.Configuration.ModuleConfigurations.First().Module)).Returns(_project.Modules.First());
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_simulationToUpdate.Configuration.ModuleConfigurations.First().SelectedInitialConditions)).Returns(_project.Modules.First().InitialConditionsCollection.First());
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_simulationToUpdate.Configuration.ModuleConfigurations.First().SelectedParameterValues)).Returns(_project.Modules.First().ParameterValuesCollection.First());
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
         _templateSimulationConfiguration = sut.CreateFromProjectTemplatesBasedOn(_simulationToUpdate.Configuration);
      }

      [Observation]
      public void the_updated_simulation_should_contain_new_building_blocks_cloned_from_project()
      {
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
}