using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentNHibernate.Utils;
using MoBi.Assets;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Events;
using IProjectTask = MoBi.Presentation.Tasks.IProjectTask;

namespace MoBi.Presentation
{
   public abstract class concern_for_ProjectTask : ContextSpecification<IProjectTask>
   {
      protected IEventPublisher _eventPublisher;
      protected ISerializationTask _serializationTask;
      protected IMoBiContext _context;
      protected IDialogCreator _dialogCreator;
      protected ICloneManagerForSimulation _cloneManager;
      protected INameCorrector _nameCorrector;
      protected IMRUProvider _mruProvider;
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      private IHeavyWorkManager _heavyWorkManager;
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
         _sbmlTask = A.Fake<ISbmlTask>();
         _reactionBuildingBlockFactory = A.Fake<IReactionBuildingBlockFactory>();
         sut = new ProjectTask(_context, _serializationTask, _dialogCreator, _mruProvider, _heavyWorkManager,
            new SimulationLoader(_cloneManager, _nameCorrector, _context), _sbmlTask);
      }
   }

   internal class When_loading_a_simulation : concern_for_ProjectTask
   {
      protected MoBiProject _project;
      protected SimulationTransfer _simulationTransfer;
      protected IMoBiSimulation _simulation;
      protected PassiveTransportBuildingBlock _newBuildingBlock;
      protected IInteractionTasksForSimulationSettings _interactionTasksForSimulationSettings;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _simulationTransfer = A.Fake<SimulationTransfer>();
         _simulationTransfer.Favorites = new Favorites { "Fav1", "Fav2" };
         _simulation = A.Fake<IMoBiSimulation>();
         _simulationTransfer.Simulation = _simulation;
         _newBuildingBlock = A.Fake<PassiveTransportBuildingBlock>();
         _interactionTasksForSimulationSettings = A.Fake<IInteractionTasksForSimulationSettings>();

         var simulationConfiguration = new SimulationConfiguration();
         A.CallTo(() => _simulation.Configuration).Returns(simulationConfiguration);
         A.CallTo(() => _serializationTask.Load<SimulationTransfer>(A<string>._, A<bool>._)).Returns(_simulationTransfer);
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _nameCorrector.CorrectName(A<IEnumerable<PassiveTransportBuildingBlock>>._, _newBuildingBlock)).Returns(true);
      }
   }

   internal class When_loading_a_simulation_into_project : When_loading_a_simulation
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadSimulation, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, null, null)).Returns("File");
      }

      protected override void Because()
      {
         sut.LoadSimulationIntoProject();
      }

      [Observation]
      public void should_publish_load_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectLoadedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_publish_new_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectCreatedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_deserialize_the_file()
      {
         A.CallTo(() => _serializationTask.Load<SimulationTransfer>("File", A<bool>._)).MustHaveHappened();
      }

      [Observation]
      public void should_update_favorites_in_projects()
      {
         _simulationTransfer.Favorites.Each(favorite => _project.Favorites.ShouldContain(favorite));
      }

      [Observation]
      public void should_not_update_default_simulation_settings_in_project()
      {
         A.CallTo(() => _interactionTasksForSimulationSettings.UpdateDefaultSimulationSettingsInProject(_simulationTransfer.Simulation.Settings.OutputSchema, _simulationTransfer.Simulation.Settings.Solver)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_update_default_output_selections_in_project()
      {
         A.CallTo(() => _interactionTasksForSimulationSettings.UpdateDefaultOutputSelectionsInProject(A<IReadOnlyList<QuantitySelection>>.Ignored)).MustNotHaveHappened();
      }
   }

   internal class When_opening_a_simulation_as_project : When_loading_a_simulation
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadProject, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, null, null)).Returns("File");
         A.CallTo(() => _context.Resolve<IInteractionTasksForSimulationSettings>()).Returns(_interactionTasksForSimulationSettings);
      }

      protected override void Because()
      {
         sut.OpenSimulationAsProject();
      }

      [Observation]
      public void should_update_default_simulation_settings_in_project()
      {
         A.CallTo(() => _interactionTasksForSimulationSettings.UpdateDefaultSimulationSettingsInProject(_simulationTransfer.Simulation.Settings.OutputSchema, _simulationTransfer.Simulation.Settings.Solver)).MustHaveHappened();
      }

      [Observation]
      public void should_update_default_output_selections_in_project()
      {
         A.CallTo(() => _interactionTasksForSimulationSettings.UpdateDefaultOutputSelectionsInProject(A<IReadOnlyList<QuantitySelection>>.Ignored)).MustHaveHappened();
      }
   }

   public class When_loading_a_simulation_defined_in_concentration : concern_for_ProjectTask
   {
      private SimulationTransfer _simulationTransfer;
      private string _fileName;

      protected override void Context()
      {
         base.Context();
         _fileName = "filename";
         _simulationTransfer = A.Fake<SimulationTransfer>();
         _simulationTransfer.Simulation = A.Fake<IMoBiSimulation>().WithName("Sim");
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_fileName);
         A.CallTo(() => _serializationTask.Load<SimulationTransfer>(_fileName, false))
            .Throws(() => new CannotConvertConcentrationToAmountException("object"))
            .Once()
            .Then.Returns(_simulationTransfer);
      }

      protected override void Because()
      {
         sut.LoadSimulationIntoProject();
      }

      [Observation]
      public void should_be_able_to_load_the_simulation()
      {
         _context.CurrentProject.ReactionDimensionMode.ShouldBeEqualTo(ReactionDimensionMode.ConcentrationBased);
      }
   }

   public class When_starting_the_application_with_an_existing_working_journal : concern_for_ProjectTask
   {
      private string _journalFilePath;

      protected override void Context()
      {
         base.Context();
         _journalFilePath = "XX.sbj";
      }

      protected override void Because()
      {
         sut.StartWithJournal(_journalFilePath);
      }

      [Observation]
      public void should_create_a_new_amount_base_reaction_project_and_load_the_journal()
      {
         A.CallTo(() => _serializationTask.NewProject()).MustHaveHappened();
         A.CallTo(() => _serializationTask.LoadJournal(_journalFilePath, null, true)).MustHaveHappened();
      }
   }

   public class When_told_to_save_and_no_filename_is_given : concern_for_ProjectTask
   {
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
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
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
         _project.FilePath = "FilePath";
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void should_tell_serialization_tasks_to_save_project()
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
         var project = A.Fake<MoBiProject>();
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

   public class When_told_to_save_as_but_was_canceled : concern_for_ProjectTask
   {
      private bool _result;
      private string _fileName;

      protected override void Context()
      {
         base.Context();
         var project = A.Fake<MoBiProject>();
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
      private MoBiProject _project;
      private IWithIdRepository _objectBaseRepository;
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private MoBiReactionBuildingBlock _moBiReactionBuildingBlock;
      private MoBiSpatialStructure _spatialStructure;
      private IContainer _topContainer;
      private PassiveTransportBuildingBlock _passiveTransportBuildingBlock;
      private ObserverBuildingBlock _observerBuildingBlock;
      private EventGroupBuildingBlock _eventGroupBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
         _objectBaseRepository = A.Fake<IWithIdRepository>();
         _spatialStructure = A.Fake<MoBiSpatialStructure>();
         _moBiReactionBuildingBlock = A.Fake<MoBiReactionBuildingBlock>();
         _moleculeBuildingBlock = A.Fake<MoleculeBuildingBlock>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.Create<MoleculeBuildingBlock>()).Returns(_moleculeBuildingBlock);
         A.CallTo(() => _reactionBuildingBlockFactory.Create()).Returns(_moBiReactionBuildingBlock);
         A.CallTo(() => _spatialStructureFactory.CreateDefault(DefaultNames.SpatialStructure)).Returns(_spatialStructure);
         _topContainer = A.Fake<IContainer>();
         A.CallTo(() => _context.Create<IContainer>()).Returns(_topContainer);
         _passiveTransportBuildingBlock = A.Fake<PassiveTransportBuildingBlock>();
         A.CallTo(() => _context.Create<PassiveTransportBuildingBlock>()).Returns(_passiveTransportBuildingBlock);
         _observerBuildingBlock = A.Fake<ObserverBuildingBlock>();
         A.CallTo(() => _context.Create<ObserverBuildingBlock>()).Returns(_observerBuildingBlock);
         _eventGroupBuildingBlock = A.Fake<EventGroupBuildingBlock>();
         A.CallTo(() => _context.Create<EventGroupBuildingBlock>()).Returns(_eventGroupBuildingBlock);
         A.CallTo(() => _context.ObjectRepository).Returns(_objectBaseRepository);
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
      public void should_publish_project_load_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ProjectLoadedEvent>._)).MustHaveHappened();
      }
   }

   public class When_told_to_close_project_which_should_be_saved_but_save_was_canceled : concern_for_ProjectTask
   {
      private bool _result;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
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
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
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
         A.CallTo(() => _context.PublishEvent(A<ProjectClosingEvent>._)).MustHaveHappened().Then(A.CallTo(() => _context.PublishEvent(A<ProjectClosedEvent>._)).MustHaveHappened());
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
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
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
      private MoBiProject _project;
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
         _project = A.Fake<MoBiProject>();
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
      public void should_tell_serialization_tasks_to_load_project()
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
      private MoBiProject _oldProject;

      protected override void Context()
      {
         base.Context();
         _fileName = "Filename.xml";
         _oldProject = A.Fake<MoBiProject>();
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
      public void should_not_tell_serialization_tasks_to_load_project()
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
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _fileName = "Filename.mbp3";
         _project = A.Fake<MoBiProject>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.OpenProjectFrom(_fileName);
      }

      [Observation]
      public void should_tell_serialization_tasks_to_load_project()
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