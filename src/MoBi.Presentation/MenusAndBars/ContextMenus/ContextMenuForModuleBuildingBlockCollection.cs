using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal abstract class ContextMenuForModuleBuildingBlockCollection : ContextMenuBase, IContextMenuFor<ModuleViewItem>
   {
      protected List<IMenuBarItem> _allMenuItems;

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }
      
      public virtual IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         _allMenuItems.Add(AddNewBuildingBlock(ModuleFor(dto)));
         return this;
      }

      protected static Module ModuleFor(ObjectBaseDTO dto)
      {
         var collectionViewItem = dto.DowncastTo<ModuleViewItem>();
         var module = collectionViewItem.Module;
         return module;
      }

      protected abstract IMenuBarItem AddNewBuildingBlock(Module module);
   }
}