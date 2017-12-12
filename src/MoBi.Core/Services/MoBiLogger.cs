using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ILogger = OSPSuite.Core.Services.ILogger;

namespace MoBi.Core.Services
{
   public class MoBiLogger : ILogger
   {
      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         Debug.Print($"{logLevel} - {message}");
      }
   }
}