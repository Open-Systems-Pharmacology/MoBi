using System.Collections.Generic;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;

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
         if (buildingBlocks.Count == 2)
            yield return ComparisonCommonContextMenuItems.CompareObjectsMenu(buildingBlocks, context, _container);

         yield return ObjectBaseCommonContextMenuItems.AddToJournal(buildingBlocks, _container);
      }
   }
}