using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleObservedDataFolderContextMenuFactory : IContextMenuSpecificationFactory<IReadOnlyList<ITreeNode>>,
      IContextMenuFactory<IReadOnlyList<ITreeNode>>
   {
      public bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return treeNodes.All(x => x is ClassificationNode classificationNode && classificationNode.Tag.ClassificationType == ClassificationType.ObservedData);
      }

      public IContextMenu CreateFor(IReadOnlyList<ITreeNode> objectRequestingContextMenu, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         var explorerPresenter = presenter.DowncastTo<IExplorerPresenter>();
         return new MultipleObservedDataFolderContextMenu(objectRequestingContextMenu.Select(x => x as ITreeNode<IClassification>).ToList(), explorerPresenter);
      }
   }

   public class MultipleObservedDataFolderContextMenu : ContextMenuBase
   {
      private readonly IExplorerPresenter _explorerPresenter;
      private readonly IReadOnlyList<ITreeNode<IClassification>> _treeNodes;

      public MultipleObservedDataFolderContextMenu(IReadOnlyList<ITreeNode<IClassification>> treeNodes, IExplorerPresenter explorerPresenter)
      {
         _explorerPresenter = explorerPresenter;
         _treeNodes = treeNodes;
      }

      private IMenuBarSubMenu createGroupingSubMenu(IReadOnlyList<ITreeNode<IClassification>> treeNodes)
      {
         var groupMenu = CreateSubMenu.WithCaption(MenuNames.GroupBy);

         var classificationsSupportedByEachNode = treeNodes.Select(treeNode => (treeNode, templates: _explorerPresenter.AvailableClassificationCategories(treeNode))).ToList();

         intersectionByClassificationName(classificationsSupportedByEachNode).Each(classification => groupMenu.AddItem(classifyButtonFor(treeNodes, classification)));

         return groupMenu;
      }

      private IMenuBarButton classifyButtonFor(IReadOnlyList<ITreeNode<IClassification>> treeNodes, ClassificationTemplate classification)
      {
         return CreateMenuButton.WithCaption(classification.ClassificationName)
            .WithIcon(classification.Icon)
            .WithActionCommand(() => classifyTreeNodes(treeNodes, classification));
      }

      private IReadOnlyList<ClassificationTemplate> intersectionByClassificationName(List<(ITreeNode<IClassification> treeNode, IEnumerable<ClassificationTemplate> templates)> classifications)
      {
         return (classifications.Any() ? classifications.First().templates.Where(template => templateIsCommonToAll(classifications, template)) : Enumerable.Empty<ClassificationTemplate>()).ToList();
      }

      private bool templateIsCommonToAll(List<(ITreeNode<IClassification> treeNode, IEnumerable<ClassificationTemplate> templates)> classifications, ClassificationTemplate classification)
      {
         return classifications.All(x => x.templates.Any(c => c.ClassificationName.Equals(classification.ClassificationName)));
      }

      private void classifyTreeNodes(IReadOnlyList<ITreeNode<IClassification>> treeNodes, ClassificationTemplate classification)
      {
         treeNodes.Each(treeNode => { _explorerPresenter.AddToClassificationTree(treeNode, classification.ClassificationName); });
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         var groupMenu = createGroupingSubMenu(_treeNodes);
         if (groupMenu.AllItems().Any())
            yield return groupMenu;
      }
   }
}