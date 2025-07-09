using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Domain.Services;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Container;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.IntegrationTests
{
   public class concern_for_InteractionTasksForModule : ContextForIntegration<InteractionTasksForModule>
   {
      protected Module _module;
      protected ISelectBuildingBlockTypePresenter _selectTypePresenter;
      protected IInteractionTaskContext _context;
      protected IDialogCreator _dialogCreator;
      protected IMoBiContext _moBiContext;
      protected IInitialConditionsTask<InitialConditionsBuildingBlock> _initialConditionsTask;
      protected IParameterValuesTask _parameterValuesTask;

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();

         var interactionTask = new InteractionTask(IoC.Resolve<ISerializationTask>(), _dialogCreator, A.Fake<IIconRepository>(),
            A.Fake<INameCorrector>(), new CloneManagerForBuildingBlockForSpecs(),
            A.Fake<IAdjustFormulasVisitor>(), A.Fake<IObjectTypeResolver>(), A.Fake<IForbiddenNamesRetriever>()
         );

         _moBiContext = A.Fake<IMoBiContext>();
         _context = new InteractionTaskContext(_moBiContext, A.Fake<IMoBiApplicationController>(),
            interactionTask, A.Fake<IActiveSubjectRetriever>(), A.Fake<IUserSettings>(),
            A.Fake<IDisplayUnitRetriever>(), _dialogCreator,
            A.Fake<ICommandTask>(), A.Fake<IObjectTypeResolver>(), A.Fake<IMoBiFormulaTask>(),
            A.Fake<IMoBiConfiguration>(), A.Fake<DirectoryMapSettings>(), A.Fake<ICheckNameVisitor>(), A.Fake<IBuildingBlockRepository>(),
            A.Fake<IObjectBaseNamingTask>());

         _parameterValuesTask = A.Fake<IParameterValuesTask>();
         _initialConditionsTask = A.Fake<IInitialConditionsTask<InitialConditionsBuildingBlock>>();

         _selectTypePresenter = A.Fake<ISelectBuildingBlockTypePresenter>();
         A.CallTo(() => _context.ApplicationController.Start<ISelectBuildingBlockTypePresenter>()).Returns(_selectTypePresenter);
         A.CallTo(() => _dialogCreator.AskForFileToOpen(A<string>._, A<string>._, A<string>._, A<string>._, A<string>._)).Returns(DomainHelperForSpecs.TestFileFullPath("Sim_V12.pkml"));
         _module = new Module();

         sut = new InteractionTasksForModule(_context, new EditTaskForModule(_context), _parameterValuesTask, _initialConditionsTask);
      }
   }

   public class When_removing_multiple_modules_from_the_project_and_some_are_used_in_simulation : concern_for_InteractionTasksForModule
   {
      private Module _module2;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _module2 = new Module();
         _project = new MoBiProject();
         var moBiSimulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration()
         };
         moBiSimulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(_module));
         _project.AddSimulation(moBiSimulation);
         
         A.CallTo(() => _moBiContext.CurrentProject).Returns(_project);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, A<ViewResult>._)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.Remove(new[] { _module, _module2 });
      }

      [Observation]
      public void the_user_should_see_the_names_of_the_modules_not_removed()
      {
         A.CallTo(() => _context.DialogCreator.MessageBoxInfo(A<string>._)).MustHaveHappened();
      }
   }

   public class When_removing_multiple_modules_from_the_project_and_none_are_used_in_simulation : concern_for_InteractionTasksForModule
   {
      private Module _module2;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _module2 = new Module();
         _project = new MoBiProject();
         A.CallTo(() => _moBiContext.CurrentProject).Returns(_project);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, A<ViewResult>._)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.Remove(new[] { _module, _module2 });
      }

      [Observation]
      public void the_user_should_not_be_prompted()
      {
         A.CallTo(() => _context.DialogCreator.MessageBoxInfo(A<string>._)).MustNotHaveHappened();
      }
   }

   public class When_loading_building_blocks_into_the_module : concern_for_InteractionTasksForModule
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo((() => _parameterValuesTask.CorrectName(A<ParameterValuesBuildingBlock>._, _module))).Returns(true);
         A.CallTo((() => _initialConditionsTask.CorrectName(A<InitialConditionsBuildingBlock>._, _module))).Returns(true);
      }

      [TestCase(BuildingBlockType.SpatialStructure)]
      [TestCase(BuildingBlockType.Reactions)]
      [TestCase(BuildingBlockType.Events)]
      [TestCase(BuildingBlockType.PassiveTransport)]
      [TestCase(BuildingBlockType.Molecules)]
      [TestCase(BuildingBlockType.Observers)]
      [TestCase(BuildingBlockType.InitialConditions)]
      [TestCase(BuildingBlockType.ParameterValues)]
      public void the_building_block_is_loaded_to_the_module(BuildingBlockType type)
      {
         A.CallTo(() => _selectTypePresenter.GetBuildingBlockType(A<Module>._)).Returns(type);
         sut.LoadBuildingBlocksToModule(_module);
         _module.BuildingBlocks.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_loading_building_blocks_into_the_module_and_renaming_is_declined : concern_for_InteractionTasksForModule
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo((() => _parameterValuesTask.CorrectName(A<ParameterValuesBuildingBlock>._, _module))).Returns(false);
         A.CallTo((() => _initialConditionsTask.CorrectName(A<InitialConditionsBuildingBlock>._, _module))).Returns(false);
      }

      [TestCase(BuildingBlockType.InitialConditions)]
      [TestCase(BuildingBlockType.ParameterValues)]
      public void the_building_block_is_not_loaded_to_the_module(BuildingBlockType type)
      {
         A.CallTo(() => _selectTypePresenter.GetBuildingBlockType(A<Module>._)).Returns(type);
         sut.LoadBuildingBlocksToModule(_module);
         _module.BuildingBlocks.Count.ShouldBeEqualTo(0);
      }
   }
}