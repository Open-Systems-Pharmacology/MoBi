using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public abstract class ContextMenuBase : IContextMenu
   {
      private readonly IContextMenuView _view;

      protected ContextMenuBase() : this(IoC.Resolve<IContextMenuView>())
      {
      }

      private ContextMenuBase(IContextMenuView view)
      {
         _view = view;
      }

      public void ActivateFirstMenu()
      {
          _view.ActivateMenu(AllMenuItems().FirstOrDefault());
      }

      public void Show(IView parentView, Point popupLocation)
      {
         AllMenuItems().Each(item => _view.AddMenuItem(item));
         _view.Display(parentView, popupLocation);
      }

      public abstract IEnumerable<IMenuBarItem> AllMenuItems();
   }
}