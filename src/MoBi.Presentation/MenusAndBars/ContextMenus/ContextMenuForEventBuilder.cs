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
using OSPSuite.Utility.Container;

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
      private readonly IContainer _container;

      public ContextMenuForEventBuilder(IMoBiContext context, IContainer container)
      {
         _context = context;
         _container = container;
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
               .WithCommandFor<EditCommandFor<IEventBuilder>, IEventBuilder>(eventBuilder, _container),
            CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
               .WithIcon(ApplicationIcons.Rename)
               .WithCommandFor<RenameObjectCommand<IEventBuilder>, IEventBuilder>(eventBuilder, _container)
         };


         var removeItem = CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Remove)
            .WithCommandFor<IRemoveRootEventBuilderFromEventGroupBuilderUICommand, IEventBuilder>(eventBuilder, _container);
         _allMenuItems.Add(removeItem);

         return this;
      }
   }
}