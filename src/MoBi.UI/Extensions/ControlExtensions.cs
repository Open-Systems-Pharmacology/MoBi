using System.Drawing;
using System.Windows.Forms;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Extensions
{
   public static class ControlExtensions
   {
      public static Point CalculateRelativeOffset(this IView view, Point location, Control control)
      {
         var controlLocation = control.PointToScreen(Point.Empty);
         var viewLocation = view.DowncastTo<Control>().PointToScreen(Point.Empty);
         var offset = new Point(controlLocation.X - viewLocation.X, controlLocation.Y - viewLocation.Y);
         return new Point(location.X + offset.X, location.Y + offset.Y);
      }
   }
}