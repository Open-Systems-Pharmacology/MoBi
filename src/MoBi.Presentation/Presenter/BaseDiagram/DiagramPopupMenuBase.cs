using System.Drawing;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.UICommand.DiagramUICommands;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter.BaseDiagram
{
   public interface IDiagramPopupMenuBase
   {
      void Show(IView parentView, Point popupLocationInParentView, PointF locationInDiagramView, IContainerBase containerBase, IBaseNode node);
   }

   public class DiagramPopupMenuBase : IDiagramPopupMenuBase
   {
      private readonly IStartOptions _runOptions;
      protected IMoBiBaseDiagramPresenter Presenter { get; private set; }
      protected IMenuBarSubMenu SubMenuSelect { get; private set; }
      protected IMenuBarSubMenu SubMenuLayout { get; private set; }
      protected IMenuBarSubMenu SubMenuDiagram { get; private set; }

      public DiagramPopupMenuBase(IMoBiBaseDiagramPresenter presenter, IStartOptions runOptions)
      {
         _runOptions = runOptions;
         Presenter = presenter;
      }

      public void Show(IView parentView, Point popupLocationInParentView, PointF locationInDiagramView, IContainerBase containerBase, IBaseNode node)
      {
         var contextMenuView = IoC.Resolve<IContextMenuView>();
         Presenter.CurrentInsertLocation = locationInDiagramView;

         SubMenuSelect = CreateSubMenu.WithCaption("Select").AsGroupStarter();
         SubMenuLayout = CreateSubMenu.WithCaption("Layout").AsGroupStarter();
         SubMenuDiagram = CreateSubMenu.WithCaption("Diagram").AsGroupStarter();

         SetModelMenuItems(contextMenuView, containerBase, node);
         SetSelectionMenuItems(contextMenuView, containerBase, node);
         if (containerBase != null) SetLayoutMenuItems(contextMenuView, containerBase);
         SetDiagramMenuItems(contextMenuView, containerBase);
         SetDetailedMenuItems(contextMenuView, containerBase);

         contextMenuView.Display(parentView, popupLocationInParentView);
      }

      protected virtual void SetModelMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase, IBaseNode node)
      {
      }

      protected virtual void SetSelectionMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase, IBaseNode node)
      {
         contextMenuView.AddMenuItem(SubMenuSelect);

         if (containerBase != null)
            SubMenuSelect.AddItem(CreateMenuButton.WithCaption("All Children")
               .WithActionCommand(() => Presenter.SelectChildren(containerBase)));

         if (Presenter.SelectionContains<IBaseNode>())
         {
            SubMenuSelect.AddItem(CreateMenuButton.WithCaption("Visible Linked nodes")
               .WithActionCommand(() => Presenter.SelectVisibleLinkedNodesForDiagramSelection()));
            if (node != null)
               SubMenuSelect.AddItem(CreateMenuButton.WithCaption("Invert Selection")
                  .WithActionCommand(() => Presenter.InvertSelection(node.GetParent())));

            IHasLayoutInfo selectedNode = Presenter.GetFirstSelected<IHasLayoutInfo>();
            if (selectedNode != null)
               SubMenuDiagram.AddItem(CreateMenuCheckButton.WithCaption("Location Fixed").AsGroupStarter()
                  .WithChecked(selectedNode.LocationFixed)
                  .WithCheckedAction(locationFixed => Presenter.SetLocationFixedForDiagramSelection(locationFixed)));

            contextMenuView.AddMenuItem(CreateMenuButton.WithCaption("Hide Selection").WithActionCommand(() => Presenter.HideSelection()));
         }

         if (Presenter.SelectionContains<IElementBaseNode>())
         {
            var subMenuNodeSize = CreateSubMenu.WithCaption("Nodesize")
               .WithItem(CreateMenuButton.WithCaption("Large").WithActionCommand(() => Presenter.SetNodeSizeForDiagramSelection(NodeSize.Large)))
               .WithItem(CreateMenuButton.WithCaption("Middle").WithActionCommand(() => Presenter.SetNodeSizeForDiagramSelection(NodeSize.Middle)))
               .WithItem(CreateMenuButton.WithCaption("Small").WithActionCommand(() => Presenter.SetNodeSizeForDiagramSelection(NodeSize.Small)));
            contextMenuView.AddMenuItem(subMenuNodeSize);
         }

         // Shows all children (e.g. after hiding a selection)

         if (containerBase != null) contextMenuView.AddMenuItem(CreateMenuButton.WithCaption("Show All Children").WithActionCommand(() => Presenter.ShowChildren(containerBase)));
      }

      protected virtual void SetLayoutMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         AddAutoLayoutMenuItems(contextMenuView, containerBase);
         contextMenuView.AddMenuItem(SubMenuLayout);
      }

      protected virtual void SetDiagramMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption("Refresh").WithActionCommand(() => Presenter.Refresh()).WithIcon(ApplicationIcons.Refresh).AsGroupStarter());

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption("Undo Diagram Layout Change").WithActionCommand(() => Presenter.Undo()).WithIcon(ApplicationIcons.Undo));

         if (Presenter.SelectionContains<IBaseNode>())
         {
            SubMenuDiagram.AddItem(CreateMenuButton.WithCaption("To Front").AsGroupStarter().WithActionCommand(() => Presenter.MoveDiagramSelectionToFront()).WithIcon(ApplicationIcons.Forward));
            SubMenuDiagram.AddItem(CreateMenuButton.WithCaption("To Back").WithActionCommand(() => Presenter.MoveDiagramSelectionToBack()).WithIcon(ApplicationIcons.Back));
         }


         SubMenuDiagram.AddItem(CreateMenuButton.WithCaption("Zoom In").AsGroupStarter().WithCommand<ZoomInCommand>().WithIcon(ApplicationIcons.ZoomIn));
         SubMenuDiagram.AddItem(CreateMenuButton.WithCaption("Zoom Out").WithCommand<ZoomOutCommand>().WithIcon(ApplicationIcons.ZoomOut));
         SubMenuDiagram.AddItem(CreateMenuButton.WithCaption("Fit into Window").WithCommand<FitToPageCommand>().WithIcon(ApplicationIcons.FitToPage));
         SubMenuDiagram.AddItem(CreateMenuButton.WithCaption("Copy Image to Clipboard").AsGroupStarter().WithActionCommand(() => Presenter.CopyBitmapToClipboard(containerBase)));
         SubMenuDiagram.AddItem(CreateMenuButton.WithCaption("Save Image...").WithActionCommand(() => Presenter.SaveBitmapToFile(containerBase)));

         contextMenuView.AddMenuItem(SubMenuDiagram);
      }

      protected void AddAutoLayoutMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         if (containerBase == null) return;

         SubMenuLayout.AddItem(CreateMenuButton.WithCaption("AutoLayout Children").AsGroupStarter().WithActionCommand(() => Presenter.Layout(containerBase, AppConstants.Diagram.Base.LayoutDepthChildren, null)));
      }

      protected void AddTemplateLayoutMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         string containerType = "Diagram";
         if (containerBase.IsAnImplementationOf<IContainerNode>()) containerType = "Container";

         IHasLayoutInfo selectedNode = Presenter.GetFirstSelected<IContainerNode>();
         if (selectedNode != null)
            SubMenuLayout.AddItem(CreateMenuButton.WithCaption("Apply Named Template to Selection...")
               .WithActionCommand(() => Presenter.ApplyLayoutTemplateToSelection()).AsGroupStarter());
         else
            SubMenuLayout.AddItem(CreateMenuButton.WithCaption("Apply Named Template to " + containerType + "...")
               .WithActionCommand(() => Presenter.ApplyLayoutTemplate(containerBase, "", false)));

         SubMenuLayout.AddItem(CreateMenuButton.WithCaption("Apply Named Template to " + containerType + " Recursive...")
            .WithActionCommand(() => Presenter.ApplyLayoutTemplate(containerBase, "", true)));

         SubMenuLayout.AddItem(CreateMenuButton.WithCaption("Save " + containerType + " as Named Template...")
            .WithActionCommand(() => Presenter.SaveContainerToXml(containerBase, "")));
      }

      protected void SetDetailedMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         var subMenuDetailed = CreateSubMenu.WithCaption("Expert Functions").AsGroupStarter();
         contextMenuView.AddMenuItem(subMenuDetailed);


         var subMenuShow = CreateSubMenu.WithCaption("Show / Hide");
         if (Presenter.SelectionContains<IBaseNode>()) subMenuDetailed.AddItem(subMenuShow);

         subMenuShow.AddItem(
            CreateMenuButton.WithCaption("Hide not Linked Nodes").WithActionCommand(() => Presenter.HideNotLinkedNodes()));
         subMenuShow.AddItem(
            CreateMenuButton.WithCaption("Show Linked Nodes").WithActionCommand(() => Presenter.ShowLinkedNodes()));

         var subMenuExpand = CreateSubMenu.WithCaption("Expand / Collapse");
         subMenuDetailed.AddItem(subMenuExpand);

         subMenuExpand.AddItem(
            CreateMenuButton.WithCaption("Set Default Expansion").WithActionCommand(() => Presenter.SetDefaultExpansion()));
         subMenuExpand.AddItem(
            CreateMenuButton.WithCaption("Show Default Expansion")
               .WithActionCommand(() => Presenter.ShowDefaultExpansion()));

         if (Presenter.SelectionContains<IContainerNode>()) AddExpandMenuItems(subMenuExpand);


         var subMenuDiagram = CreateSubMenu.WithCaption("Diagram");
         subMenuDetailed.AddItem(subMenuDiagram);

         subMenuDiagram.AddItem(
            CreateMenuButton.WithCaption("Copy clipping as Bitmap")
               .WithActionCommand(() => Presenter.CopyBitmapToClipboard(null)));
         subMenuDiagram.AddItem(
            CreateMenuButton.WithCaption("Print Preview...").WithActionCommand(() => Presenter.PrintDiagram()));

         var subMenuLayout = CreateSubMenu.WithCaption("Layout");
         if (containerBase != null) subMenuDetailed.AddItem(subMenuLayout);
         subMenuLayout.AddItem(
            CreateMenuButton.WithCaption("AutoLayout Grandchildren")
               .WithActionCommand(
                  () =>
                     Presenter.Layout(containerBase, AppConstants.Diagram.Base.LayoutDepthGrandChildren,
                        null)));
         subMenuLayout.AddItem(
            CreateMenuButton.WithCaption("AutoLayout all Descendants")
               .WithActionCommand(
                  () => Presenter.Layout(containerBase, AppConstants.Diagram.Base.LayoutDepthAll, null)));
         if (_runOptions.IsDeveloperMode)
         {
            var subMenuDeveloper = CreateSubMenu.WithCaption("Developer Functions").AsGroupStarter()
               .WithItem(
                  CreateMenuButton.WithCaption("Position at 0")
                     .WithActionCommand(() => Presenter.Position0Selection()));
            subMenuDetailed.AddItem(subMenuDeveloper);
         }
         
         
      }

      protected void AddExpandMenuItems(IMenuBarSubMenu subMenuExpand)
      {
         subMenuExpand.AddItem(CreateMenuButton.WithCaption("Expand Selection").WithActionCommand(() => Presenter.ExpandSelection()));

         subMenuExpand.AddItem(CreateMenuButton.WithCaption("Collapse Selection").WithActionCommand(() => Presenter.CollapseSelection()));
         subMenuExpand.AddItem(CreateMenuButton.WithCaption("Collapse all Except Selection").WithActionCommand(() => Presenter.CollapseAllExceptSelection()).AsGroupStarter());
      }


   }
}