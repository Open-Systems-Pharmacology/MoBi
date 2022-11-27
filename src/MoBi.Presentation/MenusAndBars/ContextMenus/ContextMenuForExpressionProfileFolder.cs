using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views;
using NPOI.POIFS.Properties;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

/*
namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForExpressionProfileFolder : ContextMenuBase
   {
      private readonly ITreeNode<RootNodeType> _treeNode;
      private readonly IExplorerPresenter _presenter;

      public ContextMenuForExpressionProfileFolder( ITreeNode<RootNodeType> treeNode,
         IExplorerPresenter presenter)
      {
         _treeNode = treeNode;
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         var importPkmlMenu = createImportPkmlSubMenu(_treeNode, _presenter);
            
         yield return importPkmlMenu;
      }

      private static IMenuBarButton createImportPkmlSubMenu(ITreeNode<RootNodeType> treeNode, IExplorerPresenter presenter)
      {
         var groupMenu = CreateMenuButton.WithCaption(AppConstants.Captions.LoadExpressionProfile)
            .WithIcon(ApplicationIcons.PKMLLoad)
         .WithCommandFor<AddExistingCommandFor<IBuildingBlock, IObjectBase>, IBuildingBlock>(treeNode); ;
         return groupMenu;
      }
   }

   internal class ContextMenuSpecificationFactoryForExpressionProfileFolder : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;
      private readonly IUserSettings _userSettings;

      public ContextMenuSpecificationFactoryForExpressionProfileFolder(IContainer container, IUserSettings userSettings)
      {
         _container = container;
         _userSettings = userSettings;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var explorerPresenter = presenter.DowncastTo<IExplorerPresenter>();
         return new ContextMenuForExpressionProfileFolder(
            explorerPresenter.NodeByType(MoBiRootNodeTypes.ExpressionProfilesFolder), explorerPresenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem == MoBiRootNodeTypes.ExpressionProfilesFolder && presenter.IsAnImplementationOf<IExplorerPresenter>();
      }
   }
}
*/