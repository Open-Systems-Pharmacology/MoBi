using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;

namespace MoBi.BatchTool.Services
{
   public interface IBatchLogger : ILogger
   {
      void Clear();
   }

   public class BatchLogger : IBatchLogger
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly IList<string> _entries;

      public BatchLogger(IEventPublisher eventPublisher)
      {
         _entries = new List<string>();
         _eventPublisher = eventPublisher;
      }

      public void AddToLog(string message, NotificationType messageStatus = NotificationType.None)
      {
         var logEntry = new LogEntry(messageStatus, message);
         _entries.Add(logEntry.Display);
         _eventPublisher.PublishEvent(new LogEntryEvent(logEntry));
      }

      public void Clear()
      {
         _entries.Clear();
      }

      public IEnumerable<string> Entries => _entries;
   }
}