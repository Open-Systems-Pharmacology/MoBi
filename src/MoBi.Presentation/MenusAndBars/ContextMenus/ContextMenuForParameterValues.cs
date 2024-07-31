using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using System.Collections.Generic;
using DevExpress.DirectX.Common.Direct2D;
using MoBi.Assets;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Data;
using OSPSuite.SimModel;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForParameterValues : ContextMenuBase
   {
      private readonly IMoBiContext _context;
      public ContextMenuForParameterValues(IMoBiContext context, IParameterValuesPresenter presenter)
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