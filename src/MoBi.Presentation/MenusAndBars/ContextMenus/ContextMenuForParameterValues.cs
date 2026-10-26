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
   public class ContextMenuForParameterValues : ContextMenuBase
   {
      private readonly IMoBiContext _context;
      private readonly IParameterValuesPresenter _presenter;

      public ContextMenuForParameterValues(IMoBiContext context, IParameterValuesPresenter presenter)
      {
         _context = context;
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton
            .WithCaption(AppConstants.MenuNames.NewParameterValue)
            .WithId(MenuBarItemIds.NewParameterValue)
            .WithIcon(ApplicationIcons.AddParameterValues)
            .WithCommand<AddParameterValuesUICommand>(_context.Container);

         foreach (var menuItem in ClipboardMenuItems.For(_presenter))
         {
            yield return menuItem;
         }
      }
   }
}
