using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesInSpatialStructurePresenter : IEditFavoritesPresenter<IBuildingBlock<IContainer>>
   {
   }

   internal class EditFavoritesInSpatialStructurePresenter : EditFavoritesInBuildindBlockPresenter<IContainer>,
      IEditFavoritesInSpatialStructurePresenter
   {
      public EditFavoritesInSpatialStructurePresenter(IEditFavoritesView view, IFavoriteRepository favoriteRepository, IEntityPathResolver entityPathResolver, IEditParameterListPresenter editParameterListPresenter, IFavoriteTask favoriteTask)
         : base(view, favoriteRepository, entityPathResolver, editParameterListPresenter, favoriteTask)
      {
      }
   }
}