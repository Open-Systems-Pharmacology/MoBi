using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public class concern_for_EventGroupListPresenter : ContextSpecification<EventGroupListPresenter>
   {
      protected ITreeNodeFactory _treeNodeFactory;
      protected IEventGroupsListView _view;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IApplicationBuilderToApplicationBuilderDTOMapper _applicationBuilderToDTOApplicationBuilderMapper;
      protected IMoBiContext _context;
      protected IEventGroupBuilderToEventGroupBuilderDTOMapper _eventGroupBuilderDTOMapper;

      protected override void Context()
      {
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();
         _view = A.Fake<IEventGroupsListView>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _applicationBuilderToDTOApplicationBuilderMapper = A.Fake<IApplicationBuilderToApplicationBuilderDTOMapper>();
         _context = A.Fake<IMoBiContext>();
         _eventGroupBuilderDTOMapper = A.Fake<IEventGroupBuilderToEventGroupBuilderDTOMapper>();

         sut = new EventGroupListPresenter(_view, _eventGroupBuilderDTOMapper, _viewItemContextMenuFactory, _applicationBuilderToDTOApplicationBuilderMapper, _context, _treeNodeFactory);
      }
   }

   public class When_handling_a_remove_event_for_the_edited_building_block : concern_for_EventGroupListPresenter
   {
      private RemovedEvent _removedEvent;
      private EventGroupBuildingBlock _editedBuildingBlock;

      protected override void Context()
      {
         base.Context();
         IObjectBase removedObject = new EventGroup();
         _editedBuildingBlock = new EventGroupBuildingBlock();
         _removedEvent = new RemovedEvent(removedObject, _editedBuildingBlock);
         sut.Edit(_editedBuildingBlock);
      }

      protected override void Because()
      {
         sut.Handle(_removedEvent);
      }

      [Observation]
      public void should_edit_the_building_block_a_second_time()
      {
         A.CallTo(() => _view.Show(A<IReadOnlyList<EventGroupBuilderDTO>>._)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_handling_a_remove_event_for_another_building_block : concern_for_EventGroupListPresenter
   {
      private RemovedEvent _removedEvent;
      private EventGroupBuildingBlock _eventGroupBuildingBlock;
      private EventGroupBuildingBlock _editedBuildingBlock;

      protected override void Context()
      {
         base.Context();
         IObjectBase removedObject = new EventGroup();
         _eventGroupBuildingBlock = new EventGroupBuildingBlock();
         _removedEvent = new RemovedEvent(removedObject, _eventGroupBuildingBlock);
         _editedBuildingBlock = new EventGroupBuildingBlock();
         sut.Edit(_editedBuildingBlock);
      }

      protected override void Because()
      {
         sut.Handle(_removedEvent);
      }

      [Observation]
      public void should_not_edit_the_building_block_a_second_time()
      {
         A.CallTo(() => _view.Show(A<IReadOnlyList<EventGroupBuilderDTO>>._)).MustHaveHappenedOnceExactly();
      }
   }



   public class When_handling_an_add_event_for_the_edited_building_block : concern_for_EventGroupListPresenter
   {
      private AddedEvent _addedEvent;
      private EventGroupBuildingBlock _editedBuildingBlock;

      protected override void Context()
      {
         base.Context();
         var eventGroup = new EventGroupBuilder();
         var addedObject = new EventBuilder();
         eventGroup.Add(addedObject);
         _editedBuildingBlock = new EventGroupBuildingBlock { eventGroup };
         _addedEvent = new AddedEvent<EventBuilder>(addedObject, eventGroup);
         sut.Edit(_editedBuildingBlock);
      }

      protected override void Because()
      {
         sut.Handle(_addedEvent);
      }

      [Observation]
      public void should_edit_the_building_block_a_second_time()
      {
         A.CallTo(() => _view.Show(A<IReadOnlyList<EventGroupBuilderDTO>>._)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_handling_an_add_event_for_another_building_block : concern_for_EventGroupListPresenter
   {
      private AddedEvent _addedEvent;
      private EventGroupBuildingBlock _eventGroupBuildingBlock;
      private EventGroupBuildingBlock _editedBuildingBlock;

      protected override void Context()
      {
         base.Context();
         var eventGroup = new EventGroupBuilder();
         var addedObject = new EventBuilder();
         eventGroup.Add(addedObject);
         _eventGroupBuildingBlock = new EventGroupBuildingBlock { eventGroup };
         _addedEvent = new AddedEvent<EventBuilder>(addedObject, eventGroup);
         _editedBuildingBlock = new EventGroupBuildingBlock();
         sut.Edit(_editedBuildingBlock);
      }

      protected override void Because()
      {
         sut.Handle(_addedEvent);
      }

      [Observation]
      public void should_not_edit_the_building_block_a_second_time()
      {
         A.CallTo(() => _view.Show(A<IReadOnlyList<EventGroupBuilderDTO>>._)).MustHaveHappenedOnceExactly();
      }
   }
}
