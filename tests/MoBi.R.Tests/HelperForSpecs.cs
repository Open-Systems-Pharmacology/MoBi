using System;
using System.IO;

namespace MoBi.R.Tests
{
   internal static class HelperForSpecs
   {
      public static string DataTestFileFullPath(string fileName)
      {
         var dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
         return Path.Combine(dataFolder, fileName);
      }
   }
}
