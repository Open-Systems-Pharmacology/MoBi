using System.Collections.Generic;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;
using MoBi.Assets;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleBuildingBlockNodeContextMenuFactory : MultipleNodeContextMenuFactory<IBuildingBlock>
   {
      private readonly IMoBiContext _context;
      private readonly IContainer _container;

      public MultipleBuildingBlockNodeContextMenuFactory(IMoBiContext context, IContainer container)
      {
         _context = context;
         _container = container;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<IBuildingBlock> buildingBlocks, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleBuildingBlockNodeContextMenu(buildingBlocks, _context, _container);
      }
   }

   public class MultipleBuildingBlockNodeContextMenu : ContextMenu<IReadOnlyList<IBuildingBlock>, IMoBiContext>
   {
      public MultipleBuildingBlockNodeContextMenu(IReadOnlyList<IBuildingBlock> buildingBlocks, IMoBiContext context, IContainer container)
         : base(buildingBlocks, context, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<IBuildingBlock> buildingBlocks, IMoBiContext context)
      {
         if (buildingBlocks.Count == 2 && (buildingBlocks[0].GetType() == buildingBlocks[1].GetType()))
            yield return ComparisonCommonContextMenuItems.CompareBuildingBlocksMenu(buildingBlocks, context, _container);

         yield return ObjectBaseCommonContextMenuItems.AddToJournal(buildingBlocks, _container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveMultipleBuildingBlocksUICommand, IReadOnlyList<IBuildingBlock>>(buildingBlocks, _container)
            .WithIcon(ApplicationIcons.Delete);
      }
   }
}