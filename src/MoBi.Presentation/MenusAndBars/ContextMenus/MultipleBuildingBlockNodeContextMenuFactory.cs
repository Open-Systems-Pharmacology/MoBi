using System.Collections.Generic;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleBuildingBlockNodeContextMenuFactory : MultipleNodeContextMenuFactory<IBuildingBlock>
   {
      private readonly IMoBiContext _context;

      public MultipleBuildingBlockNodeContextMenuFactory(IMoBiContext context)
      {
         _context = context;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<IBuildingBlock> buildingBlocks, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleBuildingBlockNodeContextMenu(buildingBlocks, _context);
      }
   }

   public class MultipleBuildingBlockNodeContextMenu : ContextMenu<IReadOnlyList<IBuildingBlock>, IMoBiContext>
   {
      public MultipleBuildingBlockNodeContextMenu(IReadOnlyList<IBuildingBlock> buildingBlocks, IMoBiContext context)
         : base(buildingBlocks, context)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<IBuildingBlock> buildingBlocks, IMoBiContext context)
      {
         if (buildingBlocks.Count == 2)
            yield return ComparisonCommonContextMenuItems.CompareObjectsMenu(buildingBlocks, context);

         yield return ObjectBaseCommonContextMenuItems.AddToJournal(buildingBlocks);
      }
   }
}