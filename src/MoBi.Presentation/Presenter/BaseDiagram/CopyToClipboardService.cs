using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace MoBi.Presentation.Presenter.BaseDiagram
{
   public class CopyToClipboardService
   {
      /// <summary>
      /// Copy the chart as enhanced metafile into the clipboard
      /// </summary>
      public static void CopyToClipboard(Bitmap bitmap, IntPtr controlHandle)
      {
         using (var ms = new MemoryStream())
         {
            bitmap.Save(ms, ImageFormat.Emf);
            ms.Seek(0, SeekOrigin.Begin);

            ClipboardMetafileHelper.PutEnhMetafileOnClipboard(controlHandle, new Metafile(ms));
         }
      }

      private static class ClipboardMetafileHelper
      {
         [DllImport("user32.dll")]
         private static extern bool OpenClipboard(IntPtr hWndNewOwner);

         [DllImport("user32.dll")]
         private static extern bool EmptyClipboard();

         [DllImport("user32.dll")]
         private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

         [DllImport("user32.dll")]
         private static extern bool CloseClipboard();

         [DllImport("gdi32.dll")]
         private static extern IntPtr CopyEnhMetaFile(IntPtr hemfSrc, IntPtr hNULL);

         [DllImport("gdi32.dll")]
         private static extern bool DeleteEnhMetaFile(IntPtr hemf);

         // Metafile mf is set to a state that is not valid inside this function.
         public static bool PutEnhMetafileOnClipboard(IntPtr hWnd, Metafile mf)
         {
            bool bResult = false;
            IntPtr hEMF, hEMF2;
            hEMF = mf.GetHenhmetafile(); // invalidates mf
            if (!hEMF.Equals(new IntPtr(0)))
            {
               hEMF2 = CopyEnhMetaFile(hEMF, new IntPtr(0));
               if (!hEMF2.Equals(new IntPtr(0)))
               {
                  if (OpenClipboard(hWnd))
                  {
                     if (EmptyClipboard())
                     {
                        IntPtr hRes = SetClipboardData(14, hEMF2);
                        bResult = hRes.Equals(hEMF2);
                        CloseClipboard();
                     }
                  }
               }
               DeleteEnhMetaFile(hEMF);
            }
            return bResult;
         }
      }

   }
}
