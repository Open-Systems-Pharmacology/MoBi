using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_SimulationCommitTask : ContextSpecification<SimulationCommitTask>
   {
      protected MoBiSimulation _simulationWithChanges;
      protected IInteractionTaskContext _interactionTaskContext;
      protected INameCorrector _nameCorrector;
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      protected ITemplateResolverTask _templateResolverTask;
      protected IMoBiContext _context;
      protected ModuleConfiguration _moduleConfiguration;
      protected Module _module;
      protected ISelectCommitTargetPresenter _selectCommitTargetPresenter;

      protected override void Context()
      {
         base.Context();
         _simulationWithChanges = new MoBiSimulation();
         _module = new Module();
         _moduleConfiguration = new ModuleConfiguration(_module);
         _simulationWithChanges.Configuration = new SimulationConfiguration();
         _simulationWithChanges.Configuration.AddModuleConfiguration(_moduleConfiguration);

         _context = A.Fake<IMoBiContext>();
         _templateResolverTask = A.Fake<ITemplateResolverTask>();
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _nameCorrector = A.Fake<INameCorrector>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _selectCommitTargetPresenter = A.Fake<ISelectCommitTargetPresenter>();
         A.CallTo(() => _context.Resolve<ISelectCommitTargetPresenter>()).Returns(_selectCommitTargetPresenter);

         // the user accepts the defaults: last module and its selected parameter values building block or a new one
         A.CallTo(() => _selectCommitTargetPresenter.SelectCommitTargetFor(_simulationWithChanges)).ReturnsLazily(() =>
         {
            var lastModuleConfiguration = _simulationWithChanges.Configuration.ModuleConfigurations.Last();
            return new CommitTarget(lastModuleConfiguration, lastModuleConfiguration.SelectedParameterValues == null ? null : _templateResolverTask.TemplateBuildingBlockFor(lastModuleConfiguration.SelectedParameterValues));
         });

         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, A<ViewResult>._)).Returns(ViewResult.Yes);
         sut = new SimulationCommitTask(_context, _templateResolverTask, _entitiesInSimulationRetriever, _nameCorrector, new ObjectTypeResolver(), _interactionTaskContext);
      }
   }

   public class When_committing_from_a_simulation_with_untraceable_changes : concern_for_SimulationCommitTask
   {
      private IMoBiCommand _commands;

      protected override void Context()
      {
         base.Context();
         _simulationWithChanges.HasUntraceableChanges = true;
      }

      protected override void Because()
      {
         _commands = sut.CommitSimulationChanges(_simulationWithChanges);
      }

      [Observation]
      public void a_dialog_reminds_user_about_the_project_conversion()
      {
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxError(
               AppConstants.Captions.SimulationHasChangesThatCannotBeCommitted(_simulationWithChanges.Name)))
            .MustHaveHappened();
      }

      [Observation]
      public void the_commands_must_be_empty()
      {
         _commands.ShouldBeAnInstanceOf<MoBiEmptyCommand>();
      }
   }

   public class When_committing_from_a_simulation_without_any_changes : concern_for_SimulationCommitTask
   {
      private Module _projectModule;

      protected override void Context()
      {
         base.Context();

         _projectModule = new Module();
         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_module)).Returns(_projectModule);
         var parameterPathCache = new PathCacheForSpecs<Parameter>
         {
            new Parameter { Name = "name" }
         };

         var moleculeAmount = new MoleculeAmount { Name = "name" };
         new Container().WithName("top").Add(moleculeAmount);
         var initialConditionsPathCache = new PathCacheForSpecs<MoleculeAmount>
         {
            moleculeAmount
         };

         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<Parameter>(_simulationWithChanges)).Returns(parameterPathCache);
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<MoleculeAmount>(_simulationWithChanges)).Returns(initialConditionsPathCache);
      }

      protected override void Because()
      {
         sut.CommitSimulationChanges(_simulationWithChanges);
      }

      [Observation]
      public void new_building_blocks_are_created()
      {
         _projectModule.ParameterValuesCollection.Count.ShouldBeEqualTo(0);
         _projectModule.InitialConditionsCollection.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_committing_to_configuration_with_selected_building_blocks : concern_for_SimulationCommitTask
   {
      protected Module _projectModule;
      protected InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      protected ParameterValuesBuildingBlock _parameterValuesBuildingBlock;
      protected InitialConditionsBuildingBlock _projectInitialConditions;
      protected ParameterValuesBuildingBlock _projectParameterValues;

      protected override void Context()
      {
         base.Context();
         _simulationWithChanges.AddOriginalQuantityValue(new OriginalQuantityValue { Path = new ObjectPath("name"), Value = 1.0 });
         _simulationWithChanges.AddOriginalQuantityValue(new OriginalQuantityValue { Path = new ObjectPath("top", "name"), Value = 1.0 });
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock();
         _module.Add(_initialConditionsBuildingBlock);
         _module.Add(_parameterValuesBuildingBlock);
         _moduleConfiguration.SelectedInitialConditions = _initialConditionsBuildingBlock;
         _moduleConfiguration.SelectedParameterValues = _parameterValuesBuildingBlock;

         _projectInitialConditions = new InitialConditionsBuildingBlock();
         _projectParameterValues = new ParameterValuesBuildingBlock();
         _projectModule = new Module
         {
            _projectInitialConditions,
            _projectParameterValues
         };

         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_module)).Returns(_projectModule);
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_initialConditionsBuildingBlock)).Returns(_projectInitialConditions);
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_parameterValuesBuildingBlock)).Returns(_projectParameterValues);
         var parameterPathCache = new PathCacheForSpecs<Parameter>
         {
            new Parameter { Name = "name" }
         };

         var moleculeAmount = new MoleculeAmount { Name = "name" };
         new Container().WithName("top").Add(moleculeAmount);
         var initialConditionsPathCache = new PathCacheForSpecs<MoleculeAmount>
         {
            moleculeAmount
         };

         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<DistributedParameter>(_simulationWithChanges)).Returns(new PathCacheForSpecs<DistributedParameter>());
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<Parameter>(_simulationWithChanges)).Returns(parameterPathCache);
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<MoleculeAmount>(_simulationWithChanges)).Returns(initialConditionsPathCache);
      }

      protected override void Because()
      {
         sut.CommitSimulationChanges(_simulationWithChanges);
      }
   }

   public class When_committing_to_configuration_with_selected_building_blocks_and_matching_start_values : When_committing_to_configuration_with_selected_building_blocks
   {
      protected override void Context()
      {
         base.Context();
         _projectInitialConditions.Add(new InitialCondition
         {
            Path = new ObjectPath("top", "name")
         });

         _projectParameterValues.Add(new ParameterValue
         {
            Path = new ObjectPath("name")
         });
      }

      [Observation]
      public void new_building_blocks_are_not_created()
      {
         _projectModule.ParameterValuesCollection.Count.ShouldBeEqualTo(1);
         _projectModule.InitialConditionsCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_original_quantity_value_tracker_is_cleared()
      {
         _simulationWithChanges.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void new_start_values_are_updated_not_created()
      {
         _projectModule.InitialConditionsCollection.First().Count().ShouldBeEqualTo(1);
         _projectModule.ParameterValuesCollection.First().Count().ShouldBeEqualTo(1);
      }
   }

   public class When_committing_to_configuration_with_selected_building_blocks_but_no_matching_start_values : When_committing_to_configuration_with_selected_building_blocks
   {
      [Observation]
      public void new_building_blocks_are_not_created()
      {
         _projectModule.ParameterValuesCollection.Count.ShouldBeEqualTo(1);
         _projectModule.InitialConditionsCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void new_project_start_values_are_created_when_needed()
      {
         _projectModule.InitialConditionsCollection.First().Count().ShouldBeEqualTo(1);
         _projectModule.ParameterValuesCollection.First().Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void new_simulation_start_values_are_created_when_needed()
      {
         _moduleConfiguration.SelectedInitialConditions.Count().ShouldBeEqualTo(1);
         _moduleConfiguration.SelectedParameterValues.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_original_quantity_value_tracker_is_cleared()
      {
         _simulationWithChanges.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_committing_to_configuration_without_selected_start_values : concern_for_SimulationCommitTask
   {
      private Module _projectModule;

      protected override void Context()
      {
         base.Context();
         _simulationWithChanges.AddOriginalQuantityValue(new OriginalQuantityValue { Path = new ObjectPath("name"), Value = 1.0 });
         _simulationWithChanges.AddOriginalQuantityValue(new OriginalQuantityValue { Path = new ObjectPath("top", "name"), Value = 1.0 });

         _projectModule = new Module();
         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_module)).Returns(_projectModule);
         var parameterPathCache = new PathCacheForSpecs<Parameter>
         {
            new Parameter { Name = "name" }
         };

         var moleculeAmount = new MoleculeAmount { Name = "name" };
         new Container().WithName("top").Add(moleculeAmount);
         var initialConditionsPathCache = new PathCacheForSpecs<MoleculeAmount>
         {
            moleculeAmount
         };

         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<Parameter>(_simulationWithChanges)).Returns(parameterPathCache);
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<DistributedParameter>(_simulationWithChanges)).Returns(new PathCacheForSpecs<DistributedParameter>());
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<MoleculeAmount>(_simulationWithChanges)).Returns(initialConditionsPathCache);
      }

      protected override void Because()
      {
         sut.CommitSimulationChanges(_simulationWithChanges);
      }

      [Observation]
      public void new_project_building_blocks_are_created()
      {
         _projectModule.ParameterValuesCollection.Count.ShouldBeEqualTo(1);
         _projectModule.InitialConditionsCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void new_simulation_building_blocks_are_created()
      {
         _moduleConfiguration.Module.InitialConditionsCollection.Count.ShouldBeEqualTo(1);
         _moduleConfiguration.Module.ParameterValuesCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void new_simulation_building_blocks_are_selected()
      {
         _moduleConfiguration.SelectedInitialConditions.ShouldNotBeNull();
         _moduleConfiguration.SelectedParameterValues.ShouldNotBeNull();
      }

      [Observation]
      public void the_original_quantity_value_tracker_is_cleared()
      {
         _simulationWithChanges.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void the_user_is_not_asked_to_select_the_commit_target_because_there_is_no_choice()
      {
         A.CallTo(() => _selectCommitTargetPresenter.SelectCommitTargetFor(_simulationWithChanges)).MustNotHaveHappened();
      }
   }

   public class When_committing_changes_to_a_simulation_with_multiple_module_configurations : concern_for_SimulationCommitTask
   {
      protected Module _lastModule;
      protected ModuleConfiguration _lastModuleConfiguration;
      protected Module _projectModule;

      protected override void Context()
      {
         base.Context();
         _lastModule = new Module();
         _lastModuleConfiguration = new ModuleConfiguration(_lastModule);
         _simulationWithChanges.Configuration.AddModuleConfiguration(_lastModuleConfiguration);

         _projectModule = new Module();
         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_module)).Returns(_projectModule);

         var parameterPathCache = new PathCacheForSpecs<Parameter>
         {
            new Parameter { Name = "name" }
         };

         var moleculeAmount = new MoleculeAmount { Name = "name" };
         new Container().WithName("top").Add(moleculeAmount);
         var initialConditionsPathCache = new PathCacheForSpecs<MoleculeAmount>
         {
            moleculeAmount
         };

         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<Parameter>(_simulationWithChanges)).Returns(parameterPathCache);
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<DistributedParameter>(_simulationWithChanges)).Returns(new PathCacheForSpecs<DistributedParameter>());
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<MoleculeAmount>(_simulationWithChanges)).Returns(initialConditionsPathCache);
      }
   }

   public class When_the_user_selects_the_module_to_commit_to : When_committing_changes_to_a_simulation_with_multiple_module_configurations
   {
      protected override void Context()
      {
         base.Context();
         _simulationWithChanges.AddOriginalQuantityValue(new OriginalQuantityValue { Path = new ObjectPath("name"), Value = 1.0 });
         _simulationWithChanges.AddOriginalQuantityValue(new OriginalQuantityValue { Path = new ObjectPath("top", "name"), Value = 1.0 });
         A.CallTo(() => _selectCommitTargetPresenter.SelectCommitTargetFor(_simulationWithChanges)).Returns(new CommitTarget(_moduleConfiguration, null));
      }

      protected override void Because()
      {
         sut.CommitSimulationChanges(_simulationWithChanges);
      }

      [Observation]
      public void new_building_blocks_are_created_in_the_selected_module()
      {
         _module.InitialConditionsCollection.Count.ShouldBeEqualTo(1);
         _module.ParameterValuesCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void new_building_blocks_are_selected_in_the_selected_module_configuration()
      {
         _moduleConfiguration.SelectedInitialConditions.ShouldNotBeNull();
         _moduleConfiguration.SelectedParameterValues.ShouldNotBeNull();
      }

      [Observation]
      public void the_last_module_configuration_is_not_modified()
      {
         _lastModule.BuildingBlocks.Count().ShouldBeEqualTo(0);
         _lastModuleConfiguration.SelectedInitialConditions.ShouldBeNull();
         _lastModuleConfiguration.SelectedParameterValues.ShouldBeNull();
      }

      [Observation]
      public void the_original_quantity_value_tracker_is_cleared()
      {
         _simulationWithChanges.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_the_user_cancels_the_commit_target_selection : When_committing_changes_to_a_simulation_with_multiple_module_configurations
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _selectCommitTargetPresenter.SelectCommitTargetFor(_simulationWithChanges)).Returns(null);
      }

      protected override void Because()
      {
         _command = sut.CommitSimulationChanges(_simulationWithChanges);
      }

      [Observation]
      public void no_command_is_returned()
      {
         _command.ShouldBeNull();
      }

      [Observation]
      public void the_user_is_not_asked_to_confirm_the_commit()
      {
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, A<ViewResult>._)).MustNotHaveHappened();
      }
   }

   public class When_committing_to_a_parameter_values_building_block_not_used_by_the_simulation : When_committing_to_configuration_with_selected_building_blocks
   {
      private ParameterValuesBuildingBlock _notUsedParameterValues;

      protected override void Context()
      {
         base.Context();
         _notUsedParameterValues = new ParameterValuesBuildingBlock();
         _projectModule.Add(_notUsedParameterValues);
         A.CallTo(() => _selectCommitTargetPresenter.SelectCommitTargetFor(_simulationWithChanges)).Returns(new CommitTarget(_moduleConfiguration, _notUsedParameterValues));
      }

      [Observation]
      public void the_parameter_values_are_written_to_the_selected_building_block_only()
      {
         _notUsedParameterValues.Count().ShouldBeEqualTo(1);
         _projectParameterValues.Count().ShouldBeEqualTo(0);
         _parameterValuesBuildingBlock.Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void the_building_block_used_by_the_simulation_stays_selected()
      {
         _moduleConfiguration.SelectedParameterValues.ShouldBeEqualTo(_parameterValuesBuildingBlock);
      }

      [Observation]
      public void the_original_quantity_value_tracker_is_retained_because_the_simulation_stays_out_of_sync()
      {
         _simulationWithChanges.OriginalQuantityValues.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_committing_to_a_new_parameter_values_building_block_while_the_simulation_uses_another_one : When_committing_to_configuration_with_selected_building_blocks
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _selectCommitTargetPresenter.SelectCommitTargetFor(_simulationWithChanges)).Returns(new CommitTarget(_moduleConfiguration, null));
      }

      [Observation]
      public void a_new_building_block_is_created_in_the_template_module_only()
      {
         _projectModule.ParameterValuesCollection.Count.ShouldBeEqualTo(2);
         _module.ParameterValuesCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_parameter_values_are_written_to_the_new_building_block_only()
      {
         _projectModule.ParameterValuesCollection.Single(x => !Equals(x, _projectParameterValues)).Count().ShouldBeEqualTo(1);
         _projectParameterValues.Count().ShouldBeEqualTo(0);
         _parameterValuesBuildingBlock.Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void the_building_block_used_by_the_simulation_stays_selected()
      {
         _moduleConfiguration.SelectedParameterValues.ShouldBeEqualTo(_parameterValuesBuildingBlock);
      }

      [Observation]
      public void the_original_quantity_value_tracker_is_retained_because_the_simulation_stays_out_of_sync()
      {
         _simulationWithChanges.OriginalQuantityValues.Count.ShouldBeEqualTo(2);
      }
   }
}