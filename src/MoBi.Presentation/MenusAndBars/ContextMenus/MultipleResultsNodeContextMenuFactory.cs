using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleResultsNodeContextMenuFactory : MultipleNodeContextMenuFactory<DataRepository>
   {
      protected override IContextMenu CreateFor(IReadOnlyList<DataRepository> historicalResults, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleResultsNodeContextMenu(historicalResults);
      }

      public override bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return base.IsSatisfiedBy(treeNodes, presenter) && allNodesBelongToSimulations(treeNodes);
      }

      private static bool allNodesBelongToSimulations(IReadOnlyList<ITreeNode> treeNodes)
      {
         return treeNodes.All(node =>
         {
            var historicalResultNode = node as HistoricalResultsNode;
            return historicalResultNode.BelongsToSimulation();
         });
      }
   }

   public class MultipleResultsNodeContextMenu : ContextMenu<IReadOnlyList<DataRepository>>
   {
      public MultipleResultsNodeContextMenu(IReadOnlyList<DataRepository> objectRequestingContextMenu) : base(objectRequestingContextMenu)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<DataRepository> results)
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveMultipleResultsUICommand, IReadOnlyList<DataRepository>>(results)
            .AsGroupStarter()
            .WithIcon(ApplicationIcons.Delete);
      }
   }
}