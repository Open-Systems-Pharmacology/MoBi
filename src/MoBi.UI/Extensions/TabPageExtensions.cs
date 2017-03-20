using OSPSuite.Assets;
using DevExpress.XtraTab;

namespace MoBi.UI.Extensions
{
   public static class TabPageExtensions
   {
      public static void InitWith(this XtraTabPage tabPage, string caption, ApplicationIcon icon)
      {
         tabPage.Text = caption;
         tabPage.Image = icon.ToImage(IconSizes.Size16x16);
      }
   }
}