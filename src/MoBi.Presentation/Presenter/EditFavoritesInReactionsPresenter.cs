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
   public interface IEditFavoritesInReactionsPresenter : IEditFavoritesPresenter<IBuildingBlock<IReactionBuilder>>
   {
   }

   internal class EditFavoritesInReactionsPresenter : EditFavoritesInBuildindBlockPresenter<IReactionBuilder>,
      IEditFavoritesInReactionsPresenter
   {
      public EditFavoritesInReactionsPresenter(IEditParameterListView view, IQuantityTask quantityTask, IInteractionTaskContext interactionTaskContext, IFormulaToFormulaBuilderDTOMapper formulaMapper, IParameterToParameterDTOMapper parameterDTOMapper, IFavoriteRepository favoriteRepository, IInteractionTasksForParameter parameterTask, IFavoriteTask favoriteTask, IEntityPathResolver entityPathResolver, IViewItemContextMenuFactory contextMenuFactory)
         : base(
            view, quantityTask, interactionTaskContext, formulaMapper, parameterDTOMapper, favoriteRepository, parameterTask,
            favoriteTask, entityPathResolver, contextMenuFactory)
      {
         UpdateSpecialColumnsVisibility = this.ConfigureForReaction;
      }
   }
}