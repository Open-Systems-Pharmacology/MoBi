using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using System.Runtime.Remoting.Contexts;

namespace MoBi.Presentation.Presenter.Main
{
   public interface IExplorerPresenter : IMainViewItemPresenter,
      IPresenterWithContextMenu<IViewItem>,
      OSPSuite.Presentation.Presenters.IExplorerPresenter,
      IListener<SimulationRunStartedEvent>,
      IListener<SimulationRunFinishedEvent>

   {
   }

   public abstract class ExplorerPresenter<TView, TPresenter> : AbstractExplorerPresenter<TView, TPresenter>, IExplorerPresenter
      where TView : IView<TPresenter>, IExplorerView
      where TPresenter : IExplorerPresenter
   {
      protected readonly ITreeNodeFactory _treeNodeFactory;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected readonly IMoBiContext _context;
      private readonly IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;

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

      public virtual void Handle(SimulationRunStartedEvent eventToHandle)
      {
         _view.Enabled = false;
      }

      public virtual void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         _view.Enabled = true;
      }
   }
}