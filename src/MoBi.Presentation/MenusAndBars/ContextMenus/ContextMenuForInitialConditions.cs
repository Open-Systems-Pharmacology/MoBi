using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForInitialConditions : ContextMenuBase
   {
      private readonly IMoBiContext _context;
      private readonly IInitialConditionsPresenter _presenter;

      public ContextMenuForInitialConditions(IMoBiContext context, IInitialConditionsPresenter presenter)
      {
         _context = context;
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton
            .WithCaption(AppConstants.MenuNames.NewInitialCondition)
            .WithId(MenuBarItemIds.NewInitialConditions)
            .WithIcon(ApplicationIcons.AddInitialConditions)
            .WithCommand<AddInitialConditionsUICommand>(_context.Container);

         foreach (var menuItem in ClipboardMenuItems.For(_presenter))
         {
            yield return menuItem;
         }
      }
   }
}
