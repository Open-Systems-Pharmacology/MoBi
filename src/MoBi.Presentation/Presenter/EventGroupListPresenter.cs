using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public interface IEventGroupListPresenter : IEditPresenter<IEventGroupBuildingBlock>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>
   {
      void Select(IObjectBase objectBase);
      void Select(IObjectBaseDTO objectBaseDTO);
   }

   public class EventGroupListPresenter : AbstractEditPresenter<IEventGroupsListView, IEventGroupListPresenter, IEventGroupBuildingBlock>, IEventGroupListPresenter
   {
      private IEventGroupBuildingBlock _eventGroupBuildingBlock;
      private readonly IEventGroupBuilderToDTOEventGroupBuilderMapper _eventGroupToDTOEventGroupMapper;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IApplicationBuilderToDTOApplicationBuilderMapper _applicationBuilderToDTOApplicationBuilderMapper;
      private readonly IMoBiContext _context;
      private readonly ITreeNode _favorites;

      public EventGroupListPresenter(IEventGroupsListView view, IEventGroupBuilderToDTOEventGroupBuilderMapper eventGroupToDTOEventGroupMapper, IViewItemContextMenuFactory viewItemContextMenuFactory, IApplicationBuilderToDTOApplicationBuilderMapper applicationBuilderToDTOApplicationBuilderMapper, IMoBiContext context, ITreeNodeFactory treeNodeFactory) : base(view)
      {
         _context = context;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _eventGroupToDTOEventGroupMapper = eventGroupToDTOEventGroupMapper;
         _applicationBuilderToDTOApplicationBuilderMapper = applicationBuilderToDTOApplicationBuilderMapper;
         _favorites = treeNodeFactory.CreateGroupFavorites();
      }

      public override void Edit(IEventGroupBuildingBlock objectToEdit)
      {
         _eventGroupBuildingBlock = objectToEdit;
         var events = _eventGroupBuildingBlock.Where(item => !item.IsAnImplementationOf<IApplicationBuilder>()).MapAllUsing(
            _eventGroupToDTOEventGroupMapper).ToList();

         var applications = from groupBuilder in _eventGroupBuildingBlock
            where groupBuilder.IsAnImplementationOf<IApplicationBuilder>()
            let applicationBuilder = groupBuilder as IApplicationBuilder
            select applicationBuilder;

         var dtoApplications = applications.MapAllUsing(_applicationBuilderToDTOApplicationBuilderMapper);
         dtoApplications.Each(events.Add);
         _view.Clear();
         _view.AddNode(_favorites);
         _view.Show(events);
      }

      public void Select(IObjectBaseDTO dtoObjectBase)
      {
         if (dtoObjectBase.Equals(_favorites.TagAsObject))
            raiseFavoritesSelectedEvent();
         else
            raiseEntitySelectedEvent(dtoObjectBase);
      }

      private void raiseFavoritesSelectedEvent()
      {
         _context.PublishEvent(new FavoritesSelectedEvent(_eventGroupBuildingBlock));
      }

      private void raiseEntitySelectedEvent(IObjectBaseDTO dtoObjectBase)
      {
         var objectBase = _context.Get<IObjectBase>(dtoObjectBase.Id);
         Select(objectBase);
      }

      public void Select(IObjectBase objectBase)
      {
         _context.PublishEvent(new EntitySelectedEvent(objectBase, this));
      }

      public override object Subject
      {
         get { return _eventGroupBuildingBlock; }
      }

      public void InitializeWith(IEnumerable<IEventGroupBuilder> initializer)
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
         if (_eventGroupBuildingBlock == null) return;
         this.DoWithinExceptionHandler(() =>
         {
            if (shouldShow(eventToHandle.Parent))
            {
               Edit(_eventGroupBuildingBlock);
            }
         });
      }

      private bool shouldShow(IObjectBase testObject)
      {
         return testObject.IsAnImplementationOf<IEventGroupBuilder>()
                || testObject.IsAnImplementationOf<IEventBuilder>()
                || testObject.IsAnImplementationOf<ITransportBuilder>()
                || testObject.IsAnImplementationOf<IContainer>();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_eventGroupBuildingBlock == null) return;
         if (eventToHandle.RemovedObjects.Any(shouldShow))
         {
            Edit(_eventGroupBuildingBlock);
         }
      }
   }
}