using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal interface IContextMenuForEventBuilder : IContextMenu
   {
      IContextMenu InitializeWith(ObjectBaseDTO dto, IEventGroupBuilder parent);
   }

   internal class ContextMenuForEventBuilder : ContextMenuBase, IContextMenuForEventBuilder
   {
      private IList<IMenuBarItem> _allMenuItems;
      private readonly IMoBiContext _context;

      public ContextMenuForEventBuilder(IMoBiContext context)
      {
         _context = context;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IEventGroupBuilder parent)
      {
         var eventBuilder = _context.Get<IEventBuilder>(dto.Id);
         _allMenuItems = new List<IMenuBarItem>
         {
            CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
               .WithIcon(ApplicationIcons.Edit)
               .WithCommandFor<EditCommandFor<IEventBuilder>, IEventBuilder>(eventBuilder),
            CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
               .WithIcon(ApplicationIcons.Rename)
               .WithCommandFor<RenameObjectCommand<IEventBuilder>, IEventBuilder>(eventBuilder)
         };


         var removeItem = CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Remove)
            .WithCommandFor<IRemoveRootEventBuilderFromEventGroupBuilderUICommand, IEventBuilder>(eventBuilder);
         _allMenuItems.Add(removeItem);

         return this;
      }
   }
}