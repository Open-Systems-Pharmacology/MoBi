using System.Diagnostics;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;

namespace MoBi.Core.Services
{
   public class MoBiLogger : ILogger
   {
      public void AddToLog(string message, NotificationType messageStatus = NotificationType.None)
      {
         Debug.Print($"{messageStatus} - {message}");
      }
   }
}