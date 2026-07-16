using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   internal class concern_for_ModuleLoader : ContextSpecification<ModuleLoader>
   {
      protected MoBiProject _project;
      protected SimulationTransfer _simulationTransfer;
      protected MoBiSimulation _moBiSimulation;
      protected IInteractionTasksForIndividualBuildingBlock _individualTask;
      protected IInteractionTasksForExpressionProfileBuildingBlock _expressionTask;
      protected IInteractionTasksForModule _moduleTask;
      protected IInteractionTaskContext _interactionTaskContext;
      protected Module _addedModule;
      protected ICloneManagerForSimulation _cloneManager;
      protected SimulationConfiguration _clonedConfiguration;

      protected override void Context()
      {
         base.Context();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _moduleTask = A.Fake<IInteractionTasksForModule>();
         _expressionTask = A.Fake<IInteractionTasksForExpressionProfileBuildingBlock>();
         _individualTask = A.Fake<IInteractionTasksForIndividualBuildingBlock>();
         _cloneManager = A.Fake<ICloneManagerForSimulation>();
         _project = new MoBiProject();
         _simulationTransfer = new SimulationTransfer();

         _moBiSimulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration()
         };

         _simulationTransfer.Simulation = _moBiSimulation;

         _clonedConfiguration = new SimulationConfiguration();
         _addedModule = new Module().WithName("module");
         _clonedConfiguration.AddModuleConfiguration(new ModuleConfiguration(_addedModule));
         AddAdditionalBuildingBlocks();

         A.CallTo(() => _interactionTaskContext.InteractionTask.CorrectName(A<Module>._, A<IEnumerable<string>>._)).Returns(true);
         A.CallTo(() => _moduleTask.AskForPKMLFileToOpen()).Returns("filePath");
         A.CallTo(() => _cloneManager.CloneSimulationConfiguration(_moBiSimulation.Configuration)).Returns(_clonedConfiguration);

         sut = new ModuleLoader(_moduleTask, _expressionTask, _individualTask, _interactionTaskContext, _cloneManager);
      }

      protected virtual void AddAdditionalBuildingBlocks()
      {
         _clonedConfiguration.Individual = new IndividualBuildingBlock().WithName("individual");
         _clonedConfiguration.AddExpressionProfile(new ExpressionProfileBuildingBlock().WithName("expressionProfile1"));
         _clonedConfiguration.AddExpressionProfile(new ExpressionProfileBuildingBlock().WithName("expressionProfile2"));
      }
   }

   internal class When_the_user_cancels_loading_a_module : concern_for_ModuleLoader
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _moduleTask.AddTo(A<List<Module>>.That.Matches(x => containsAddedModule(x)), _project, null)).Returns(new MoBiEmptyCommand());
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadTransfer<SimulationTransfer>(A<string>._)).Returns(_simulationTransfer);
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.Yes);
      }

      private bool containsAddedModule(List<Module> list)
      {
         return list.Contains(_addedModule);
      }

      protected override void Because()
      {
         sut.LoadModule(_project);
      }

      [Observation]
      public void the_expressions_and_individual_should_not_be_added()
      {
         A.CallTo(() => _individualTask.AddTo(A<IReadOnlyCollection<IndividualBuildingBlock>>._, _project, null)).MustNotHaveHappened();
         A.CallTo(() => _expressionTask.AddTo(A<IReadOnlyCollection<ExpressionProfileBuildingBlock>>._, _project, null)).MustNotHaveHappened();

      }
   }

   internal class When_loading_from_pkml_with_module_pkml : concern_for_ModuleLoader
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadTransfer<SimulationTransfer>(A<string>._)).Throws(() => new NotMatchingSerializationFileException("exception"));
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadItems<Module>("filePath")).Returns(new[] { new Module() });
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.LoadModule(_project);
      }

      [Observation]
      public void the_module_is_loaded_using_the_interaction_task()
      {
         A.CallTo(() => _moduleTask.AddFromFileTo("filePath", _project, null)).MustHaveHappened();
      }
   }

   internal class When_loading_from_pkml_with_simulation_transfer_pkml_and_user_confirms_additional_building_blocks : concern_for_ModuleLoader
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadTransfer<SimulationTransfer>(A<string>._)).Returns(_simulationTransfer);
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.LoadModule(_project);
      }

      [Observation]
      public void the_tasks_should_be_used_to_add_elements_to_projects()
      {
         A.CallTo(() => _moduleTask.AddTo(A<IReadOnlyCollection<Module>>.That.Matches(x => x.Contains(_addedModule)), _project, null)).MustHaveHappened();
         A.CallTo(() => _individualTask.AddTo(A<IReadOnlyCollection<IndividualBuildingBlock>>.That.Matches(x => x.ElementAt(0).Equals(_clonedConfiguration.Individual)), _project, null)).MustHaveHappened();
         A.CallTo(() => _expressionTask.AddTo(A<IReadOnlyCollection<ExpressionProfileBuildingBlock>>.That.Matches(x => x.ElementAt(0).Equals(_clonedConfiguration.ExpressionProfiles[0])), _project, null)).MustHaveHappened();
         A.CallTo(() => _expressionTask.AddTo(A<IReadOnlyCollection<ExpressionProfileBuildingBlock>>.That.Matches(x => x.ElementAt(1).Equals(_clonedConfiguration.ExpressionProfiles[1])), _project, null)).MustHaveHappened();
      }
   }

   internal class When_loading_from_pkml_with_simulation_transfer_pkml_and_user_declines_additional_building_blocks : concern_for_ModuleLoader
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadTransfer<SimulationTransfer>(A<string>._)).Returns(_simulationTransfer);
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.LoadModule(_project);
      }

      [Observation]
      public void the_module_is_still_loaded_using_the_interaction_task()
      {
         A.CallTo(() => _moduleTask.AddTo(A<IReadOnlyCollection<Module>>.That.Matches(x => x.Contains(_addedModule)), _project, null)).MustHaveHappened();
      }

      [Observation]
      public void the_tasks_should_not_be_used_to_add_elements_to_projects()
      {
         A.CallTo(() => _individualTask.AddTo(A<IReadOnlyCollection<IndividualBuildingBlock>>._, A<MoBiProject>._, null)).MustNotHaveHappened();
         A.CallTo(() => _expressionTask.AddTo(A<IReadOnlyCollection<ExpressionProfileBuildingBlock>>._, A<MoBiProject>._, null)).MustNotHaveHappened();
         A.CallTo(() => _expressionTask.AddTo(A<IReadOnlyCollection<ExpressionProfileBuildingBlock>>._, A<MoBiProject>._, null)).MustNotHaveHappened();
      }
   }

   internal class When_loading_from_pkml_with_simulation_transfer_pkml_and_there_arent_additional_building_blocks : concern_for_ModuleLoader
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadTransfer<SimulationTransfer>(A<string>._)).Returns(_simulationTransfer);
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.Yes);
      }

      protected override void AddAdditionalBuildingBlocks()
      {
         // do not add anything
      }

      protected override void Because()
      {
         sut.LoadModule(_project);
      }

      [Observation]
      public void the_user_should_not_be_prompted_to_import_additions()
      {
         A.CallTo(() => _interactionTaskContext.DialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).MustNotHaveHappened();
      }

      [Observation]
      public void the_module_is_still_loaded_using_the_interaction_task()
      {
         A.CallTo(() => _moduleTask.AddTo(A<IReadOnlyCollection<Module>>.That.Matches(x => x.Contains(_addedModule)), _project, null)).MustHaveHappened();
      }
   }
}
