using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MdiChildViewContextMenu : ContextMenu<IMdiChildView>
   {
      public MdiChildViewContextMenu(IMdiChildView view, IContainer container) : base(view, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IMdiChildView view)
      {
         yield return CreateMenuButton.WithCaption(AppConstants.Captions.CloseView)
            .WithIcon(ApplicationIcons.Close)
            .WithCommandFor<CloseMdiViewCommand, IMdiChildView>(view, _container);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.CloseAll)
            .WithCommand<CloseAllMdiViewCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.CloseAllButThis)
            .WithCommandFor<CloseAllButMdiViewCommand, IMdiChildView>(view, _container);
      }
   }

   public class MdiChildViewContextMenuFactory : IContextMenuSpecificationFactory<IMdiChildView>
   {
      private readonly IContainer _container;

      public MdiChildViewContextMenuFactory(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IMdiChildView view, IPresenterWithContextMenu<IMdiChildView> presenter)
      {
         return new MdiChildViewContextMenu(view, _container);
      }

      public bool IsSatisfiedBy(IMdiChildView view, IPresenterWithContextMenu<IMdiChildView> presenter)
      {
         return view != null;
      }
   }
}