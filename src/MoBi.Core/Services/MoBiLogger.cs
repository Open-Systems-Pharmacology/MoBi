using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MoBi.Assets;
using OSPSuite.Core.Services;

namespace MoBi.Core.Services
{
   public class MoBiLogger : IOSPSuiteLogger
   {
      public string DefaultCategoryName { get; set; } = AppConstants.PRODUCT_NAME;

      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         Debug.Print($"{logLevel} - {message}");
      }

   }
}