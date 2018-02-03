using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesPresenter : IEditParameterListPresenter,
      IListener<AddParameterToFavoritesEvent>,
      IListener<RemoveParameterFromFavoritesEvent>,
      IListener<FavoritesLoadedEvent>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>
   {
      void Select(ParameterDTO parameterDTO);
      Func<IObjectBase, bool> ShouldHandleRemovedEvent { set; }
   }

   public interface IEditFavoritesPresenter<T> : IEditFavoritesPresenter, IEditPresenter<T> where T : IObjectBase
   {
   }

   public abstract class EditFavoritesPresenter<T> : AbstractParameterBasePresenter<IEditParameterListView, IEditParameterListPresenter>,
      IEditFavoritesPresenter<T> where T : IObjectBase
   {
      private readonly IParameterToParameterDTOMapper _parameterDTOMapper;
      protected readonly List<ParameterDTO> _favorites;
      private readonly IFavoriteRepository _favoriteRepository;
      protected Cache<string, IParameter> _parameterCache;
      protected readonly IEntityPathResolver _entityPathResolver;
      private readonly IMoBiContext _context;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected T _projectItem;
      public Func<IObjectBase, bool> ShouldHandleRemovedEvent { protected get; set; }

      protected EditFavoritesPresenter(IEditParameterListView view, IQuantityTask quantityTask,
         IInteractionTaskContext interactionTaskContext, IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IParameterToParameterDTOMapper parameterDTOMapper, IFavoriteRepository favoriteRepository,
         IInteractionTasksForParameter parameterTask, IFavoriteTask favoriteTask, IEntityPathResolver entityPathResolver, IViewItemContextMenuFactory viewItemContextMenuFactory)
         : base(
            view, quantityTask, interactionTaskContext,
            formulaMapper, parameterTask, favoriteTask)
      {
         _parameterDTOMapper = parameterDTOMapper;
         _favoriteRepository = favoriteRepository;
         _entityPathResolver = entityPathResolver;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _parameterCache = new Cache<string, IParameter>(p => _entityPathResolver.PathFor(p), s => null);
         _favorites = new List<ParameterDTO>();
         _context = interactionTaskContext.Context;
      }

      protected abstract void CacheParameters(T projectItem);

      protected override void RefreshViewAndSelect(IParameterDTO parameterDTO)
      {
         updateFavorites();
         Select(parameterDTO.DowncastTo<ParameterDTO>());
      }

      public void Select(ParameterDTO parameterDTO)
      {
         _view.Select(parameterDTO);
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         releaseFavorites();
      }

      public void GoTo(ParameterDTO parameterDTO)
      {
         if (parameterDTO == null)
            return;

         var parameter = GetParameterFrom(parameterDTO);
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

         var allFavoritesParameters = _favoriteRepository.All().Select(path => _parameterCache[path]).Where(p => p != null);
         _favorites.AddRange(allFavoritesParameters.Select(favoriteDTOFrom));

         EnumHelper.AllValuesFor<PathElement>().Each(updateColumnVisibility);
         UpdateSpecialColumnsVisibility();
         _view.BindTo(_favorites);
      }

      private ParameterDTO favoriteDTOFrom(IParameter parameter)
      {
         return _parameterDTOMapper.MapFrom(parameter).DowncastTo<ParameterDTO>();
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
         if (!ShouldHandleEvent(eventToHandle.Parent))
            return;

         refresh();
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

         if (!removedObjects.Any(ShouldHandleRemovedEvent))
            return;

         refresh();
      }
   }
}