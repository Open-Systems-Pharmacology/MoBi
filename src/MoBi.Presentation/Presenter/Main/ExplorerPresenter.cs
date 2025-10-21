using System;
using System.Collections.Generic;
using System.Drawing;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Nodes;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter.Main
{
   public interface IExplorerPresenter : IMainViewItemPresenter,
      IPresenterWithContextMenu<IViewItem>,
      OSPSuite.Presentation.Presenters.IExplorerPresenter,
      IListener<SimulationRunFinishedEvent>

   {
      int OrderingComparisonForModules(ITreeNode<IWithName> node1, ITreeNode<IWithName> node2);
   }

   public abstract class ExplorerPresenter<TView, TPresenter> : AbstractExplorerPresenter<TView, TPresenter>, IExplorerPresenter
      where TView : IView<TPresenter>, IExplorerView
      where TPresenter : IExplorerPresenter
   {
      protected readonly ITreeNodeFactory _treeNodeFactory;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected readonly IMoBiContext _context;
      private readonly IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;

      private static readonly IReadOnlyDictionary<Type, int> _buildingBlockOrder = new Dictionary<Type, int>
      {
         { typeof(MoBiSpatialStructure), 0 }, // Organism
         { typeof(MoleculeBuildingBlock), 1 }, // Molecules
         { typeof(MoBiReactionBuildingBlock), 2 }, // Reactions
         { typeof(PassiveTransportBuildingBlock), 3 }, // Passive Transports
         { typeof(ObserverBuildingBlock), 4 }, // Observers
         { typeof(EventGroupBuildingBlock), 5 }, // Events
         { typeof(InitialConditionsBuildingBlock), 6 }, // Initial Conditions
         { typeof(ParameterValuesBuildingBlock), 7 }, // Parameter Values
      };

      protected ExplorerPresenter(TView view, IRegionResolver regionResolver, ITreeNodeFactory treeNodeFactory, IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context, RegionName regionName, IClassificationPresenter classificationPresenter, IToolTipPartCreator toolTipPartCreator, IMultipleTreeNodeContextMenuFactory multipleTreeNodeContextMenuFactory, IProjectRetriever projectRetriever)
         : base(view, regionResolver, classificationPresenter, toolTipPartCreator, regionName, projectRetriever)
      {
         _treeNodeFactory = treeNodeFactory;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _context = context;
         _multipleTreeNodeContextMenuFactory = multipleTreeNodeContextMenuFactory;
      }

      protected abstract void AddProjectToTree(MoBiProject project);

      protected void ReloadProject()
      {
         AddProjectToTree(_context.CurrentProject);
      }

      protected override void AddProjectToTree(IProject project)
      {
         AddProjectToTree(project.DowncastTo<MoBiProject>());
      }

      public void ShowContextMenu(IViewItem viewItem, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(viewItem, this);
         showContextMenu(contextMenu, popupLocation);
      }

      public override void ShowContextMenu(ITreeNode treeNode, Point popupLocation)
      {
         var contextMenu = ContextMenuFor(treeNode);
         showContextMenu(contextMenu, popupLocation);
      }

      public override void NodeDoubleClicked(ITreeNode node)
      {
         if (IsExpandable(node))
         {
            base.NodeDoubleClicked(node);
            return;
         }

         var contextMenu = ContextMenuFor(node);
         contextMenu.ActivateFirstMenu();
      }

      protected virtual IContextMenu ContextMenuFor(ITreeNode treeNode)
      {
         var viewItem = treeNode as IViewItem ?? treeNode.TagAsObject as IViewItem;
         return ContextMenuFor(viewItem);
      }

      protected virtual IContextMenu ContextMenuFor(IViewItem viewItem)
      {
         return _viewItemContextMenuFactory.CreateFor(viewItem, this);
      }

      private void showContextMenu(IContextMenu contextMenu, Point popupLocation)
      {
         contextMenu.Show(_view, popupLocation);
      }

      public override void ShowContextMenu(IReadOnlyList<ITreeNode> treeNodes, Point popupLocation)
      {
         var contextMenu = _multipleTreeNodeContextMenuFactory.CreateFor(treeNodes, this);
         showContextMenu(contextMenu, popupLocation);
      }

      public virtual void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         _view.Enabled = true;
      }

      public virtual int OrderingComparisonForModules(ITreeNode<IWithName> node1, ITreeNode<IWithName> node2)
      {
         if (nodeIsStartValueFolderNode(node1) && nodeIsStartValueFolderNode(node2))
            return nodeIsInitialConditionsNode(node1) ? -1 : 1;

         if (nodeIsStartValueFolderNode(node1))
            return 1;

         if (nodeIsStartValueFolderNode(node2))
            return -1;

         if (nodeTagIsModuleRootNode(node1) && nodeTagIsModuleRootNode(node2))
            return rootNodeTypeComparison(node1);

         if (nodeTagIsModuleRootNode(node1) && !nodeTagIsModuleRootNode(node2))
            return -1;

         if (!nodeTagIsModuleRootNode(node1) && nodeTagIsModuleRootNode(node2))
            return 1;

         if (nodeTagIsBuildingBlock(node1) && nodeTagIsBuildingBlock(node2))
         {
            var o1 = orderFor(node1.Tag as BuildingBlock);
            var o2 = orderFor(node2.Tag as BuildingBlock);

            var byRank = o1.CompareTo(o2);
            if (byRank != 0) return byRank;
         }

         return nameComparison(node1, node2);
      }

      private static bool nodeIsStartValueFolderNode(ITreeNode<IWithName> n) =>
         nodeIsParameterValuesNode(n) || nodeIsInitialConditionsNode(n);

      private static bool nodeIsInitialConditionsNode(ITreeNode<IWithName> n) =>
         n is InitialConditionsFolderNode;

      private static bool nodeIsParameterValuesNode(ITreeNode<IWithName> n) =>
         n is ParameterValuesFolderNode;

      private static bool nodeTagIsModuleRootNode(ITreeNode<IWithName> n) =>
         Equals(n?.Tag, RootNodeTypes.ModulesFolder);

      private static bool nodeTagIsBuildingBlock(ITreeNode<IWithName> n) =>
         n?.Tag is BuildingBlock;

      private static int rootNodeTypeComparison(ITreeNode<IWithName> node1)
      {
         if (node1.Tag.Equals(RootNodeTypes.ModulesFolder))
            return 1;
         return -1;
      }

      private static int nameComparison(ITreeNode<IWithName> a, ITreeNode<IWithName> b)
      {
         if (a != null && b != null)
            return string.Compare(a.Tag.Name, b.Tag.Name, StringComparison.InvariantCultureIgnoreCase);
         return 0;
      }

      private static int orderFor(BuildingBlock buildingBlock)
      {
         if (buildingBlock == null)
            return int.MaxValue;

         if (_buildingBlockOrder.TryGetValue(buildingBlock.GetType(), out var order))
            return order;

         return int.MaxValue;
      }
   }
}