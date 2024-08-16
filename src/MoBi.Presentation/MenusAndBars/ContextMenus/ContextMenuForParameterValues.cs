using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForParameterValues : ContextMenuBase
   {
      private readonly IMoBiContext _context;

      public ContextMenuForParameterValues(IMoBiContext context)
      {
         _context = context;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton
            .WithCaption(AppConstants.MenuNames.NewParameterValue)
            .WithId(MenuBarItemIds.NewParameterValue)
            .WithIcon(ApplicationIcons.AddParameterValues)
            .WithCommand<AddParameterValuesUICommand>(_context.Container);
      }
   }
}