using System.Linq;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public class EditFavoritesInBuildindBlockPresenter<T> : EditFavoritesPresenter<IBuildingBlock<T>>
      where T : class, IContainer
   {
      public EditFavoritesInBuildindBlockPresenter(IEditFavoritesView view, IFavoriteRepository favoriteRepository, IEntityPathResolver entityPathResolver, IEditParameterListPresenter editParameterListPresenter, IFavoriteTask favoriteTask)
         : base(view, favoriteRepository, entityPathResolver, editParameterListPresenter, favoriteTask)
      {
      }

      public override void Edit(IBuildingBlock<T> buildingBlock)
      {
         base.Edit(buildingBlock);
         _editParameterListPresenter.BuildingBlock = buildingBlock;
      }

      protected override void CacheParameters(IBuildingBlock<T> buildingBlock)
      {
         foreach (var builder in buildingBlock)
         {
            _parameterCache.AddRange(builder.GetAllChildren<IParameter>());
         }
      }

      protected override bool IsAddedToParent(IObjectBase parent)
      {
         var typedParent = parent as T;
         return typedParent != null && _projectItem.Any(builder => builder.GetAllContainersAndSelf<IContainer>().Contains(typedParent));
      }
   }
}