using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesPresenter : IPresenter,
      IListener<AddParameterToFavoritesEvent>,
      IListener<RemoveParameterFromFavoritesEvent>,
      IListener<FavoritesLoadedEvent>,
      IListener<FavoritesOrderChangedEvent>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>
   {
      Func<IObjectBase, bool> ShouldHandleRemovedEvent { set; }
      void MoveUp();
      void MoveDown();

   }

   public interface IEditFavoritesPresenter<T> : IEditFavoritesPresenter, IEditPresenter<T> where T : IObjectBase
   {
   }

   public abstract class EditFavoritesPresenter<T> : AbstractCommandCollectorPresenter<IEditFavoritesView, IEditFavoritesPresenter>,
      IEditFavoritesPresenter<T> where T : IObjectBase
   {
      private readonly IFavoriteRepository _favoriteRepository;
      protected Cache<string, IParameter> _parameterCache;
      protected readonly IEntityPathResolver _entityPathResolver;
      protected readonly IEditParameterListPresenter _editParameterListPresenter;
      private readonly IFavoriteTask _favoriteTask;
      protected T _projectItem;

      public Func<IObjectBase, bool> ShouldHandleRemovedEvent { protected get; set; }

      protected EditFavoritesPresenter(
         IEditFavoritesView view,
         IFavoriteRepository favoriteRepository,
         IEntityPathResolver entityPathResolver,
         IEditParameterListPresenter editParameterListPresenter,
         IFavoriteTask favoriteTask)
         : base(view)
      {
         _favoriteRepository = favoriteRepository;
         _entityPathResolver = entityPathResolver;
         _editParameterListPresenter = editParameterListPresenter;
         _favoriteTask = favoriteTask;
         _parameterCache = new Cache<string, IParameter>(pathFor, s => null);
         AddSubPresenters(_editParameterListPresenter);
         _view.AddParametersView(_editParameterListPresenter.BaseView);
      }

      private Func<IParameter, string> pathFor
      {
         get { return p => _entityPathResolver.PathFor(p); }
      }

      protected abstract void CacheParameters(T projectItem);

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
         var allFavoritesParameters = _favoriteRepository.All().Select(path => _parameterCache[path]).Where(p => p != null);
         _editParameterListPresenter.Edit(allFavoritesParameters);

         //name always visible
         _editParameterListPresenter.SetVisibility(PathElementId.Name, isVisible: true);

         UpdateSpecialColumnsVisibility();
      }

      protected virtual Action UpdateSpecialColumnsVisibility { get; set; } = () => { };

      public object Subject => _projectItem;

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<T>());
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

      public void MoveUp() => moveFavorites(x => x.MoveUp);

      public void MoveDown() => moveFavorites(x => x.MoveDown);

      private void moveFavorites(Func<IFavoriteTask, Action<IEnumerable<string>>> moveActionFunc)
      {
         var selectedParameters = _editParameterListPresenter.SelectedParameters;
         var moveAction = moveActionFunc(_favoriteTask);
         moveAction(selectedParameters.Select(pathFor));
         _editParameterListPresenter.SelectedParameters = selectedParameters;
      }

      public void Handle(FavoritesOrderChangedEvent eventToHandle)
      {
         updateFavorites();
      }
   }
}