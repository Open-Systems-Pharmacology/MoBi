using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using NPOI.POIFS.Properties;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;


namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForExpressionProfileFolder : ContextMenuBase
   {
      private readonly ITreeNode<RootNodeType> _treeNode;
      private readonly IExplorerPresenter _presenter;
      private readonly IMenuBarItemRepository _menuBarItemRepository;


      public ContextMenuForExpressionProfileFolder(IMenuBarItemRepository menuBarItemRepository,  ITreeNode<RootNodeType> treeNode,
         IExplorerPresenter presenter)
      {
         _treeNode = treeNode;
         _presenter = presenter;
         _menuBarItemRepository = menuBarItemRepository;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return _menuBarItemRepository[MenuBarItemIds.LoadExpressionProfile];

      }
   }

   internal class ContextMenuSpecificationFactoryForExpressionProfileFolder : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForExpressionProfileFolder(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var explorerPresenter = presenter.DowncastTo<IExplorerPresenter>();
         return new ContextMenuForExpressionProfileFolder(_container.Resolve<IMenuBarItemRepository>(),
            explorerPresenter.NodeByType(MoBiRootNodeTypes.ExpressionProfilesFolder), explorerPresenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem == MoBiRootNodeTypes.ExpressionProfilesFolder && presenter.IsAnImplementationOf<IExplorerPresenter>();
      }
   }
}