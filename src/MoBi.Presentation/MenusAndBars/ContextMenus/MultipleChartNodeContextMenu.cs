using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleChartNodeContextMenuFactory : MultipleNodeContextMenuFactory<ICurveChart>
   {
      protected override IContextMenu CreateFor(IReadOnlyList<ICurveChart> charts, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleChartNodeContextMenu(charts);
      }

      public override bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return treeNodes.All(node => node.IsAnImplementationOf<ChartNode>());
      }
   }

   public class MultipleChartNodeContextMenu : ContextMenu<IReadOnlyList<ICurveChart>>
   {
      public MultipleChartNodeContextMenu(IReadOnlyList<ICurveChart> charts)
         : base(charts)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<ICurveChart> charts)
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveMultipleSummaryChartUICommand, IReadOnlyList<ICurveChart>>(charts)
            .AsGroupStarter()
            .WithIcon(ApplicationIcons.Delete);
      }
   }
}