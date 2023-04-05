using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Serialization.Journal;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.FileLocker;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiContext : ContextSpecification<IMoBiContext>
   {
      protected IMoBiDimensionFactory _dimensionFactory;
      protected IEventPublisher _eventPublisher;
      protected IObjectBaseFactory _objectBaseFactory;
      private IXmlSerializationService _serializationService;
      private IObjectPathFactory _objectPathFactory;
      protected IWithIdRepository _objectBaseRepository;
      private IHistoryManagerFactory _moBiHistoryManagerFactory;
      private IRegisterTask _registerTask;
      protected IUnregisterTask _unregisterTask;
      private IClipboardManager _clipboardManager;
      private IContainer _container;
      protected IObjectTypeResolver _objectTypeResolver;
      private ICloneManagerForBuildingBlock _cloneManager;
      private IJournalSession _journalSession;
      private IFileLocker _fileLocker;
      private ILazyLoadTask _lazyLoadTask;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IMoBiDimensionFactory>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _registerTask = A.Fake<IRegisterTask>();
         _objectBaseRepository = A.Fake<IWithIdRepository>();
         _moBiHistoryManagerFactory = A.Fake<IHistoryManagerFactory>();
         _serializationService = A.Fake<IXmlSerializationService>();
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _unregisterTask = A.Fake<IUnregisterTask>();
         _clipboardManager = A.Fake<IClipboardManager>();
         _container = A.Fake<IContainer>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _journalSession = A.Fake<IJournalSession>();
         _fileLocker = A.Fake<IFileLocker>();
         _lazyLoadTask = A.Fake<ILazyLoadTask>();

         sut = new MoBiContext(_objectBaseFactory, _dimensionFactory, _eventPublisher,
            _serializationService, _objectPathFactory, _objectBaseRepository,
            _moBiHistoryManagerFactory, _registerTask, _unregisterTask,
            _clipboardManager, _container,
            _objectTypeResolver, _cloneManager,
            _journalSession, _fileLocker, _lazyLoadTask);

         A.CallTo(() => _moBiHistoryManagerFactory.Create()).Returns(A.Fake<MoBiHistoryManager>());
      }
   }

   public class When_told_to_clear : concern_for_MoBiContext
   {
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
         _project.Name = "Neu";
         sut.LoadFrom(_project);
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_set_the_reference_to_current_project_and_history_manager_to_null()
      {
         sut.CurrentProject.ShouldBeNull();
         sut.HistoryManager.ShouldBeNull();
      }

      [Observation]
      public void should_clear_the_object_base_repository()
      {
         A.CallTo(() => _objectBaseRepository.Clear()).MustHaveHappened();
      }
   }

   public class When_asked_to_get_an_object_base : concern_for_MoBiContext
   {
      private readonly string _id = "ID";

      protected override void Context()
      {
         base.Context();
         var project = A.Fake<MoBiProject>();
         project.Name = "Neu";
         A.CallTo(() => _objectBaseFactory.Create<MoBiProject>()).Returns(project);
         sut.NewProject();
      }

      protected override void Because()
      {
         sut.Get<IObjectBase>(_id);
      }

      [Observation]
      public void should_ask_objectbaserepository_for_the_object()
      {
         A.CallTo(() => _objectBaseRepository.Get(_id)).MustHaveHappened();
      }
   }

   public class When_told_to_create_a_new_ObjectBase : concern_for_MoBiContext
   {
      private IObjectBase _newObjectBase;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
         _project.Name = "Neu";
         sut.LoadFrom(_project);
         _newObjectBase = A.Fake<IObjectBase>();
         A.CallTo(() => _objectBaseFactory.Create<IObjectBase>()).Returns(_newObjectBase);
      }

      protected override void Because()
      {
         sut.Create<IObjectBase>(null);
      }

      [Observation]
      public void should_ask_objectBase_Factory_for_the_new_objectBase()
      {
         A.CallTo(() => _objectBaseFactory.Create<IObjectBase>()).MustHaveHappened();
      }

      [Observation]
      public void should_not_register_the_new_object_in_the_objectBaseRepository()
      {
         A.CallTo(() => _objectBaseRepository.Register(_newObjectBase)).MustNotHaveHappened();
      }
   }

   public class When_told_to_create_a_new_ObjectBase_with_a_given_id : concern_for_MoBiContext
   {
      private IObjectBase _newObjectBase;
      private MoBiProject _project;
      private string id;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<MoBiProject>();
         _project.Name = "Neu";
         _newObjectBase = A.Fake<IObjectBase>();
         id = "ID";
         _newObjectBase.Id = id;
         A.CallTo(() => _objectBaseFactory.Create<IObjectBase>(id)).Returns(_newObjectBase);
         sut.LoadFrom(_project);
      }

      protected override void Because()
      {
         sut.Create<IObjectBase>(id);
      }

      [Observation]
      public void should_ask_objectBase_Factory_for_the_new_objectBase()
      {
         A.CallTo(() => _objectBaseFactory.Create<IObjectBase>(id)).MustHaveHappened();
      }

      [Observation]
      public void should_not_register_the_new_object_in_the_objectBaseRepository()
      {
         A.CallTo(() => _objectBaseRepository.Register(_newObjectBase)).MustNotHaveHappened();
      }
   }

   public class When_creating_a_new_project : concern_for_MoBiContext
   {
      private MoBiProject _newProject;

      protected override void Context()
      {
         base.Context();
         _newProject = A.Fake<MoBiProject>();
         _newProject.Name = "Neu";
         A.CallTo(() => _objectBaseFactory.Create<MoBiProject>()).Returns(_newProject);
      }

      protected override void Because()
      {
         sut.NewProject();
      }

      [Observation]
      public void should_set_current_project_to_the_new_project()
      {
         sut.CurrentProject.ShouldBeEqualTo(_newProject);
      }
   }

   public class When_unregistering_a_simulation_from_context : concern_for_MoBiContext
   {
      private IMoBiSimulation _simulation;
      private IFormula _explicitFormula;
      private IFormula _explicitFormulaInNeighborhood;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _simulation.Model = A.Fake<IModel>();
         _explicitFormula = new ExplicitFormula("1+2");
         _explicitFormulaInNeighborhood = new ExplicitFormula("1+2");
         _simulation.Model.Root = new Container {new Parameter().WithFormula(_explicitFormula)};
         _simulation.Model.Neighborhoods = new Container {new Parameter().WithFormula(_explicitFormulaInNeighborhood)};
      }

      protected override void Because()
      {
         sut.UnregisterSimulation(_simulation);
      }

      [Observation]
      public void should_unregister_the_simulation()
      {
         A.CallTo(() => _unregisterTask.UnregisterAllIn(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_also_unregister_all_cachable_formula()
      {
         A.CallTo(() => _unregisterTask.UnregisterAllIn(_explicitFormula)).MustHaveHappened();
         A.CallTo(() => _unregisterTask.UnregisterAllIn(_explicitFormulaInNeighborhood)).MustHaveHappened();
      }
   }
}