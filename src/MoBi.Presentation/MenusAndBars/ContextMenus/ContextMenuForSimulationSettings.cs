using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
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

      public IContextMenu InitializeWith(SimulationSettingsDTO viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         _allMenuItems.Add(createRefreshSubMenu(viewItem));
         _allMenuItems.Add(createCommitSubMenu(viewItem));

         return this;
      }

      private IMenuBarItem createRefreshSubMenu(SimulationSettingsDTO viewItem)
      {
         return CreateSubMenu.WithCaption(AppConstants.MenuNames.RefreshFromProjectDefaults).WithIcon(ApplicationIcons.Refresh)
            .WithItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.OutputSelections)
               .WithCommandFor<RefreshSimulationOutputSelectionsUICommand, IMoBiSimulation>(viewItem.Simulation, _container))
            .WithItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.SettingsAndSchema)
               .WithCommandFor<RefreshSimulationSolverAndSchemaUICommand, IMoBiSimulation>(viewItem.Simulation, _container));
         ;
      }

      private IMenuBarItem createCommitSubMenu(SimulationSettingsDTO viewItem)
      {
         return CreateSubMenu.WithCaption(AppConstants.MenuNames.CommitToProjectDefaults).WithIcon(ApplicationIcons.Commit)
            .WithItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.OutputSelections)
               .WithCommandFor<CommitSimulationOutputSelectionsUICommand, SimulationSettings>(viewItem.Simulation.Settings, _container))
            .WithItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.SettingsAndSchema)
               .WithCommandFor<CommitSimulationSolverAndSchemaUICommand, SimulationSettings>(viewItem.Simulation.Settings, _container));
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
         return contextMenu.InitializeWith(viewItem.DowncastTo<SimulationSettingsDTO>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter) => viewItem is SimulationSettingsDTO;
   }
}