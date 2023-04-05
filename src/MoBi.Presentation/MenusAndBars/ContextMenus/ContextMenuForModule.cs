using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForModule : ContextMenuBase, IContextMenuFor<Module>
   {
      private readonly IContainer _container;
      private readonly List<IMenuBarItem> _allMenuItems;

      public ContextMenuForModule(IContainer container)
      {
         _container = container;
         _allMenuItems = new List<IMenuBarItem>();
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var module = dto.ObjectBase.DowncastTo<Module>();

         var item = CreateMenuButton.WithCaption(AppConstants.MenuNames.AddBuildingBlocks)
            .WithCommandFor<AddBuildingBlocksToModuleUICommand, Module>(module, _container)
            .WithIcon(ApplicationIcons.AddIconFor(nameof(Module)));

         _allMenuItems.Add(item);
         return this;
      }
   }

   public class ContextMenuSpecificationFactoryForBuildingBlockForModule : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForBuildingBlockForModule(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForModule(_container);
         return contextMenu.InitializeWith(viewItem.DowncastTo<ModuleViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<ModuleViewItem>();
      }
   }
}