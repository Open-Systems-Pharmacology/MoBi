using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MdiChildViewContextMenu : ContextMenu<IMdiChildView>
   {
      public MdiChildViewContextMenu(IMdiChildView view) : base(view)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IMdiChildView view)
      {
         yield return CreateMenuButton.WithCaption(AppConstants.Captions.CloseView)
            .WithIcon(ApplicationIcons.Close)
            .WithCommandFor<CloseMdiViewCommand, IMdiChildView>(view);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.CloseAll)
            .WithCommand<CloseAllMdiViewCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.CloseAllButThis)
            .WithCommandFor<CloseAllButMdiViewCommand, IMdiChildView>(view);
      }
   }

   public class MdiChildViewContextMenuFactory : IContextMenuSpecificationFactory<IMdiChildView>
   {
      public IContextMenu CreateFor(IMdiChildView view, IPresenterWithContextMenu<IMdiChildView> presenter)
      {
         return new MdiChildViewContextMenu(view);
      }

      public bool IsSatisfiedBy(IMdiChildView view, IPresenterWithContextMenu<IMdiChildView> presenter)
      {
         return view != null;
      }
   }
}