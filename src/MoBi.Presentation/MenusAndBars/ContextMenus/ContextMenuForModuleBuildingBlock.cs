using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForModuleBuildingBlock<TBuildingBlock> : ContextMenuForBuildingBlock<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      public ContextMenuForModuleBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      protected override IMenuBarItem CreateDeleteItemFor(TBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithRemoveCommand(buildingBlock.Module, buildingBlock);
      }
   }
}