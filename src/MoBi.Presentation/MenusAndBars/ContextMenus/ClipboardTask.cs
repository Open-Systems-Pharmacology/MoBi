using System;
using System.Data;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IClipboardTask
   {
      /// <summary>
      ///    Creates a DataObject suitable for inserting into the clipboard
      ///    The data object contains all of the data types supported by the suite including
      ///    a "DataTable" type which is used when pasting into other places of the suite.
      /// </summary>
      /// <param name="dataTable">The data table to be copied to clipboard</param>
      /// <param name="includeHeaders">if true, headers are added to the data output from the datatable column caption</param>
      /// <returns>A dataobject containing all the clipboard types supported by SPBSuite</returns>
      DataObject CreateDataObject(DataTable dataTable, bool includeHeaders = true);

      /// <summary>
      ///    Gets an enhanced metafile from the clipboard if one exists.
      /// </summary>
      /// <returns>The metafile from the clipboard, or null if the clipboard cannot be accessed or does not contain metafile</returns>
      Metafile GetEnhMetafileFromClipboard();

      /// <summary>
      ///    Puts the metafile on the clipboard
      /// </summary>
      /// <param name="control">The control containing the metafile</param>
      /// <param name="metaFile">The metafile</param>
      /// <returns></returns>
      bool PutEnhMetafileOnClipboard(Control control, Metafile metaFile);
   }

   public class ClipboardTask : IClipboardTask
   {
      private readonly IDataTableToHtmlClipboardFormatMapper _dataTableToHtmlMapper;
      private readonly IDataTableToTextMapper _dataTableToTextMapper;
      private const uint CF_ENHMETAFILE = 14;
      private const uint CF_UNICODETEXT = 13;

      public ClipboardTask(IDataTableToHtmlClipboardFormatMapper dataTableToHtmlMapper, IDataTableToTextMapper dataTableToTextMapper)
      {
         _dataTableToHtmlMapper = dataTableToHtmlMapper;
         _dataTableToTextMapper = dataTableToTextMapper;
      }

      // The dependencies are not injected because this constructor is used by the Visual Studio Designer
      public ClipboardTask()
         : this(new DataTableToHtmlClipboardFormatMapper(new DataTableToHtmlMapper()), new DataTableToTextMapper())
      {
      }

      public DataObject CreateDataObject(DataTable dataTable, bool includeHeaders = true)
      {
         var htmlFragment = _dataTableToHtmlMapper.MapFrom(dataTable, includeHeaders);
         var plainText = _dataTableToTextMapper.MapFrom(dataTable, includeHeaders);

         var dataObject = new DataObject();
         dataObject.SetData(DataFormats.Html, htmlFragment);
         dataObject.SetData(DataFormats.Text, plainText);
         dataObject.SetData(DataFormats.UnicodeText, plainText);
         dataObject.SetData(Captions.DataTable, dataTable);

         return dataObject;
      }

      [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
      private static extern bool IsClipboardFormatAvailable(uint format);

      [DllImport("user32.dll")]
      private static extern bool OpenClipboard(IntPtr hWndNewOwner);

      [DllImport("user32.dll")]
      private static extern bool EmptyClipboard();

      [DllImport("user32.dll")]
      private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

      [DllImport("user32.dll")]
      private static extern bool CloseClipboard();

      [DllImport("gdi32.dll")]
      private static extern IntPtr CopyEnhMetaFile(IntPtr hemfSrc, IntPtr hNull);

      [DllImport("gdi32.dll")]
      private static extern bool DeleteEnhMetaFile(IntPtr hemf);

      [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
      private static extern IntPtr GetClipboardData(uint format);

      public Metafile GetEnhMetafileFromClipboard()
      {
         try
         {
            if (OpenClipboard(IntPtr.Zero))
            {
               if (IsClipboardFormatAvailable(CF_ENHMETAFILE))
               {
                  var ptr = GetClipboardData(CF_ENHMETAFILE);
                  if (!ptr.Equals(IntPtr.Zero))
                  {
                     // Taking a clone seems to be necessary for copy/paste interoperability
                     // removing the clone results in the pasted chart disappearing when a second
                     // chart is pasted.
                     return new Metafile(ptr, true).Clone() as Metafile;
                  }
               }
            }
         }
         finally
         {
            CloseClipboard();
         }

         return null;
      }

      public bool PutEnhMetafileOnClipboard(Control control, Metafile metaFile)
      {
         var hWnd = control.Handle;
         var bResult = false;
         var handleForEnhancedMetaFile = metaFile.GetHenhmetafile();
         if (handleForEnhancedMetaFile.Equals(new IntPtr(0)))
            return false;

         var copyHandle = CopyEnhMetaFile(handleForEnhancedMetaFile, new IntPtr(0));
         if (!copyHandle.Equals(new IntPtr(0)))
         {
            if (OpenClipboard(hWnd))
            {
               if (EmptyClipboard())
               {
                  var hRes = SetClipboardData(CF_ENHMETAFILE, copyHandle);
                  bResult = hRes.Equals(copyHandle);
                  CloseClipboard();
               }
            }
         }

         DeleteEnhMetaFile(handleForEnhancedMetaFile);
         return bResult;
      }

      public bool PutTextToClipboard(string text)
      {
         if (text.IsNullOrEmpty()) return false;

         var hGlobal = IntPtr.Zero;
         try
         {
            if (OpenClipboard(IntPtr.Zero))
            {
               if (EmptyClipboard())
               {
                  hGlobal = Marshal.StringToHGlobalUni(text);
                  SetClipboardData(CF_UNICODETEXT, hGlobal);
                  CloseClipboard();
                  return true;
               }
            }
         }
         finally
         {
            if (hGlobal != IntPtr.Zero)
               Marshal.FreeHGlobal(hGlobal);
            CloseClipboard();
         }

         return false;

      }
   }
}