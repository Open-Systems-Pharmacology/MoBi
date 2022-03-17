using static OSPSuite.UI.UIConstants.Size;

namespace MoBi.UI
{
   public static class UIConstants
   {
      public const int STATUS_WIDTH = 95;

      public static class UI
      {
         public const double SCREEN_RESIZE_FRACTION = 0.9;
         public const int SIMULATION_VIEW_WIDTH = 600;
         public const int SIMULATION_VIEW_HEIGHT = 800;
         public const int APPLY_TO_SELECTION_WIDTH = 420;
         public static readonly int APPLY_TO_SELECTION_HEIGHT = ScaleForScreenDPI(25);
         public static readonly int START_VALUES_LEGEND_HEIGHT = ScaleForScreenDPI(50);
         public static readonly int START_VALUES_LEGEND_WIDTH = ScaleForScreenDPI(390);
      }
   }
}