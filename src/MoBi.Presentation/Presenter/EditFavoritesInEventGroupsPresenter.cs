using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesInEventGroupsPresenter : IEditFavoritesPresenter<EventGroupBuildingBlock>
   {
   }

   internal class EditFavoritesInEventGroupsPresenter : EditFavoritesInBuildingBlockPresenter<EventGroupBuildingBlock, EventGroupBuilder>, IEditFavoritesInEventGroupsPresenter
   {
      public EditFavoritesInEventGroupsPresenter(IEditFavoritesView view, IFavoriteRepository favoriteRepository, 
         IEntityPathResolver entityPathResolver, IEditParameterListPresenter editParameterListPresenter, IFavoriteTask favoriteTask) :
         base(view, favoriteRepository, entityPathResolver, editParameterListPresenter, favoriteTask)
      {
         UpdateSpecialColumnsVisibility = _editParameterListPresenter.ConfigureForEvent;
      }
   }
}