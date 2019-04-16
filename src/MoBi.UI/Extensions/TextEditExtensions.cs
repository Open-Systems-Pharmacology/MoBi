using System;
using System.Threading.Tasks;
using DevExpress.XtraEditors;

namespace MoBi.UI.Extensions
{
   public static class TextEditExtensions
   {
      public static async Task Debounce(this TextEdit textEdit, Action action)
      {
         int startLength = textEdit.Text.Length;
         //Add some debouncing capability to ensure that we do not call the validation all the time that a key stroke is pressed
         await Task.Delay(300);
         if (startLength != textEdit.Text.Length)
            return;

         action();
      }
   }
}