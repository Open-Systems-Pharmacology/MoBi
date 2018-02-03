using System.Linq;
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
   public class EditFavoritesInBuildindBlockPresenter<T> : EditFavoritesPresenter<IBuildingBlock<T>>
      where T : class, IContainer
   {
      public EditFavoritesInBuildindBlockPresenter(IEditParameterListView view, IQuantityTask quantityTask, IInteractionTaskContext interactionTaskContext, IFormulaToFormulaBuilderDTOMapper formulaMapper, IParameterToParameterDTOMapper parameterDTOMapper, IFavoriteRepository favoriteRepository, IInteractionTasksForParameter parameterTask, IFavoriteTask favoriteTask, IEntityPathResolver entityPathResolver, IViewItemContextMenuFactory contextMenuFactory)
         : base(
            view, quantityTask, interactionTaskContext, formulaMapper, parameterDTOMapper, favoriteRepository, parameterTask,
            favoriteTask, entityPathResolver, contextMenuFactory)
      {
      }

      public override void Edit(IBuildingBlock<T> buildingBlock)
      {
         base.Edit(buildingBlock);
         BuildingBlock = buildingBlock;
      }

      protected override void CacheParameters(IBuildingBlock<T> projectItem)
      {
         foreach (var topBuilder in projectItem)
         {
            _parameterCache.AddRange(topBuilder.GetAllChildren<IParameter>());
         }
      }

      protected override bool IsAddedToParent(IObjectBase parent)
      {
         var typedParent = parent as T;
         return typedParent != null && _projectItem.Any(topBuilder => topBuilder.GetAllContainersAndSelf<IContainer>().Contains(typedParent));
      }
   }
}