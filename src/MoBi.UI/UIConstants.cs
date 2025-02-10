using System.Drawing;
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

         public static readonly Size SELECT_SINGLE_SIZE = new Size(ScaleForScreenDPI(475), ScaleForScreenDPI(160));
         public static readonly Size PARAMETER_SELECTION_SIZE = new Size(ScaleForScreenDPI(1200), ScaleForScreenDPI(800));
      }
   }
}