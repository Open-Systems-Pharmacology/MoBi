using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForFavorite : ContextMenuBase
   {
      private readonly ParameterDTO _favoriteParameterDTO;
      private readonly IEditFavoritesPresenter _presenter;

      public ContextMenuForFavorite(ParameterDTO favoriteParameterDTO, IEditFavoritesPresenter presenter)
      {
         _favoriteParameterDTO = favoriteParameterDTO;
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.GoTo)
            .WithActionCommand(() => _presenter.GoTo(_favoriteParameterDTO))
            .WithIcon(ApplicationIcons.GoTo);
      }
   }

   public class ContextMenuForFavoriteFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForFavorite(viewItem.DowncastTo<ParameterDTO>(), presenter.DowncastTo<IEditFavoritesPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<ParameterDTO>() &&
                presenter.IsAnImplementationOf<IEditFavoritesPresenter>();
      }
   }
}