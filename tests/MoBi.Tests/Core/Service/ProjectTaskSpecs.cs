using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Exceptions;
using MoBi.Core.Mappers;
using MoBi.Core.SBML;
using MoBi.Core.Services;
using MoBi.Presentation;
using MoBi.Presentation.Tasks;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Presentation.Services;
using IProjectTask = MoBi.Presentation.Tasks.IProjectTask;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ProjectTask : ContextSpecification<IProjectTask>
   {
      protected IMoBiContext _context;
      protected ISerializationTask _serializationTask;
      protected IDialogCreator _dialogCreator;
      protected INameCorrector _nameCorrector;
      protected IMRUProvider _mruProvider;
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      protected ISimulationFactory _simFactory;
      protected ICloneManagerForSimulation _cloneManager;
      protected ISimulationSettingsFactory _simSettingsFactory;
      protected IMoBiApplicationController _appController;
      protected ISbmlTask _sbmlTask;
      private IReactionBuildingBlockFactory _reactionBuildingBlockFactory;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _serializationTask = A.Fake<ISerializationTask>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _nameCorrector = A.Fake<INameCorrector>();
         _mruProvider = A.Fake<IMRUProvider>();
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _simFactory = A.Fake<ISimulationFactory>();
         _cloneManager = A.Fake<ICloneManagerForSimulation>();
         _simSettingsFactory = A.Fake<ISimulationSettingsFactory>();
         _appController = A.Fake<IMoBiApplicationController>();
         _sbmlTask = A.Fake<ISbmlTask>();
         _reactionBuildingBlockFactory = A.Fake<IReactionBuildingBlockFactory>();
         sut = new ProjectTask(_context, _serializationTask, _dialogCreator, _mruProvider, new HeavyWorkManagerForSpecs(), new SimulationLoader(_cloneManager, _nameCorrector, _context), _sbmlTask);
      }
   }

   internal class When_loading_a_simulation : concern_for_ProjectTask
   {
      private IMoBiProject _project;
      private SimulationTransfer _simulationTransfer;
      private IMoBiSimulation _simulation;
      private IPassiveTransportBuildingBlock _newBuildingBlock;
      private IMoBiReactionBuildingBlock _existingBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         _simulationTransfer = A.Fake<SimulationTransfer>();
         _simulationTransfer.Favorites = new Favorites {"Fav1", "Fav2"};
         _simulation = A.Fake<IMoBiSimulation>();
         _simulationTransfer.Simulation = _simulation;
         _newBuildingBlock = A.Fake<IPassiveTransportBuildingBlock>();
         _existingBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithId("Existing");
         // _existingBBInfo = new ReactionBuildingBlockInfo() {BuildingBlock = _existingBuildingBlock, TemplateBuildingBlockId = "Existing"};
         // _newBBInfo = new PassiveTransportBuildingBlockInfo() {BuildingBlock = _newBuildingBlock, TemplateBuildingBlockId = "New"};

         A.CallTo(() => _project.ReactionBlockCollection).Returns(new[] {_existingBuildingBlock});
         var simulationConfiguration = new SimulationConfiguration();
         A.CallTo(() => _simulation.Configuration).Returns(simulationConfiguration);
         // A.CallTo(() => simulationConfiguration.AllBuildingBlockInfos()).Returns(new IBuildingBlockInfo[] {_existingBBInfo, _newBBInfo});
         // simulationConfiguration.ReactionsInfo = _existingBBInfo;
         // simulationConfiguration.PassiveTransportsInfo = _newBBInfo;
         A.CallTo(() => _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadSimulation, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, null, null)).Returns("File");
         A.CallTo(() => _serializationTask.Load<SimulationTransfer>(A<string>._, A<bool>._)).Returns(_simulationTransfer);
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _nameCorrector.CorrectName(A<IEnumerable<IPassiveTransportBuildingBlock>>._, _newBuildingBlock)).Returns(true);
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
      public void should_deserilize_the_file()
      {
         A.CallTo(() => _serializationTask.Load<SimulationTransfer>("File", A<bool>._)).MustHaveHappened();
      }

      [Observation]
      public void should_update_favorites_in_projects()
      {
         A.CallTo(() => _project.Favorites.AddFavorites(_simulationTransfer.Favorites)).MustHaveHappened();
      }

      [Observation]
      [Ignore("46-7112: Problem with duplicate id")]
      public void should_clone_the_new_buidingblock()
      {
         A.CallTo(() => _cloneManager.CloneBuildingBlock(_newBuildingBlock)).MustHaveHappened();
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
      public void should_create_a_new_amount_base_reaction_project_and_load_the_jounrnal()
      {
         A.CallTo(() => _serializationTask.NewProject()).MustHaveHappened();
         A.CallTo(() => _serializationTask.LoadJournal(_journalFilePath, null,  true)).MustHaveHappened();
      }
   }
}