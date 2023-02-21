using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.SBML;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Services;
using IProjectTask = MoBi.Presentation.Tasks.IProjectTask;

namespace MoBi.Presentation
{
   public abstract class concern_for_ProjectTask : ContextSpecification<IProjectTask>
   {
      protected IEventPublisher _eventPublisher;
      protected ISerializationTask _serializationTask;
      protected IMoBiContext _context;
      protected IDialogCreator _dialogCreator;
      private ICloneManagerForSimulation _cloneManager;
      private INameCorrector _nameCorrector;
      protected IMRUProvider _mruProvider;
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      private IHeavyWorkManager _heavyWorkManager;
      protected ISimulationSettingsFactory _simulationSettingsFactory;
      private ISbmlTask _sbmlTask;
      protected IReactionBuildingBlockFactory _reactionBuildingBlockFactory;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _serializationTask = A.Fake<ISerializationTask>();
         _context = A.Fake<IMoBiContext>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _heavyWorkManager = new HeavyWorkManagerForSpecs();
         _nameCorrector = A.Fake<INameCorrector>();
         _cloneManager = A.Fake<ICloneManagerForSimulation>();
         _mruProvider = A.Fake<IMRUProvider>();
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _simulationSettingsFactory = A.Fake<ISimulationSettingsFactory>();
         _sbmlTask = A.Fake<ISbmlTask>();
         _reactionBuildingBlockFactory = A.Fake<IReactionBuildingBlockFactory>();
         sut = new ProjectTask(_context, _serializationTask, _dialogCreator, _mruProvider, _spatialStructureFactory, _heavyWorkManager, _simulationSettingsFactory, 
            new SimulationLoader(_cloneManager, _nameCorrector, _context), _sbmlTask, _reactionBuildingBlockFactory);
      }
   }

   public class When_told_to_save_and_no_filename_is_given : concern_for_ProjectTask
   {
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         A.CallTo(
            () => _dialogCreator.AskForFileToSave(AppConstants.Dialog.AskForSaveProject, AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT, _project.Name, null))
            .Returns("Name");
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void should_ask_for_filename()
      {
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToSave(AppConstants.Dialog.AskForSaveProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT, string.Empty, null))
            .MustHaveHappened();
      }
   }

   public class When_told_to_save : concern_for_ProjectTask
   {
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         _project.FilePath = "FilePath";
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void should_tell_serialisation_tasks_to_save_project()
      {
         A.CallTo(() => _serializationTask.SaveProject()).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_file_name_to_mru_provider()
      {
         A.CallTo(() => _mruProvider.Add(_project.FilePath)).MustHaveHappened();
      }
   }

   public class When_told_to_save_as : concern_for_ProjectTask
   {
      private bool _result;
      private string _fileName;

      protected override void Context()
      {
         base.Context();
         var project = A.Fake<IMoBiProject>();
         _fileName = "file";
         A.CallTo(() => project.Name).Returns(_fileName);
         A.CallTo(() => _context.CurrentProject).Returns(project);
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToSave(AppConstants.Dialog.AskForSaveProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER,
                  Constants.DirectoryKey.PROJECT, _fileName, null))
            .Returns(_fileName);
      }

      protected override void Because()
      {
         _result = sut.SaveAs();
      }


      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }

      [Observation]
      public void should_add_filename_to_mru_provider()
      {
         A.CallTo(() => _mruProvider.Add(_fileName)).MustHaveHappened();
      }
   }

   public class When_told_to_save_as_but_was_canceld : concern_for_ProjectTask
   {
      private bool _result;
      private string _fileName;

      protected override void Context()
      {
         base.Context();
         var project = A.Fake<IMoBiProject>();
         _fileName = "file";
         A.CallTo(() => project.Name).Returns(_fileName);
         A.CallTo(() => _context.CurrentProject).Returns(project);
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToSave(AppConstants.Dialog.AskForSaveProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER,
                  Constants.DirectoryKey.PROJECT, _fileName, null))
            .Returns(String.Empty);
      }

      protected override void Because()
      {
         _result = sut.SaveAs();
      }

      [Observation]
      public void should_ask_for_the_filename()
      {
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToSave(AppConstants.Dialog.AskForSaveProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER,
                  Constants.DirectoryKey.PROJECT, _fileName, null)).MustHaveHappened();
      }

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      [Observation]
      public void should_not_add_filename_to_mru_provider()
      {
         A.CallTo(() => _mruProvider.Add(_fileName)).MustNotHaveHappened();
      }
   }

   public class When_told_to_create_new_project : concern_for_ProjectTask
   {
      private IMoBiProject _project;
      private IWithIdRepository _objectBaseRepository;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private IMoBiReactionBuildingBlock _moBiReactionBuildingBlock;
      private IMoBiSpatialStructure _spatialStructure;
      private IContainer _topContainer;
      private IPassiveTransportBuildingBlock _passiveTransportBuildingBlock;
      private IObserverBuildingBlock _observerBuildingBlock;
      private IEventGroupBuildingBlock _eventGroupBuildingBlock;
      private ISimulationSettings _simulationSettings;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         _module = new Module();
         _objectBaseRepository = A.Fake<IWithIdRepository>();
         _spatialStructure = A.Fake<IMoBiSpatialStructure>();
         _simulationSettings = A.Fake<ISimulationSettings>();
         _moBiReactionBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>();
         _moleculeBuildingBlock = A.Fake<IMoleculeBuildingBlock>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.Create<IMoleculeBuildingBlock>()).Returns(_moleculeBuildingBlock);
         A.CallTo(() => _reactionBuildingBlockFactory.Create()).Returns(_moBiReactionBuildingBlock);
         A.CallTo(() => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure)).Returns(_spatialStructure);
         A.CallTo(() => _simulationSettingsFactory.CreateDefault()).Returns(_simulationSettings);
         _topContainer = A.Fake<IContainer>();
         A.CallTo(() => _context.Create<IContainer>()).Returns(_topContainer);
         _passiveTransportBuildingBlock = A.Fake<IPassiveTransportBuildingBlock>();
         A.CallTo(() => _context.Create<IPassiveTransportBuildingBlock>()).Returns(_passiveTransportBuildingBlock);
         _observerBuildingBlock = A.Fake<IObserverBuildingBlock>();
         A.CallTo(() => _context.Create<IObserverBuildingBlock>()).Returns(_observerBuildingBlock);
         _eventGroupBuildingBlock = A.Fake<IEventGroupBuildingBlock>();
         A.CallTo(() => _context.Create<IEventGroupBuildingBlock>()).Returns(_eventGroupBuildingBlock);
         A.CallTo(() => _context.ObjectRepository).Returns(_objectBaseRepository);
         A.CallTo(() => _context.Create<Module>()).Returns(_module);
      }

      protected override void Because()
      {
         sut.New(ReactionDimensionMode.AmountBased);
      }

      [Observation]
      public void should_tell_context_to_renew()
      {
         A.CallTo(() => _serializationTask.NewProject()).MustHaveHappened();
      }

      [Observation]
      public void the_default_module_contains_appropriate_building_blocks()
      {
         _module.SpatialStructure.ShouldBeEqualTo(_spatialStructure);
         _module.PassiveTransport.ShouldBeEqualTo(_passiveTransportBuildingBlock);
         _module.EventGroup.ShouldBeEqualTo(_eventGroupBuildingBlock);
         _module.Molecule.ShouldBeEqualTo(_moleculeBuildingBlock);
         _module.Observer.ShouldBeEqualTo(_observerBuildingBlock);
         _module.Reaction.ShouldBeEqualTo(_moBiReactionBuildingBlock);
         _module.MoleculeStartValuesCollection.ShouldBeEmpty();
         _module.ParameterStartValuesCollection.ShouldBeEmpty();
      }

      [Observation]
      public void should_have_created_default_project_entries()
      {
         A.CallTo(() => _context.Create<IMoleculeBuildingBlock>()).MustHaveHappened();
         A.CallTo(() => _reactionBuildingBlockFactory.Create()).MustHaveHappened();
         A.CallTo(() => _context.Create<IPassiveTransportBuildingBlock>()).MustHaveHappened();
         A.CallTo(() => _context.Create<IObserverBuildingBlock>()).MustHaveHappened();
         A.CallTo(() => _context.Create<IEventGroupBuildingBlock>()).MustHaveHappened();
         A.CallTo(() => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure)).MustHaveHappened();
         A.CallTo(() => _simulationSettingsFactory.CreateDefault()).MustHaveHappened();
         A.CallTo(() => _context.Create<Module>()).MustHaveHappened();
      }

      [Observation]
      public void should_have_registered_default_project_entries()
      {
         A.CallTo(() => _context.Register(_moleculeBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _reactionBuildingBlockFactory.Create()).MustHaveHappened();
         A.CallTo(() => _context.Register(_spatialStructure)).MustHaveHappened();
         A.CallTo(() => _context.Register(_passiveTransportBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _context.Register(_observerBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _context.Register(_eventGroupBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _context.Register(_simulationSettings)).MustHaveHappened();
         A.CallTo(() => _context.Register(_module)).MustHaveHappened();
      }

      [Observation]
      public void should_publish_project_load_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectLoadedEvent>._)).MustHaveHappened();
      }
   }

   public class When_told_to_close_project_which_should_be_saved_but_save_was_canceled : concern_for_ProjectTask
   {
      private bool _result;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         _project.HasChanged = true;
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(string.Empty);
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _dialogCreator.MessageBoxYesNoCancel(AppConstants.Dialog.DoYouWantToSaveTheCurrentProject, ViewResult.Yes))
            .Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         _result = sut.CloseProject();
      }

      [Observation]
      public void should_tell_context_not_to_clear()
      {
         A.CallTo(() => _context.Clear()).MustNotHaveHappened();
      }

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      [Observation]
      public void should_not_publish_project_closed_or_closing_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectClosedEvent>._)).MustNotHaveHappened();
         A.CallTo(() => _context.PublishEvent(A<ProjectClosingEvent>._)).MustNotHaveHappened();
      }
   }

   public class When_told_to_close_project_which_should_be_saved : concern_for_ProjectTask
   {
      private bool _result;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         _project.HasChanged = true;
         _project.FilePath = "AA";
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _dialogCreator.MessageBoxYesNoCancel(AppConstants.Dialog.DoYouWantToSaveTheCurrentProject, ViewResult.Yes))
            .Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         _result = sut.CloseProject();
      }

      [Observation]
      public void closing_and_closed_events_should_be_raised_in_order()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectClosingEvent>._)).MustHaveHappened().
            Then(A.CallTo(() => _context.PublishEvent(A<ProjectClosedEvent>._)).MustHaveHappened());
      }

      [Observation]
      public void should_ask_if_project_Should_be_closed()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNoCancel(AppConstants.Dialog.DoYouWantToSaveTheCurrentProject, ViewResult.Yes))
            .MustHaveHappened();
      }

      [Observation]
      public void should_tell_context_to_clear()
      {
         A.CallTo(() => _serializationTask.CloseProject()).MustHaveHappened();
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }

      [Observation]
      public void should_publish_project_closed_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectClosedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_tell_serialization_task_to_serialize_project()
      {
         A.CallTo(() => _serializationTask.SaveProject()).MustHaveHappened();
      }
   }

   public class When_told_to_close_project_which_should_not_be_saved : concern_for_ProjectTask
   {
      private bool _result;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         _project.HasChanged = true;
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _dialogCreator.MessageBoxYesNoCancel(AppConstants.Dialog.DoYouWantToSaveTheCurrentProject, ViewResult.Yes))
            .Returns(ViewResult.No);
      }

      protected override void Because()
      {
         _result = sut.CloseProject();
      }

      [Observation]
      public void closing_and_closed_events_should_be_raised_in_order()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectClosingEvent>._)).MustHaveHappened();
         A.CallTo(() => _context.PublishEvent(A<ProjectClosedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_ask_if_project_Should_be_closed()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNoCancel(AppConstants.Dialog.DoYouWantToSaveTheCurrentProject, ViewResult.Yes))
            .MustHaveHappened();
      }

      [Observation]
      public void should_tell_context_to_clear()
      {
         A.CallTo(() => _serializationTask.CloseProject()).MustHaveHappened();
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }

      [Observation]
      public void should_publish_project_closed_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectClosedEvent>._)).MustHaveHappened();
      }
   }

   public class When_told_to_open : concern_for_ProjectTask
   {
      private string _fileName;
      private IMoBiProject _project;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _fileName = "Filename.mbp3";
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT, null, null))
            .Returns(_fileName);
         _project = A.Fake<IMoBiProject>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         _result = sut.Open();
      }

      [Observation]
      public void should_ask_for_file_name_to_open()
      {
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT, null, null))
            .MustHaveHappened();
      }

      [Observation]
      public void should_tell_serialisation_tasks_to_load_project()
      {
         A.CallTo(() => _serializationTask.LoadProject(_fileName)).MustHaveHappened();
      }

      [Observation]
      public void should_publish_project_load_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectLoadedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }

      [Observation]
      public void should_add_FileName_to_mru_provider()
      {
         A.CallTo(() => _mruProvider.Add(_fileName)).MustHaveHappened();
      }
   }

   public class When_told_to_open_and_an_existing_project_could_not_be_closed : concern_for_ProjectTask
   {
      private string _fileName;
      private bool _result;
      private IMoBiProject _oldProject;

      protected override void Context()
      {
         base.Context();
         _fileName = "Filename.xml";
         _oldProject = A.Fake<IMoBiProject>();
         _oldProject.HasChanged = true;
         _oldProject.FilePath = String.Empty;
         A.CallTo(() => _context.CurrentProject).Returns(_oldProject);
         A.CallTo(() => _dialogCreator.MessageBoxYesNoCancel(AppConstants.Dialog.DoYouWantToSaveTheCurrentProject, ViewResult.Yes))
            .Returns(ViewResult.Yes);
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToSave(AppConstants.Dialog.AskForSaveProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT, null, null))
            .Returns(String.Empty);
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT, null, null))
            .Returns(_fileName);
      }

      protected override void Because()
      {
         _result = sut.Open();
      }

      [Observation]
      public void should_not_ask_for_file_name_to_open()
      {
         A.CallTo(
            () =>
               _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadProject,
                  AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT, null, null)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_tell_serialisation_tasks_to_load_project()
      {
         A.CallTo(() => _serializationTask.LoadProject(_fileName)).MustNotHaveHappened();
      }

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }
   }

   public class When_told_to_open_from_filename : concern_for_ProjectTask
   {
      private string _fileName;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _fileName = "Filename.mbp3";
         _project = A.Fake<IMoBiProject>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.OpenProjectFrom(_fileName);
      }

      [Observation]
      public void should_tell_serialisation_tasks_to_load_project()
      {
         A.CallTo(() => _serializationTask.LoadProject(_fileName)).MustHaveHappened();
      }

      [Observation]
      public void should_publish_project_load_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectLoadedEvent>._)).MustHaveHappened();
      }
   }

   public class When_creating_a_new_project : concern_for_ProjectTask
   {
      protected override void Because()
      {
         sut.New(ReactionDimensionMode.AmountBased);
      }

      [Observation]
      public void should_set_the_selected_rate_dimension_to_be_the_one_used_in_the_project()
      {
         _context.CurrentProject.ReactionDimensionMode.ShouldBeEqualTo(ReactionDimensionMode.AmountBased);
      }
   }
}