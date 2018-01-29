using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
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
   public interface IMoleculeListPresenter : IEditPresenter<IMoleculeBuildingBlock>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<EntitySelectedEvent>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>,
      IListener<MoleculeIconChangedEvent>,
      IListener<BulkUpdateStartedEvent>,
      IListener<BulkUpdateFinishedEvent>
   {
      /// <summary>
      ///    Called by the view when item is selected
      /// </summary>
      /// <param name="dto"></param>
      void Select(IObjectBaseDTO dto);

      IMoleculeBuildingBlock MoleculeBuildingBlock { get; }
   }

   internal class MoleculeListPresenter :
      AbstractEditPresenter<IMoleculeListView, IMoleculeListPresenter, IMoleculeBuildingBlock>, IMoleculeListPresenter
   {
      private readonly IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderToDTOMoleculeBuilderMapper;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IMoBiContext _context;
      private bool _disableEventsForHeavyWork;
      private readonly ITreeNode _favorites;
      public IMoleculeBuildingBlock MoleculeBuildingBlock { get; private set; }

      public MoleculeListPresenter(IMoleculeListView view,
         IMoleculeBuilderToMoleculeBuilderDTOMapper moleculeBuilderToDTOMoleculeBuilderMapper,
         IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context, ITreeNodeFactory treeNodeFactory)
         : base(view)
      {
         _moleculeBuilderToDTOMoleculeBuilderMapper = moleculeBuilderToDTOMoleculeBuilderMapper;
         _context = context;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _favorites = treeNodeFactory.CreateForFavorites();
      }

      public override void Edit(IMoleculeBuildingBlock objectToEdit)
      {
         MoleculeBuildingBlock = objectToEdit;
         _view.Clear();
         _view.AddNode(_favorites);
         _view.Show(moleculesToBindTo());
      }

      private IEnumerable<MoleculeBuilderDTO> moleculesToBindTo()
      {
         return MoleculeBuildingBlock.OrderBy(x => x.Name).MapAllUsing(_moleculeBuilderToDTOMoleculeBuilderMapper);
      }

      public override object Subject
      {
         get { return MoleculeBuildingBlock; }
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public virtual void Select(IObjectBaseDTO dtoObjectBase)
      {
         if (dtoObjectBase.Equals(_favorites.TagAsObject))
            raiseFavoritesSelectedEvent();
         else
            raiseEntitySelectedEvent(dtoObjectBase);
      }

      private void raiseFavoritesSelectedEvent()
      {
         _context.PublishEvent(new FavoritesSelectedEvent(MoleculeBuildingBlock));
      }

      private void raiseEntitySelectedEvent(IObjectBaseDTO dtoObjectBase)
      {
         var objectBase = _context.Get<IObjectBase>(dtoObjectBase.Id);
         _context.PublishEvent(new EntitySelectedEvent(objectBase, this));
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_disableEventsForHeavyWork) return;
         if (MoleculeBuildingBlock == null) return;
         var addedObject = eventToHandle.AddedObject;
         if (!canHandle(addedObject)) return;
         Edit(MoleculeBuildingBlock);
         _view.SelectItem(addedObject);
      }

      private bool canHandle(IObjectBase obj)
      {
         return obj.CouldBeInMoleculeBuildingBlock();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_disableEventsForHeavyWork) return;

         if (MoleculeBuildingBlock == null) return;
         if (eventToHandle.RemovedObjects.Any(canHandle))
         {
            Edit(MoleculeBuildingBlock);
         }
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         if (MoleculeBuildingBlock == null) return;
         if (eventToHandle.Sender == this) return;
         var selectedObjectBase = eventToHandle.ObjectBase;
         if (!canHandle(selectedObjectBase))
            return;
         if (isParameterInMoleculeBuildingBlock(selectedObjectBase))
            _view.SelectItem(((IEntity) selectedObjectBase).ParentContainer);
         else
            _view.SelectItem(selectedObjectBase);
      }

      private bool isParameterInMoleculeBuildingBlock(IObjectBase selectedObjectBase)
      {
         return selectedObjectBase.IsAnImplementationOf<IParameter>() &&
                canHandle(((IEntity) selectedObjectBase).ParentContainer);
      }

      public void Handle(MoleculeIconChangedEvent eventToHandle)
      {
         if (MoleculeBuildingBlock == null) return;
         if (canHandle(eventToHandle.MoleculeBuilder))
         {
            Edit(MoleculeBuildingBlock);
         }
      }

      public void Handle(BulkUpdateStartedEvent eventToHandle)
      {
         _disableEventsForHeavyWork = true;
      }

      public void Handle(BulkUpdateFinishedEvent eventToHandle)
      {
         _disableEventsForHeavyWork = false;
         Edit(MoleculeBuildingBlock);
      }
   }
}