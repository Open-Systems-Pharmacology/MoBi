using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;

namespace MoBi.UI.Extensions
{
   public static class MemoEditExtensions
   {
      /// <summary>
      /// Shows/hides scrollbars based on the text width. Call this in response to text or size changes
      /// </summary>
      public static void AutoScrollBars(this MemoEdit memoEditObjectPaths)
      {
         memoEditObjectPaths.BeginInvoke(new MethodInvoker(() =>
         {
            var vi = memoEditObjectPaths.GetViewInfo() as MemoEditViewInfo;
            var cache = new GraphicsCache(memoEditObjectPaths.CreateGraphics());
            var args = new ObjectInfoArgs();
            var g = memoEditObjectPaths.CreateGraphics();
            var width = g.MeasureString(memoEditObjectPaths.Text, vi.Appearance.Font, 0, vi.Appearance.GetStringFormat()).Width + 6;
            args.Bounds = new Rectangle(0, 0, (int)width, vi.ClientRect.Height);
            var rect = vi.BorderPainter.CalcBoundsByClientRectangle(args);
            cache.Dispose();

            memoEditObjectPaths.Properties.ScrollBars = rect.Width > memoEditObjectPaths.Width ? ScrollBars.Horizontal : ScrollBars.None;
         }));
      }
   }
}
