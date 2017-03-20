using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain.Model;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleBuildingBlockInfoNodeContextMenuFactory : MultipleNodeContextMenuFactory<IBuildingBlockInfo>
   {
      private readonly IMoBiContext _context;

      public MultipleBuildingBlockInfoNodeContextMenuFactory(IMoBiContext context)
      {
         _context = context;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<IBuildingBlockInfo> buildingBlockInfos, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleBuildingBlockNodeContextMenu(buildingBlockInfos.Select(x => x.UntypedBuildingBlock).ToList(), _context);
      }

      public override bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         var allBuildingBlockInfo = AllTagsFor(treeNodes);
         return base.IsSatisfiedBy(treeNodes, presenter) && allBuildingBlockInfo.Select(x => x.UntypedBuildingBlock.GetType()).Distinct().Count() == 1;
      }
   }
}