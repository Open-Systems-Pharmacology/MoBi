using System.Collections.Generic;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForSimulationFavoritesNode : ContextMenuBase
   {
      private readonly IHierarchicalSimulationPresenter _presenter;

      public ContextMenuForSimulationFavoritesNode(FavoritesBaseDTO favoriteDTO, IHierarchicalSimulationPresenter hierarchicalSimulationPresenter)
      {
         _presenter = hierarchicalSimulationPresenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(MenuNames.StartParameterIdentification)
            .WithCommandFor<CreateParameterIdentificationBasedOnParametersUICommand, IEnumerable<IParameter>>(_presenter.SimulationFavorites())
            .WithIcon(ApplicationIcons.ParameterIdentification);
      }
   }

   public class ContextMenuForSimulationFavoritesNodeFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForSimulationFavoritesNode(objectRequestingContextMenu.DowncastTo<FavoritesBaseDTO>(), presenter.DowncastTo<IHierarchicalSimulationPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<FavoritesBaseDTO>() && presenter.IsAnImplementationOf<IHierarchicalSimulationPresenter>();
      }
   }
}