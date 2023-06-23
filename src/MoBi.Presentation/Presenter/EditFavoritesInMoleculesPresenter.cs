using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesInMoleculesPresenter : IEditFavoritesPresenter<MoleculeBuildingBlock>
   {
   }

   internal class EditFavoritesInMoleculesPresenter : EditFavoritesInBuildingBlockPresenter<MoleculeBuildingBlock, MoleculeBuilder>, IEditFavoritesInMoleculesPresenter
   {
      public EditFavoritesInMoleculesPresenter(IEditFavoritesView view, IFavoriteRepository favoriteRepository, IEntityPathResolver entityPathResolver, IEditParameterListPresenter editParameterListPresenter, IFavoriteTask favoriteTask)
         : base(view, favoriteRepository,entityPathResolver,editParameterListPresenter, favoriteTask)
      {
         UpdateSpecialColumnsVisibility = _editParameterListPresenter.ConfigureForMolecule;
      }
   }
}