using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using FakeItEasy;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation
{
   public abstract class concern_for_ApplicationControllerSpecs : ContextSpecification<IMoBiApplicationController>
   {
      protected IContainer _container;
      protected IEventPublisher _eventPublisher;
      protected IRegisterAllVisitor _registerTask;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _registerTask = A.Fake<IRegisterAllVisitor>();
         sut = new MoBiApplicationController(_container, _eventPublisher, new ObjectTypeResolver(), _registerTask);
      }
   }

   internal class When_Opening_a_project_item : concern_for_ApplicationControllerSpecs
   {
      private IMoleculeBuildingBlock _projectItem;
      private IEditMoleculeBuildingBlockPresenter _editPresenter;
      private ISingleStartPresenter _result;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _projectItem = new MoleculeBuildingBlock();
         _editPresenter = A.Fake<IEditMoleculeBuildingBlockPresenter>();
         _commandCollector = A.Fake<ICommandCollector>();
         A.CallTo(() => _container.Resolve<IEditMoleculeBuildingBlockPresenter>()).Returns(_editPresenter);
      }

      protected override void Because()
      {
         _result = sut.Open(_projectItem, _commandCollector);
      }

      [Observation]
      public void should_get_edit_presenter_from_container()
      {
         A.CallTo(() => _container.Resolve<IEditMoleculeBuildingBlockPresenter>()).MustHaveHappened();
      }

      [Observation]
      public void should_initialise_edit_presenter_right()
      {
         A.CallTo(() => _editPresenter.InitializeWith(_commandCollector)).MustHaveHappened();
      }

      [Observation]
      public void should_return_the_new_and_right_presenter()
      {
         _result.ShouldBeEqualTo(_editPresenter);
      }


   }

   internal class When_selecting_an_Object_Base : concern_for_ApplicationControllerSpecs
   {
      private IObjectBase _selectedObject;
      private IObjectBase _projectItem;
      private IEditMoleculeBuildingBlockPresenter _editPresenter;

      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _projectItem = new MoleculeBuildingBlock();
         _editPresenter = A.Fake<IEditMoleculeBuildingBlockPresenter>();
         _commandCollector = A.Fake<ICommandCollector>();
         A.CallTo(() => _container.Resolve<IEditMoleculeBuildingBlockPresenter>()).Returns(_editPresenter);
         _selectedObject = A.Fake<IMoleculeBuilder>();
      }

      protected override void Because()
      {
         sut.Select(_selectedObject, _projectItem, _commandCollector);
      }


      [Observation]
      public void should_register_the_project_item()
      {
         A.CallTo(() => _registerTask.RegisterAllIn(_projectItem)).MustHaveHappened();
      }

      [Observation]
      public void should_get_edit_presenter_from_container()
      {
         A.CallTo(() => _container.Resolve<IEditMoleculeBuildingBlockPresenter>()).MustHaveHappened();
      }

      [Observation]
      public void should_initialise_edit_presenter_right()
      {
         A.CallTo(() => _editPresenter.Edit(_projectItem)).MustHaveHappened();
      }

      [Observation]
      public void should_raise_EntitySelectedEvent()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<EntitySelectedEvent>._)).MustHaveHappened();
      }
   }

   internal class When_selecting_null : concern_for_ApplicationControllerSpecs
   {
      private IObjectBase _selectedObject;
      private IMoleculeBuildingBlock _projectItem;
      private IEditMoleculeBuildingBlockPresenter _editPresenter;

      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _projectItem = new MoleculeBuildingBlock();
         _editPresenter = A.Fake<IEditMoleculeBuildingBlockPresenter>();
         _commandCollector = A.Fake<ICommandCollector>();
         A.CallTo(() => _container.Resolve<IEditMoleculeBuildingBlockPresenter>()).Returns(_editPresenter);
         _selectedObject = null;
      }

      protected override void Because()
      {
         sut.Select(_selectedObject, _projectItem, _commandCollector);
      }

      [Observation]
      public void should_not_get_edit_presenter_from_container()
      {
         A.CallTo(() => _container.Resolve<IEditMoleculeBuildingBlockPresenter>()).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_initialise_edit_presenter_right()
      {
         A.CallTo(() => _editPresenter.Edit(_projectItem)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_raise_EntitySelectedEvent()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<EntitySelectedEvent>._)).MustNotHaveHappened();
      }
   }

   internal class When_no_project_item_is_provided_for_selection : concern_for_ApplicationControllerSpecs
   {
      private IObjectBase _selectedObject;
      private IMoleculeBuildingBlock _projectItem;
      private IEditMoleculeBuildingBlockPresenter _editPresenter;

      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _projectItem = null;
         _editPresenter = A.Fake<IEditMoleculeBuildingBlockPresenter>();
         _commandCollector = A.Fake<ICommandCollector>();
         A.CallTo(() => _container.Resolve<IEditMoleculeBuildingBlockPresenter>()).Returns(_editPresenter);
         _selectedObject = A.Fake<IParameter>();
      }

      protected override void Because()
      {
         sut.Select(_selectedObject, _projectItem, _commandCollector);
      }

      [Observation]
      public void should_do_nothing()
      {
         A.CallTo(() => _container.Resolve<IEditMoleculeBuildingBlockPresenter>()).MustNotHaveHappened();
         A.CallTo(() => _editPresenter.Edit(_projectItem)).MustNotHaveHappened();
         A.CallTo(() => _eventPublisher.PublishEvent(A<EntitySelectedEvent>._)).MustNotHaveHappened();
      }
   }
}