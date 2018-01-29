using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public abstract class EditBuildingBlockWithFavoriteAndUserDefinedPresenterBase<TView, TPresenter, TBuildingBlock, TBuilder> :
      EditBuildingBlockPresenterBase<TView, TPresenter, TBuildingBlock, TBuilder>,
      IListener<FavoritesSelectedEvent>,
      IListener<UserDefinedSelectedEvent>
      where TView : IView<TPresenter>, IEditBuildingBlockBaseView
      where TPresenter : IPresenter, ISingleStartPresenter
      where TBuildingBlock : IBuildingBlock<TBuilder>
      where TBuilder : class, IContainer

   {
      protected readonly IEditFavoritesPresenter<IBuildingBlock<TBuilder>> _favoritesPresenter;
      protected readonly IUserDefinedParametersPresenter _userDefinedParametersPresenter;

      protected EditBuildingBlockWithFavoriteAndUserDefinedPresenterBase(
         TView view,
         IFormulaCachePresenter formulaCachePresenter,
         IEditFavoritesPresenter<IBuildingBlock<TBuilder>> favoritesPresenter,
         IUserDefinedParametersPresenter userDefinedParametersPresenter) : base(view, formulaCachePresenter)
      {
         _favoritesPresenter = favoritesPresenter;
         _userDefinedParametersPresenter = userDefinedParametersPresenter;

         AddSubPresenters(favoritesPresenter, userDefinedParametersPresenter);
      }

      public void Handle(FavoritesSelectedEvent eventToHandle)
      {
         if (!CanHandle(eventToHandle))
            return;

         ShowView(_favoritesPresenter.BaseView);
      }

      public void Handle(UserDefinedSelectedEvent eventToHandle)
      {
         if (!CanHandle(eventToHandle))
            return;

         _userDefinedParametersPresenter.ShowUserDefinedParametersIn(BuildingBlock);
         ShowView(_userDefinedParametersPresenter.BaseView);
      }

      protected abstract void ShowView(IView viewToShow);
   }
}