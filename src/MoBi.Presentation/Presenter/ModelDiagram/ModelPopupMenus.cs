using OSPSuite.Core.Services;
using MoBi.Presentation.Presenter.BaseDiagram;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.Presenter.ModelDiagram
{
   public class PopupMenuModelDiagram : DiagramPopupMenuBase
   {
      private readonly IDialogCreator _dialogCreator;

      public PopupMenuModelDiagram(ISimulationDiagramPresenter presenter, IDialogCreator dialogCreator, IStartOptions runOptions, IContainer container) : base(presenter, runOptions, container)
      {
         _dialogCreator = dialogCreator;
      }

      protected new ISimulationDiagramPresenter Presenter
      {
         get { return (ISimulationDiagramPresenter) base.Presenter; }
      }

      private void askAndApplySpaceReactionLayout()
      {
         if (_dialogCreator.MessageBoxYesNo("Layout change cannot be undone. Do you really want to apply layouts?") == ViewResult.Yes)
            Presenter.ApplySpaceReactionLayout();
      }

      protected override void SetLayoutMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         AddTemplateLayoutMenuItems(contextMenuView, containerBase);

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption("Apply current Layouts from Structure and Reactions")
            .WithActionCommand(askAndApplySpaceReactionLayout));

         AddAutoLayoutMenuItems(contextMenuView, containerBase);
         contextMenuView.AddMenuItem(SubMenuLayout);
      }
   }
}