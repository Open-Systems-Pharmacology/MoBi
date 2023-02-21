using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuSpecificationFactoryForSummaryChart : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForSummaryChart(IContainer container)
      {
         _container = container;
      }
      
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForSummaryChart(_container);
         return contextMenu.InitializeWith(viewItem.DowncastTo<ChartNode>().Tag);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<ChartNode>();
      }
   }

   public class ContextMenuForSummaryChart : ContextMenuBase
   {
      private readonly IContainer _container;
      private IList<IMenuBarItem> _allMenuItems;

      public ContextMenuForSummaryChart(IContainer container)
      {
         _container = container;
      }
      
      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(CurveChart chart)
      {
         _allMenuItems = new List<IMenuBarItem>();

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditSummaryChartUICommand, CurveChart>(chart, _container)
            .WithIcon(ApplicationIcons.Edit));

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveSummaryChartUICommand, CurveChart>(chart, _container)
            .WithIcon(ApplicationIcons.Delete));

         return this;
      }
   }
}