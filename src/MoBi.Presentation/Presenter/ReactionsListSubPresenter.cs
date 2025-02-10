using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IReactionsListSubPresenter : IEditPresenter<ReactionBuildingBlock>,
      IPresenterWithContextMenu<IViewItem>,
      IPresenterWithContextMenu<ReactionInfoDTO>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>

   {
      void Select(string id);
   }

   public class ReactionsListSubPresenter : AbstractEditPresenter<IReactionListView, IReactionsListSubPresenter, ReactionBuildingBlock>, IReactionsListSubPresenter, IListener<AddedReactionPartnerEvent>, IListener<RemovedReactionPartnerEvent>, IListener<EditReactionPartnerEvent>
   {
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IReactionBuilderToReactionInfoDTOMapper _reactionBuilderBuilderToDtoReactionInfoMapper;
      private ReactionBuildingBlock _reactionBuildingBlock;
      private readonly IMoBiContext _context;
      private IEnumerable<ReactionInfoDTO> _dtoReactions;

      public ReactionsListSubPresenter(IReactionListView view, IReactionBuilderToReactionInfoDTOMapper reactionBuilderBuilderToDtoReactionInfoMapper, IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context) : base(view)
      {
         _reactionBuilderBuilderToDtoReactionInfoMapper = reactionBuilderBuilderToDtoReactionInfoMapper;
         _context = context;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Select(string id)
      {
         var objectBase = _context.Get<IObjectBase>(id);
         _context.PublishEvent(new EntitySelectedEvent(objectBase, this));
      }

      public void ShowContextMenu(ReactionInfoDTO objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public override void Edit(ReactionBuildingBlock objectToEdit)
      {
         _dtoReactions = objectToEdit.MapAllUsing(_reactionBuilderBuilderToDtoReactionInfoMapper);
         _view.Show(_dtoReactions);
         _reactionBuildingBlock = objectToEdit;
      }

      public override object Subject
      {
         get { return _reactionBuildingBlock; }
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_reactionBuildingBlock == null) return;
         if (_reactionBuildingBlock.Equals(eventToHandle.Parent))
         {
            Edit(_reactionBuildingBlock);
         }
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_reactionBuildingBlock == null) return;
         if (eventToHandle.RemovedObjects.Any(item => item.IsAnImplementationOf<ReactionBuilder>()))
         {
            Edit(_reactionBuildingBlock);
         }
      }

      public void Handle(AddedReactionPartnerEvent eventToHandle)
      {
         performBaseEventHandlingFor(eventToHandle.Reaction);
      }

      public void Handle(RemovedReactionPartnerEvent eventToHandle)
      {
         performBaseEventHandlingFor(eventToHandle.Reaction);
      }

      public void Handle(EditReactionPartnerEvent eventToHandle)
      {
         performBaseEventHandlingFor(eventToHandle.Reaction);
      }

      private void performBaseEventHandlingFor(ReactionBuilder reactionBuilder)
      {
         if (!canHandleEventFor(reactionBuilder))
            return;

         var dto = _dtoReactions.FindById(reactionBuilder.Id);
         var upadteDTO = _reactionBuilderBuilderToDtoReactionInfoMapper.MapFrom(reactionBuilder);
         dto.StoichiometricFormula = upadteDTO.StoichiometricFormula;
      }

      private bool canHandleEventFor(ReactionBuilder reactionBuilder)
      {
         if (_reactionBuildingBlock == null) return false;
         return _reactionBuildingBlock.Contains(reactionBuilder);
      }
   }
}