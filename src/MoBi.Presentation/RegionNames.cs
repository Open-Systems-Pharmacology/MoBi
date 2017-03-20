using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Regions;

namespace MoBi.Presentation
{
   public static class RegionNames
   {
      private static readonly IList<RegionName> _allRegions = new List<RegionName>();

      public static RegionName BuildingBlockExplorer = createRegionName("BuildingBlockExplorer", AppConstants.MenuNames.BuildingBlockExplorer, ApplicationIcons.BuildingBlockExplorer);
      public static RegionName SimulationExplorer = createRegionName("ProjectExplorer", AppConstants.MenuNames.SimulationExplorer, ApplicationIcons.SimulationExplorer);
      public static RegionName History = createRegionName("History", AppConstants.BarNames.History, ApplicationIcons.History);
      public static RegionName Search = createRegionName("Search", AppConstants.Captions.Search, ApplicationIcons.Search);
      public static RegionName NotificationList = createRegionName("NotificationList", AppConstants.BarNames.NotificationList, ApplicationIcons.Warning);
      public static RegionName Comparison = createRegionName("Comparison", AppConstants.BarNames.Comparison, ApplicationIcons.Comparison);
      public static RegionName Journal = createRegionName("Journal", Captions.Journal.JournalView, ApplicationIcons.Journal);
      public static RegionName JournalDiagram = createRegionName("JournalDiagram", Captions.Journal.JournalDiagram, ApplicationIcons.JournalDiagram);

      private static RegionName createRegionName(string name, string caption, ApplicationIcon icon)
      {
         var newRegion = new RegionName(name, caption, icon);
         _allRegions.Add(newRegion);
         return newRegion;
      }

      public static IEnumerable<RegionName> All()
      {
         return _allRegions.All();
      }
   }
}