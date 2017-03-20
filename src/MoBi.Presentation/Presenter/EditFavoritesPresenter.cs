using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesPresenter : IParameterPresenter,
      IListener<AddParameterToFavoritesEvent>,
      IListener<RemoveParameterFromFavoritesEvent>,
      IListener<FavoritesLoadedEvent>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>,
      IPresenterWithContextMenu<IViewItem>
   {
      void Select(FavoriteParameterDTO parameterDTO);
      void GoTo(FavoriteParameterDTO favoriteParameterDTO);
      Func<IObjectBase, bool> ShouldHandleRemovedEvent { set; }
   }

   public interface IEditFavoritesPresenter<T> : IEditFavoritesPresenter, IEditPresenter<T> where T : IObjectBase
   {
   }

   public abstract class EditFavoritesPresenter<T> : AbstractParameterBasePresenter<IEditFavoritesView, IEditFavoritesPresenter>,
      IEditFavoritesPresenter<T> where T : IObjectBase
   {
      private readonly IParameterToFavoriteParameterDTOMapper _favoriteMapper;
      protected readonly List<FavoriteParameterDTO> _favorites;
      private readonly IFavoriteRepository _favoriteRepository;
      protected Cache<string, IParameter> _parameterCache;
      protected readonly IEntityPathResolver _entityPathResolver;
      private readonly IMoBiContext _context;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected T _projectItem;

      protected EditFavoritesPresenter(IEditFavoritesView view, IQuantityTask quantityTask,
         IInteractionTaskContext interactionTaskContext, IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IParameterToFavoriteParameterDTOMapper favoriteMapper, IFavoriteRepository favoriteRepository,
         IInteractionTasksForParameter parameterTask, IFavoriteTask favoriteTask, IEntityPathResolver entityPathResolver, IViewItemContextMenuFactory viewItemContextMenuFactory)
         : base(
            view, quantityTask, interactionTaskContext,
            formulaMapper, parameterTask, favoriteTask)
      {
         _favoriteMapper = favoriteMapper;
         _favoriteRepository = favoriteRepository;
         _entityPathResolver = entityPathResolver;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _parameterCache = new Cache<string, IParameter>(p => _entityPathResolver.PathFor(p), s => null);
         _favorites = new List<FavoriteParameterDTO>();
         _context = interactionTaskContext.Context;
      }

      protected abstract void CacheParameters(T projectItem);

      protected override void UpdateView(IParameterDTO parameterDTO)
      {
         updateFavorites();
         Select(parameterDTO.DowncastTo<FavoriteParameterDTO>());
      }

      public void Select(FavoriteParameterDTO parameterDTO)
      {
         _view.Select(parameterDTO);
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         releaseFavorites();
      }

      public void GoTo(FavoriteParameterDTO favoriteParameterDTO)
      {
         if (favoriteParameterDTO == null)
            return;

         var parameter = GetParameterFrom(favoriteParameterDTO);
         _context.PublishEvent(new EntitySelectedEvent(parameter, this));
      }

      public void Handle(AddParameterToFavoritesEvent eventToHandle)
      {
         updateFavorites();
      }

      public void Handle(RemoveParameterFromFavoritesEvent eventToHandle)
      {
         updateFavorites();
      }

      public virtual void Edit(T projectItem)
      {
         _projectItem = projectItem;
         refresh();
      }

      private void updateFavorites()
      {
         releaseFavorites();
         _favorites.Clear();

         foreach (var path in _favoriteRepository.All())
         {
            var parameter = _parameterCache[path];
            if (parameter != null)
               _favorites.Add(_favoriteMapper.MapFrom(parameter));
         }

         EnumHelper.AllValuesFor<PathElement>().Each(updateColumnVisibility);
         UpdateSpecialColumnsVisibility();
         _view.Show(_favorites);
      }

      private void releaseFavorites()
      {
         _favorites.Each(dto => dto.Release());
      }

      protected virtual void UpdateSpecialColumnsVisibility()
      {
         //name always visible
         _view.SetVisibility(PathElement.Name, isVisible: true);
      }

      private void updateColumnVisibility(PathElement pathElement)
      {
         _view.SetVisibility(pathElement, !_favorites.HasOnlyEmptyValuesAt(pathElement));
      }

      public object Subject => _projectItem;

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<T>());
      }

      public void ShowContextMenu(IViewItem favorite, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(favorite, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Handle(FavoritesLoadedEvent eventToHandle)
      {
         updateFavorites();
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (ShouldHandleEvent(eventToHandle.Parent))
         {
            refresh();
         }
      }

      private void refresh()
      {
         _parameterCache.Clear();
         CacheParameters(_projectItem);
         updateFavorites();
      }

      protected bool ShouldHandleEvent(IObjectBase parent)
      {
         return Equals(_projectItem, parent) || IsAddedToParent(parent);
      }

      protected abstract bool IsAddedToParent(IObjectBase parent);

      public void Handle(RemovedEvent eventToHandle)
      {
         var removedObjects = eventToHandle.RemovedObjects.ToList();

         if (!removedObjects.Any(ShouldHandleRemovedEvent)) return;
         
         refresh();
      }

      public Func<IObjectBase, bool> ShouldHandleRemovedEvent { protected get; set; }
   }
}