using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
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
   internal class ContextMenuSpecificationFactoryForSummaryChart : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = IoC.Resolve<ContextMenuForSummaryChart>();
         return contextMenu.InitializeWith(viewItem.DowncastTo<ChartNode>().Tag);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<ChartNode>();
      }
   }

   public class ContextMenuForSummaryChart : ContextMenuBase
   {
      private IList<IMenuBarItem> _allMenuItems;

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ICurveChart chart)
      {
         _allMenuItems = new List<IMenuBarItem>();

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditSummaryChartUICommand, ICurveChart>(chart)
            .WithIcon(ApplicationIcons.Edit));

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveSummaryChartUICommand, ICurveChart>(chart)
            .WithIcon(ApplicationIcons.Delete));

         return this;
      }
   }
}