using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForSimulationFavoritesNode : ContextMenuBase
   {
      private readonly IHierarchicalSimulationPresenter _presenter;
      private readonly IContainer _container;

      public ContextMenuForSimulationFavoritesNode(IHierarchicalSimulationPresenter hierarchicalSimulationPresenter, IContainer container)
      {
         _presenter = hierarchicalSimulationPresenter;
         _container = container;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(MenuNames.StartParameterIdentification)
            .WithCommandFor<CreateParameterIdentificationBasedOnParametersUICommand, IEnumerable<IParameter>>(_presenter.SimulationFavorites(), _container)
            .WithIcon(ApplicationIcons.ParameterIdentification);
      }
   }

   public class ContextMenuForSimulationFavoritesNodeFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuForSimulationFavoritesNodeFactory(IContainer container)
      {
         _container = container;
      }
      
      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForSimulationFavoritesNode(presenter.DowncastTo<IHierarchicalSimulationPresenter>(), _container);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<FavoritesNodeViewItem>() &&
                presenter.IsAnImplementationOf<IHierarchicalSimulationPresenter>();
      }
   }
}