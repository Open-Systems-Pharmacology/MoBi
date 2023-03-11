using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForNeighborhoodBuilder : ContextMenuBase, IContextMenuFor<NeighborhoodBuilder>
   {
      private readonly IList<IMenuBarItem> _allMenuItems;
      private readonly IMoBiContext _context;
      private readonly IContainer _container;

      public ContextMenuForNeighborhoodBuilder(IMoBiContext context, IContainer container)
      {
         _context = context;
         _container = container;
         _allMenuItems = new List<IMenuBarItem>();
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var neighborhoodBuilder = _context.Get<NeighborhoodBuilder>(dto.Id);
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditCommandFor<NeighborhoodBuilder>, NeighborhoodBuilder>(neighborhoodBuilder, _container));
         
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithIcon(ApplicationIcons.Rename)
            .WithCommandFor<RenameObjectCommand<NeighborhoodBuilder>, NeighborhoodBuilder>(neighborhoodBuilder, _container));
         
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithRemoveCommand(neighborhoodBuilder.ParentContainer,neighborhoodBuilder));
         return this;
      }
   }
}