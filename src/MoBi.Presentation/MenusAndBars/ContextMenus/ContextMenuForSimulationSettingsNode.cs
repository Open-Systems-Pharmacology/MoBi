using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForSimulationSettingsNodeFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuForSimulationSettingsNodeFactory(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForSimulationSettingsNode(presenter.DowncastTo<IHierarchicalSimulationPresenter>(), _container);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<SimulationSettingsViewItem>() &&
                presenter.IsAnImplementationOf<IHierarchicalSimulationPresenter>();
      }
   }

   public class ContextMenuForSimulationSettingsNode : ContextMenuBase
   {
      private readonly IContainer _container;
      private readonly IHierarchicalSimulationPresenter _presenter;

      public ContextMenuForSimulationSettingsNode(IHierarchicalSimulationPresenter hierarchicalSimulationPresenter, IContainer container)
      {
         _container = container;
         _presenter = hierarchicalSimulationPresenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.RefreshFromProjectDefaults)
            .WithCommandFor<RefreshSimulationSolverAndSchemaUICommand, IMoBiSimulation>(_presenter.Simulation, _container)
            .WithIcon(ApplicationIcons.Refresh);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.CommitToProjectDefaults)
            .WithCommandFor<CommitSimulationSolverAndSchemaUICommand, SimulationSettings>(_presenter.Simulation.Settings, _container)
            .WithIcon(ApplicationIcons.Commit);
      }
   }
}