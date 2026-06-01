using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForInitialConditions : ContextMenuBase
   {
      private readonly IMoBiContext _context;

      public ContextMenuForInitialConditions(IMoBiContext context)
      {
         _context = context;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton
            .WithCaption(AppConstants.MenuNames.NewInitialCondition)
            .WithId(MenuBarItemIds.NewInitialConditions)
            .WithIcon(ApplicationIcons.AddInitialConditions)
            .WithCommand<AddInitialConditionsUICommand>(_context.Container);
      }
   }
}