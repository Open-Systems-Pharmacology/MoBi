using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public interface IEventGroupListPresenter : IEditPresenter<EventGroupBuildingBlock>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>
   {
      void Select(IObjectBase objectBase);
      void Select(ObjectBaseDTO objectBaseDTO);
   }

   public class EventGroupListPresenter : AbstractEditPresenter<IEventGroupsListView, IEventGroupListPresenter, EventGroupBuildingBlock>, IEventGroupListPresenter
   {
      private EventGroupBuildingBlock _eventGroupBuildingBlock;
      private readonly IEventGroupBuilderToEventGroupBuilderDTOMapper _eventGroupBuilderDTOMapper;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IApplicationBuilderToApplicationBuilderDTOMapper _applicationBuilderToDTOApplicationBuilderMapper;
      private readonly IMoBiContext _context;
      private readonly ITreeNode _favoritesNodes;
      private readonly ITreeNode _userDefinedNodes;

      public EventGroupListPresenter(IEventGroupsListView view, IEventGroupBuilderToEventGroupBuilderDTOMapper eventGroupBuilderDTOMapper, IViewItemContextMenuFactory viewItemContextMenuFactory, IApplicationBuilderToApplicationBuilderDTOMapper applicationBuilderToDTOApplicationBuilderMapper, IMoBiContext context, ITreeNodeFactory treeNodeFactory) : base(view)
      {
         _context = context;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _eventGroupBuilderDTOMapper = eventGroupBuilderDTOMapper;
         _applicationBuilderToDTOApplicationBuilderMapper = applicationBuilderToDTOApplicationBuilderMapper;
         _favoritesNodes = treeNodeFactory.CreateForFavorites();
         _userDefinedNodes = treeNodeFactory.CreateForUserDefined();
      }

      public override void Edit(EventGroupBuildingBlock eventGroupBuildingBlock)
      {
         _eventGroupBuildingBlock = eventGroupBuildingBlock;

         var events = _eventGroupBuildingBlock
            .Where(item => !item.IsAnImplementationOf<ApplicationBuilder>())
            .MapAllUsing(_eventGroupBuilderDTOMapper)
            .ToList();

         var applications = from groupBuilder in _eventGroupBuildingBlock
            where groupBuilder.IsAnImplementationOf<ApplicationBuilder>()
            let applicationBuilder = groupBuilder as ApplicationBuilder
            select applicationBuilder;

         var applicationDTOList = applications.MapAllUsing(_applicationBuilderToDTOApplicationBuilderMapper);
         events.AddRange(applicationDTOList);

         _view.Clear();
         _view.AddNode(_favoritesNodes);
         _view.AddNode(_userDefinedNodes);
         _view.Show(events);
      }

      public void Select(ObjectBaseDTO objectBaseDTO)
      {
         if (objectBaseDTO.Equals(_favoritesNodes.TagAsObject))
            raiseFavoritesSelectedEvent();
         else if (objectBaseDTO.Equals(_userDefinedNodes.TagAsObject))
            raiseUserDefinedSelectedEvent();
         else
            raiseEntitySelectedEvent(objectBaseDTO);
      }

      private void raiseFavoritesSelectedEvent()
      {
         _context.PublishEvent(new FavoritesSelectedEvent(_eventGroupBuildingBlock));
      }

      private void raiseUserDefinedSelectedEvent()
      {
         _context.PublishEvent(new UserDefinedSelectedEvent(_eventGroupBuildingBlock));
      }

      private void raiseEntitySelectedEvent(ObjectBaseDTO dtoObjectBase)
      {
         var objectBase = _context.Get<IObjectBase>(dtoObjectBase.Id);
         Select(objectBase);
      }

      public void Select(IObjectBase objectBase)
      {
         _context.PublishEvent(new EntitySelectedEvent(objectBase, this));
      }

      public override object Subject => _eventGroupBuildingBlock;

      public void InitializeWith(IEnumerable<EventGroupBuilder> initializer)
      {
         Edit(initializer);
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_eventGroupBuildingBlock == null)
            return;

         if (!shouldShow(eventToHandle.Parent))
            return;

         Edit(_eventGroupBuildingBlock);
      }

      private bool shouldShow(IObjectBase testObject)
      {
         return testObject.IsAnImplementationOf<EventGroupBuilder>()
                || testObject.IsAnImplementationOf<EventBuilder>()
                || testObject.IsAnImplementationOf<TransportBuilder>()
                || testObject.IsAnImplementationOf<IContainer>();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_eventGroupBuildingBlock == null)
            return;

         if (!eventToHandle.RemovedObjects.Any(shouldShow))
            return;

         Edit(_eventGroupBuildingBlock);
      }
   }
}