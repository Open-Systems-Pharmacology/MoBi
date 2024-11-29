using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_InteractionTasksForSimulation : ContextSpecification<InteractionTasksForSimulation>
   {
      protected IEditTasksForSimulation _editTask;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IMoBiApplicationController _applicationController;
      protected IDialogCreator _dialogCreator;
      protected ISimulationReferenceUpdater _simulationReferenceUpdater;
      protected ISimulationFactory _simulationFactory;
      protected IMoBiContext _moBiContext;
      protected IndividualBuildingBlock _individualBuildingBlock;
      protected MoBiProject _moBiProject;
      protected ITemplateResolverTask _templateResolver;
      protected ICloneManagerForSimulation _cloneManager;

      protected override void Context()
      {
         _cloneManager = A.Fake<ICloneManagerForSimulation>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTasksForSimulation>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => _interactionTaskContext.ApplicationController).Returns(_applicationController);
         A.CallTo(() => _interactionTaskContext.DialogCreator).Returns(_dialogCreator);
         _simulationReferenceUpdater = A.Fake<ISimulationReferenceUpdater>();
         _simulationFactory = A.Fake<ISimulationFactory>();
         _moBiContext = A.Fake<IMoBiContext>();
         _moBiProject = new MoBiProject
         {
            SimulationSettings = new SimulationSettings()
         };
         _individualBuildingBlock = new IndividualBuildingBlock().WithName("common individual");
         _moBiProject.AddIndividualBuildingBlock(_individualBuildingBlock);
         A.CallTo(() => _moBiContext.CurrentProject).Returns(_moBiProject);
         A.CallTo(() => _interactionTaskContext.Context).Returns(_moBiContext);
         
         _templateResolver = A.Fake<ITemplateResolverTask>();

         sut = new InteractionTasksForSimulation(_interactionTaskContext, _editTask, _simulationReferenceUpdater, _simulationFactory, _templateResolver, _cloneManager);
      }
   }

   public class When_cloning_a_simulation_and_rename_is_canceled : concern_for_InteractionTasksForSimulation
   {
      private IMoBiSimulation _originalSimulation;
      private IMoBiSimulation _clonedSimulation;
      protected override void Context()
      {
         base.Context();
         _originalSimulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration()
         };
         _clonedSimulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration()
         };

         A.CallTo(() => _interactionTaskContext.InteractionTask.PromptForNewName(A<IObjectBase>._, A<IEnumerable<string>>._)).Returns(string.Empty);
         A.CallTo(() => _cloneManager.CloneSimulation(_originalSimulation)).Returns(_clonedSimulation);
         A.CallTo(() => _interactionTaskContext.Context.CurrentProject).Returns(_moBiProject);
      }

      protected override void Because()
      {
         sut.CloneSimulation(_originalSimulation);
      }

      [Observation]
      public void the_new_simulation_is_added_to_the_project_by_command()
      {
         _moBiProject.Simulations.ShouldNotContain(_clonedSimulation);
      }
   }

   public class When_cloning_a_simulation_with_rename : concern_for_InteractionTasksForSimulation
   {
      private IMoBiSimulation _originalSimulation;
      private IMoBiSimulation _clonedSimulation;
      protected override void Context()
      {
         base.Context();
         _originalSimulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration()
         };
         _clonedSimulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration(),
            Model = new Model
            {
               Root = new Container(),
               Neighborhoods = new Container()
            }
         };

         A.CallTo(() => _interactionTaskContext.InteractionTask.PromptForNewName(A<IMoBiSimulation>._, A<IEnumerable<string>>._)).Returns("new name");
         A.CallTo(() => _cloneManager.CloneSimulation(_originalSimulation)).Returns(_clonedSimulation);
         A.CallTo(() => _interactionTaskContext.Context.CurrentProject).Returns(_moBiProject);
      }

      protected override void Because()
      {
         sut.CloneSimulation(_originalSimulation);
      }

      [Observation]
      public void the_name_corrector_should_be_used_to_check_for_existing_names()
      {
         A.CallTo(() => _interactionTaskContext.InteractionTask.PromptForNewName(_originalSimulation, A<IEnumerable<string>>._)).MustHaveHappened();
      }

      [Observation]
      public void the_new_simulation_is_not_added_to_the_project_by_command()
      {
         _moBiProject.Simulations.ShouldNotContain(_clonedSimulation);
      }

      [Observation]
      public void the_cloned_simulation_should_be_renamed()
      {
         _clonedSimulation.Name.ShouldBeEqualTo("new name");
         _clonedSimulation.Model.Name.ShouldBeEqualTo("new name");
         _clonedSimulation.Model.Root.Name.ShouldBeEqualTo("new name");
      }

      [Observation]
      public void the_model_rename_command_is_used_to_rename_the_model()
      {
         A.CallTo(() => _interactionTaskContext.Context.PromptForCancellation(A<ICommand>.That.Matches(x => isModelRenameCommand(x)))).MustHaveHappened();
      }

      private bool isModelRenameCommand(ICommand command)
      {
         return command is RenameModelCommand;
      }
   }

   public class When_removing_multiple_simulations_from_project_but_canceling : concern_for_InteractionTasksForSimulation
   {
      private IReadOnlyList<IMoBiSimulation> _simulations;
      private IMoBiCommand _result;

      protected override void Context()
      {
         base.Context();
         _simulations = new List<IMoBiSimulation> { A.Fake<IMoBiSimulation>(), A.Fake<IMoBiSimulation>() };
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.No);
      }

      protected override void Because()
      {
         _result = sut.RemoveMultipleSimulations(_simulations);
      }

      [Observation]
      public void must_not_remove_simulations_from_parameter_identifications()
      {
         A.CallTo(() => _simulationReferenceUpdater.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(A<ISimulation>._)).MustNotHaveHappened();
      }

      [Observation]
      public void returns_the_correct_number_of_commands()
      {
         _result.IsEmpty().ShouldBeTrue();
      }
   }

   public class When_removing_multiple_simulations_from_project : concern_for_InteractionTasksForSimulation
   {
      private IReadOnlyList<IMoBiSimulation> _simulations;
      private MoBiMacroCommand _result;

      protected override void Context()
      {
         base.Context();
         _simulations = new List<IMoBiSimulation> { A.Fake<IMoBiSimulation>(), A.Fake<IMoBiSimulation>() };
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         _result = sut.RemoveMultipleSimulations(_simulations) as MoBiMacroCommand;
      }

      [Observation]
      public void should_remove_all_simulations_from_parameter_identifications()
      {
         A.CallTo(() => _simulationReferenceUpdater.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(_simulations[0])).MustHaveHappened();
         A.CallTo(() => _simulationReferenceUpdater.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(_simulations[1])).MustHaveHappened();
      }

      [Observation]
      public void the_commands_should_be_remove_simulation_type()
      {
         _result.All().Each(command => command.ShouldBeAnInstanceOf<RemoveSimulationCommand>());
      }

      [Observation]
      public void returns_the_correct_number_of_commands()
      {
         _result.Count.ShouldBeEqualTo(_simulations.Count);
      }
   }

   internal class When_adding_a_new_simulation : concern_for_InteractionTasksForSimulation
   {
      private ICreateSimulationConfigurationPresenter _presenter;
      private IMoBiSimulation _simulation;
      private SimulationConfiguration _simulationConfiguration;
      private IMoBiSimulation _configuredSimulation;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ICreateSimulationConfigurationPresenter>();
         _simulation = new MoBiSimulation();
         _simulation.Configuration = new SimulationConfiguration();
         _configuredSimulation = new MoBiSimulation();
         _simulationConfiguration = new SimulationConfiguration();
         _configuredSimulation.Configuration = new SimulationConfiguration();
         _configuredSimulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module()));
         A.CallTo(() => _presenter.CreateBasedOn(_simulation, true)).Returns(_simulationConfiguration);
         A.CallTo(() => _applicationController.Start<ICreateSimulationConfigurationPresenter>()).Returns(_presenter);
         A.CallTo(() => _applicationController.PresenterFor(_simulation)).Returns(A.Fake<IEditSimulationPresenter>());
         A.CallTo(() => _simulationFactory.Create()).Returns(_simulation);
         A.CallTo(() => _simulationFactory.CreateSimulationAndValidate(_simulationConfiguration, A<string>._)).Returns(_configuredSimulation);
      }

      protected override void Because()
      {
         sut.CreateSimulation();
      }

      [Observation]
      public void the_configured_simulation_modules_should_have_import_version_set()
      {
         _configuredSimulation.Modules.All(x => !string.IsNullOrEmpty(x.ModuleImportVersion)).ShouldBeTrue();
      }

      [Observation]
      public void the_configuration_should_use_the_default_individual_for_the_project()
      {
         _simulation.Configuration.Individual.ShouldBeEqualTo(_individualBuildingBlock);
      }

      [Observation]
      public void should_get_create_presenter_from_application_controller()
      {
         A.CallTo(() => _applicationController.Start<ICreateSimulationConfigurationPresenter>()).MustHaveHappened();
      }

      [Observation]
      public void should_start_edit_action_for_the_new_simulation()
      {
         A.CallTo(() => _editTask.Edit(_configuredSimulation)).MustHaveHappened();
      }
   }

   public class When_finding_modules_with_changes_from_simulation : concern_for_InteractionTasksForSimulation
   {
      private IMoBiSimulation _simulation;
      private Module _module;
      private IndividualBuildingBlock _simulationIndividual;
      private IEnumerable<Module> _changes;
      private Module _projectModule;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         _simulation.Configuration = new SimulationConfiguration();
         _simulationIndividual = new IndividualBuildingBlock();

         _projectModule = new Module
         {
            new IndividualBuildingBlock()
         };

         _module = new Module
         {
            _simulationIndividual
         };

         var moduleConfiguration = new ModuleConfiguration(_module);
         _simulation.Configuration.AddModuleConfiguration(moduleConfiguration);

         _projectModule.Add(new EventGroupBuildingBlock());

         A.CallTo(() => _templateResolver.TemplateModuleFor(_simulation.Modules.First())).Returns(_projectModule);
      }

      protected override void Because()
      {
         _changes = sut.FindChangedModules(_simulation);
      }

      [Observation]
      public void the_changes_should_include_the_individual_from_the_simulation()
      {
         _changes.ShouldContain(_simulation.Modules.First());
      }
   }

   public class When_finding_building_blocks_with_changes_from_simulation : concern_for_InteractionTasksForSimulation
   {
      private IMoBiSimulation _simulation;
      private Module _module;
      private IndividualBuildingBlock _simulationIndividual;
      private IEnumerable<IBuildingBlock> _changes;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         _simulation.Configuration = new SimulationConfiguration
         {
            SimulationSettings = new SimulationSettings { Version = 4 }
         };
         _simulationIndividual = new IndividualBuildingBlock
         {
            Version = 112
         };
         _module = new Module
         {
            _simulationIndividual
         };

         var moduleConfiguration = new ModuleConfiguration(_module);
         _simulation.Configuration.AddModuleConfiguration(moduleConfiguration);

         A.CallTo(() => _templateResolver.TemplateBuildingBlockFor(_simulationIndividual)).Returns(_individualBuildingBlock);
      }

      protected override void Because()
      {
         _changes = sut.FindChangedBuildingBlocks(_simulation);
      }

      [Observation]
      public void the_changes_should_include_the_individual_from_the_simulation()
      {
         _changes.ShouldContain(_simulationIndividual);
      }

      [Observation]
      public void the_changes_should_not_include_the_settings_from_the_simulation()
      {
         _changes.ShouldNotContain(_simulation.Settings);
      }
   }
}