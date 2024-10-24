﻿using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MoBi.Assets;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;

namespace MoBi.BatchTool.Services
{
   public interface IBatchLogger : IOSPSuiteLogger
   {
      void Clear();
   }

   public class BatchLogger : IBatchLogger
   {
      public string DefaultCategoryName { get; set; } = AppConstants.PRODUCT_NAME;

      private readonly IEventPublisher _eventPublisher;
      private readonly IList<string> _entries;

      public BatchLogger(IEventPublisher eventPublisher)
      {
         _entries = new List<string>();
         _eventPublisher = eventPublisher;
      }

      public void Clear()
      {
         _entries.Clear();
      }

      public IEnumerable<string> Entries => _entries;

      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         var logEntry = new LogEntry(logLevel, message);
         _entries.Add(logEntry.Display);
         _eventPublisher.PublishEvent(new LogEntryEvent(logEntry));
      }
   }
}