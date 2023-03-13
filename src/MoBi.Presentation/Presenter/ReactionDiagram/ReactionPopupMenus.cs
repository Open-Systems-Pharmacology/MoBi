using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.Presenter.ReactionDiagram
{
   public class PopupMenuReactionBuilder : PopupMenuFullEntityNode<IReactionBuilder>
   {
      public PopupMenuReactionBuilder(IReactionDiagramPresenter presenter, IMoBiContext context, IStartOptions runOptions) : base(presenter, context, runOptions)
      {
      }

      protected new IReactionDiagramPresenter Presenter => (IReactionDiagramPresenter) base.Presenter;

      protected override void SetSelectionMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase, IBaseNode node)
      {
         if (!Presenter.IsReactionNode(node))
            return;

         contextMenuView.AddMenuItem(CreateMenuCheckButton.WithCaption("Connect Educts right")
            .WithChecked(Presenter.DisplayEductsRight(node))
            .WithCheckedAction(displayRight => Presenter.SetDisplayEductsRightForDiagramSelection(displayRight)));

         base.SetSelectionMenuItems(contextMenuView, containerBase, node);
      }
   }

   public class PopupMenuReactionDiagram : DiagramPopupMenuBase
   {
      public PopupMenuReactionDiagram(IReactionDiagramPresenter presenter, IMoBiContext context, IStartOptions runOptions) : base(presenter, context, runOptions)
      {
      }

      protected new IReactionDiagramPresenter Presenter => (IReactionDiagramPresenter) base.Presenter;

      protected override void SetModelMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase, IBaseNode node)
      {
         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.RibbonButtonNames.NewMolecule)
            .WithActionCommand(() => Presenter.AddMoleculeNode())
            .WithIcon(ApplicationIcons.MoleculeAdd));

         var parent = Presenter.Subject as IMoBiReactionBuildingBlock;
         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Reaction))
            .WithCommandFor<AddNewCommandFor<IMoBiReactionBuildingBlock, IReactionBuilder>, IMoBiReactionBuildingBlock>(parent, _context.Container)
            .WithIcon(ApplicationIcons.ReactionAdd));

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Reaction))
            .WithCommandFor<AddExistingCommandFor<IMoBiReactionBuildingBlock, IReactionBuilder>, IMoBiReactionBuildingBlock>(parent, _context.Container)
            .WithIcon(ApplicationIcons.ReactionLoad));

         base.SetModelMenuItems(contextMenuView, containerBase, node);
      }

      protected override void SetLayoutMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         AddTemplateLayoutMenuItems(contextMenuView, containerBase);
         AddAutoLayoutMenuItems(contextMenuView, containerBase);
         contextMenuView.AddMenuItem(SubMenuLayout);
         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption("AutoLayout in layers")
            .WithActionCommand(() => Presenter.LayerLayout(containerBase)));
      }
   }
}