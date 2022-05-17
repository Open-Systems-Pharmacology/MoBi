using OSPSuite.Assets;
using DevExpress.XtraTab;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Extensions
{
   public static class TabPageExtensions
   {
      public static void InitWith(this XtraTabPage tabPage, string caption, ApplicationIcon icon)
      {
         tabPage.Text = caption;
         tabPage.SetImage(icon);
      }
   }
}