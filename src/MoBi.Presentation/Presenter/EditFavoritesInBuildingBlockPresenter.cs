using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public class EditFavoritesInBuildingBlockPresenter<TBuildingBlock, TBuilder> : EditFavoritesPresenter<TBuildingBlock>
      where TBuilder : class, IContainer where TBuildingBlock : IBuildingBlock, IEnumerable<TBuilder>
   {
      public EditFavoritesInBuildingBlockPresenter(IEditFavoritesView view, IFavoriteRepository favoriteRepository, IEntityPathResolver entityPathResolver, IEditParameterListPresenter editParameterListPresenter, IFavoriteTask favoriteTask)
         : base(view, favoriteRepository, entityPathResolver, editParameterListPresenter, favoriteTask)
      {
      }

      public override void Edit(TBuildingBlock buildingBlock)
      {
         base.Edit(buildingBlock);
         _editParameterListPresenter.BuildingBlock = buildingBlock;
      }

      protected override void CacheParameters(TBuildingBlock buildingBlock)
      {
         foreach (var builder in buildingBlock)
         {
            _parameterCache.AddRange(builder.GetAllChildren<IParameter>());
         }
      }

      protected override bool IsAddedToParent(IObjectBase parent)
      {
         var typedParent = parent as TBuilder;
         return typedParent != null && _projectItem.Any(builder => builder.GetAllContainersAndSelf<IContainer>().Contains(typedParent));
      }
   }
}