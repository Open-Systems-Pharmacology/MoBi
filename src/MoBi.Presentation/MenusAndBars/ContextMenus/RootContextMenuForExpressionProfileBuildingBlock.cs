using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForExpressionProfileBuildingBlock : RootContextMenuFor<MoBiProject, ExpressionProfileBuildingBlock>
   {
      public RootContextMenuForExpressionProfileBuildingBlock(IObjectTypeResolver objectTypeResolver, IMoBiContext context, IContainer container) : base(objectTypeResolver, context, container)
      {
      }

      protected override void CreateAddItems(MoBiProject parent)
      {
         var menuBarItemRepository = _container.Resolve<IMenuBarItemRepository>();
         var newExpressionProfile = menuBarItemRepository[MenuBarItemIds.NewExpressionProfile].WithCaption(MenuNames.NewExpressionProfile);

         _allMenuItems.Add(newExpressionProfile);
         _allMenuItems.Add(CreateAddExistingItemFor(parent));
      }
   }
}