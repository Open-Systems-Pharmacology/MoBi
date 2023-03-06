using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using IContainer = OSPSuite.Utility.Container.IContainer;
using OSPSuite.Core.Domain.Data;

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
         var moduleViewItem = dto.DowncastTo<ModuleViewItem>();
         var module = moduleViewItem.Module;

         var item = CreateMenuButton.WithCaption(AppConstants.MenuNames.AddBuildingBlocks)
            .WithCommandFor<AddBuildingBlocksToModuleUICommand, Module>(module, _container)
            .WithIcon(ApplicationIcons.AddIconFor(nameof(Module)));

         //.WithIcon(ApplicationIcons.LoadTemplateIconFor(typeName))

         _allMenuItems.Add(item);

         var item_2 = CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingBuildingBlocks)
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(Module)));

         var item_3 = CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingBuildingBlocksFromTemplate)
            .WithIcon(ApplicationIcons.LoadTemplateIconFor(nameof(Module)));

         _allMenuItems.Add(item_2);
         _allMenuItems.Add(item_3);
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