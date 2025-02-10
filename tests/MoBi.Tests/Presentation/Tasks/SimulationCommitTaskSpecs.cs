using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
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
      protected IInitialConditionsCreator _initialConditionsCreator;
      protected IParameterValuesCreator _parameterValuesCreator;
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      protected ITemplateResolverTask _templateResolverTask;
      protected IMoBiContext _context;
      protected ModuleConfiguration _moduleConfiguration;
      protected Module _module;

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
         _initialConditionsCreator = A.Fake<IInitialConditionsCreator>();
         _parameterValuesCreator = A.Fake<IParameterValuesCreator>();
         _nameCorrector = A.Fake<INameCorrector>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, A<ViewResult>._)).Returns(ViewResult.Yes);
         sut = new SimulationCommitTask(_context, _templateResolverTask, _entitiesInSimulationRetriever, _initialConditionsCreator, _parameterValuesCreator, _nameCorrector, new ObjectTypeResolver(), _interactionTaskContext);
      }

      [Observation]
      public void the_simulation_should_not_have_original_value_trackers()
      {
         _simulationWithChanges.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
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
   }
}