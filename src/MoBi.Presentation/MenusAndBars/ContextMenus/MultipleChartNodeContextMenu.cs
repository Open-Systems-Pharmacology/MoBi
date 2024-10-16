using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleChartNodeContextMenuFactory : MultipleNodeContextMenuFactory<CurveChart>
   {
      private readonly IContainer _container;

      public MultipleChartNodeContextMenuFactory(IContainer container)
      {
         _container = container;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<CurveChart> charts, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleChartNodeContextMenu(charts, _container);
      }

      public override bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return treeNodes.All(node => node.IsAnImplementationOf<ChartNode>());
      }
   }

   public class MultipleChartNodeContextMenu : ContextMenu<IReadOnlyList<CurveChart>>
   {

      public MultipleChartNodeContextMenu(IReadOnlyList<CurveChart> charts, IContainer container)
         : base(charts, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<CurveChart> charts)
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveMultipleSummaryChartUICommand, IReadOnlyList<CurveChart>>(charts, _container)
            .AsGroupStarter()
            .WithIcon(ApplicationIcons.Delete);
      }
   }
}