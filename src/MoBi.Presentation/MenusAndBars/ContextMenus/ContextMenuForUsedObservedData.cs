using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForUsedObservedData : ContextMenuBase
   {
      private readonly IContainer _container;
      private IList<IMenuBarItem> _allMenuItems;

      public ContextMenuForUsedObservedData(IContainer container)
      {
         _container = container;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems() => _allMenuItems;

      public IContextMenu InitializeWith(UsedObservedData usedObservedData)
      {
         _allMenuItems = new List<IMenuBarItem>
         {
            CreateMenuButton.WithCaption(MenuNames.Remove)
               .WithCommand(_container.Resolve<RemoveUsedObservedDataUICommand>().For(usedObservedData))
               .WithIcon(ApplicationIcons.Remove)
         };

         return this;
      }
   }

   internal class ContextMenuFactoryForUsedObservedData : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuFactoryForUsedObservedData(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var usedObservedData = viewItem.DowncastTo<UsedObservedDataViewItem>().UsedObservedData;
         return new ContextMenuForUsedObservedData(_container).InitializeWith(usedObservedData);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<UsedObservedDataViewItem>();
      }
   }
}
