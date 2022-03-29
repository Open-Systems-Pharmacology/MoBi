using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForObservedDataFolder : ContextMenuBase
   {
      private readonly IMenuBarItemRepository _menuBarItemRepository;
      private readonly ITreeNode<RootNodeType> _treeNode;
      private readonly IExplorerPresenter _presenter;
      private readonly IUserSettings _userSettings;

      public ContextMenuForObservedDataFolder(IMenuBarItemRepository menuBarItemRepository, ITreeNode<RootNodeType> treeNode, IExplorerPresenter presenter, IUserSettings userSettings)
      {
         _menuBarItemRepository = menuBarItemRepository;
         _treeNode = treeNode;
         _presenter = presenter;
         _userSettings = userSettings;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return _menuBarItemRepository[MenuBarItemIds.AddObservedData];
         yield return _menuBarItemRepository[MenuBarItemIds.LoadObservedData];

         if (_treeNode.AllLeafNodes.OfType<ObservedDataNode>().Any())
            yield return ObservedDataClassificationCommonContextMenuItems.CreateEditMultipleMetaDataMenuButton(_treeNode).AsGroupStarter();


         yield return ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(_treeNode, _presenter);

         var groupMenu = createGroupingSubMenu(_treeNode, _presenter);
         if (groupMenu.AllItems().Any())
            yield return groupMenu;

         if (_treeNode.HasChildren)
         {
            var colorGroupingButton = CreateMenuCheckButton.WithCaption(Captions.ColorGroupObservedDataContextMenu);
            colorGroupingButton.Checked = _userSettings.ColorGroupObservedDataFromSameFolder;

            colorGroupingButton.CheckedChanged += (o) => { _userSettings.ColorGroupObservedDataFromSameFolder = o; };
            yield return colorGroupingButton;
         }

         yield return ClassificationCommonContextMenuItems.RemoveClassificationFolderMainMenu(_treeNode, _presenter).AsGroupStarter();
      }

      private static IMenuBarSubMenu createGroupingSubMenu(ITreeNode<RootNodeType> treeNode, IExplorerPresenter presenter)
      {
         var groupMenu = CreateSubMenu.WithCaption(MenuNames.GroupBy);

         presenter.AvailableClassificationCategories(treeNode)
            .Each(classification => groupMenu.AddItem(
               CreateMenuButton.WithCaption(classification.ClassificationName)
                  .WithActionCommand(() => presenter.AddToClassificationTree(treeNode, classification.ClassificationName))));

         return groupMenu;
      }
   }

   internal class ContextMenuSpecificationFactoryForObservedDataFolder : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;
      private readonly IUserSettings _userSettings;

      public ContextMenuSpecificationFactoryForObservedDataFolder(IContainer container, IUserSettings userSettings)
      {
         _container = container;
         _userSettings = userSettings;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var explorerPresenter = presenter.DowncastTo<IExplorerPresenter>();
         return new ContextMenuForObservedDataFolder(_container.Resolve<IMenuBarItemRepository>(), explorerPresenter.NodeByType(RootNodeTypes.ObservedDataFolder), explorerPresenter, _userSettings);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem == RootNodeTypes.ObservedDataFolder && presenter.IsAnImplementationOf<IExplorerPresenter>();
      }
   }
}