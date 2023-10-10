using static OSPSuite.UI.UIConstants.Size;

namespace MoBi.UI
{
   public static class UIConstants
   {
      public const int STATUS_WIDTH = 95;
      
      public static class UI
      {
         public const double SCREEN_RESIZE_FRACTION = 0.9;
         public static readonly int SIMULATION_VIEW_WIDTH = ScaleForScreenDPI(1200);
         public static readonly int SIMULATION_VIEW_HEIGHT = ScaleForScreenDPI(800);
      }
   }
}