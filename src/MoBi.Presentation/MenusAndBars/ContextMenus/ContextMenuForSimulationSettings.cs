using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForSimulationSettings : ContextMenuBase
   {
      private readonly List<IMenuBarItem> _allMenuItems = new List<IMenuBarItem>();
      private readonly IContainer _container;

      public ContextMenuForSimulationSettings(IContainer container)
      {
         _container = container;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems() => _allMenuItems;

      public IContextMenu InitializeWith(SimulationSettingsViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.RefreshSettingsFromProjectDefault)
            .WithIcon(ApplicationIcons.Refresh)
            .WithCommandFor<RefreshSimulationSettingsUICommand, SimulationSettings>(viewItem.SimulationSettings, _container));

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.MakeSettingsProjectDefault)
            .WithIcon(ApplicationIcons.Commit)
            .WithCommandFor<CommitSimulationSettingsUICommand, SimulationSettings>(viewItem.SimulationSettings, _container));

         return this;
      }
   }

   public class ContextMenuSpecificationFactoryForSimulationSettingsViewItem : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForSimulationSettingsViewItem(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForSimulationSettings(_container);
         return contextMenu.InitializeWith(viewItem.DowncastTo<SimulationSettingsViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter) => viewItem is SimulationSettingsViewItem;
   }
}