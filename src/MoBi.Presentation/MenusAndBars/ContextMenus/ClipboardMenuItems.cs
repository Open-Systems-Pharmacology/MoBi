using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public static class ClipboardMenuItems
   {
      /// <summary>
      ///    Returns the copy and paste menu items available for the path and value entities edited by
      ///    <paramref name="presenter" />
      /// </summary>
      public static IEnumerable<IMenuBarItem> For<TPathAndValueEntityDTO>(IExtendablePathAndValueBuildingBlockPresenter<TPathAndValueEntityDTO> presenter) where TPathAndValueEntityDTO : IPathAndValueEntityDTO
      {
         if (presenter.HasSelection)
         {
            yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Copy)
               .WithActionCommand(presenter.CopySelectionToClipBoard)
               .WithIcon(ApplicationIcons.Copy)
               .AsGroupStarter();
         }

         if (presenter.CanPasteFromClipBoard())
         {
            var pasteMenuButton = CreateMenuButton.WithCaption(AppConstants.MenuNames.Paste)
               .WithActionCommand(presenter.PasteFromClipBoard)
               .WithIcon(ApplicationIcons.Paste);

            yield return presenter.HasSelection ? pasteMenuButton : pasteMenuButton.AsGroupStarter();
         }
      }
   }
}
