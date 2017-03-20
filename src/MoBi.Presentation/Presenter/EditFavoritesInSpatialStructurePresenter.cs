using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesInSpatialStructurePresenter : IEditFavoritesPresenter<IBuildingBlock<IContainer>>
   {
   }

   internal class EditFavoritesInSpatialStructurePresenter : EditFavoritesInBuildindBlockPresenter<IContainer>,
      IEditFavoritesInSpatialStructurePresenter
   {
      public EditFavoritesInSpatialStructurePresenter(IEditFavoritesView view, IQuantityTask quantityTask, IInteractionTaskContext interactionTaskContext, IFormulaToFormulaBuilderDTOMapper formulaMapper, IParameterToFavoriteParameterDTOMapper favoriteMapper, IFavoriteRepository favoriteRepository, IInteractionTasksForParameter parameterTask, IFavoriteTask favoriteTask, IEntityPathResolver entityPathResolver, IViewItemContextMenuFactory contextMenuFactory)
         : base(view, quantityTask, interactionTaskContext, formulaMapper, favoriteMapper,
            favoriteRepository, parameterTask, favoriteTask, entityPathResolver, contextMenuFactory)
      {
      }
   }
}