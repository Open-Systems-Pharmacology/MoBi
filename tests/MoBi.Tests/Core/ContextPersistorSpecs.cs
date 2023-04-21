using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Infrastructure.Serialization.Services;
using OSPSuite.Utility.Events;

namespace MoBi.Core
{
   public abstract class concern_for_ContextPersistor : ContextSpecification<IContextPersistor>
   {
      private SimulationSettings _simulationSettings;
      private ISimulationSettingsFactory _simulationSettingsFactory;
      private IProjectConverterLogger _projectConverterLogger;
      private ISessionManager _sessionManager;
      private IProjectPersistor _projectPersistor;
      private IHistoryManagerPersistor _historyManagerPersistor;
      private IProjectFileCompressor _projectFileCompressor;
      private IEventPublisher _eventPublisher;
      private IJournalTask _journalTask;
      private IJournalLoader _journalLoader;

      protected override void Context()
      {
         _projectConverterLogger = A.Fake<IProjectConverterLogger>();
         _sessionManager = A.Fake<ISessionManager>();
         _projectPersistor = A.Fake<IProjectPersistor>();
         _historyManagerPersistor = A.Fake<IHistoryManagerPersistor>();
         _projectFileCompressor = A.Fake<IProjectFileCompressor>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _journalTask = A.Fake<IJournalTask>();
         _journalLoader = A.Fake<IJournalLoader>();

         _simulationSettingsFactory = A.Fake<ISimulationSettingsFactory>();
         _simulationSettings = new SimulationSettings();
         A.CallTo(() => _simulationSettingsFactory.CreateDefault()).Returns(_simulationSettings);


         sut = new ContextPersistor(_projectConverterLogger, _sessionManager, _projectPersistor, _historyManagerPersistor, _projectFileCompressor, _eventPublisher, _journalTask, _journalLoader, _simulationSettingsFactory);
      }

      public class When_creating_a_new_project : concern_for_ContextPersistor
      {
         private IMoBiContext _context;
         private MoBiProject _project;

         protected override void Context()
         {
            base.Context();
            _context = A.Fake<IMoBiContext>();
            _project = new MoBiProject();
            A.CallTo(() => _context.CurrentProject).Returns(_project);
         }

         protected override void Because()
         {
            sut.NewProject(_context);
         }

         [Observation]
         public void should_create_a_new_project_in_context()
         {
            A.CallTo(() => _context.NewProject()).MustHaveHappened();
         }

         [Observation]
         public void there_should_be_a_simulation_settings_created_by_the_simulation_settings_factory()
         {
            _project.SimulationSettings.ShouldBeEqualTo(_simulationSettings);
         }
      }
   }
}