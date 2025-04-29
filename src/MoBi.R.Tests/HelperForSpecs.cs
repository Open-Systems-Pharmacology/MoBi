using System;
using System.IO;

namespace MoBi.R.Tests
{
   public static class HelperForSpecs
   {
      public static string TestFileFullPath(string fileName)
      {
         var dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
         return Path.Combine(dataFolder, fileName);
      }
   }
}