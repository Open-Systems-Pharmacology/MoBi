﻿using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Services;

namespace MoBi.Core.Services
{
   public class MoBiLogger : IOSPSuiteLogger
   {
      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         Debug.Print($"{logLevel} - {message}");
      }
   }
}