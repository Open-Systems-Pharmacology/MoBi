using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging.Abstractions;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public class PresenterLogger : ILogger
   {
      private readonly IEventPublisher _eventPublisher;
      public string Name { get; }

      public PresenterLogger(string name)
      {
         _eventPublisher = IoC.Resolve<IEventPublisher>();
         Name = name;
      }

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
      {
         if (formatter == null)
            throw new ArgumentNullException(nameof(formatter));

         string message = formatter(state, exception);
         if (string.IsNullOrEmpty(message) && exception == null)
            return;

         writeMessage(logLevel, Name, eventId.Id, message, exception);
      }

      private void writeMessage(LogLevel logLevel, string name, int eventIdId, string message, Exception exception)
      {
         var logEntry = new LogEntry(logLevel, message);
         _eventPublisher.PublishEvent(new LogEntryEvent(logEntry));
      }

      public bool IsEnabled(LogLevel logLevel) => true;

      public IDisposable BeginScope<TState>(TState state) => NullLogger.Instance.BeginScope(state);
   }
   public class PresenterLoggerProvider : ILoggerProvider
   {
      private readonly ConcurrentDictionary<string, PresenterLogger> _loggers = new ConcurrentDictionary<string, PresenterLogger>();

      public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, createLoggerImplementation);

      private PresenterLogger createLoggerImplementation(string name) => new PresenterLogger(name);

      public void Dispose()
      {
      }
   }

   public static class PresenterLoggingBuilderExtensions
   {
      public static ILoggingBuilder AddPresenter(this ILoggingBuilder builder)
      {
         builder.Services.AddSingleton<ILoggerProvider, PresenterLoggerProvider>(serviceProvider => new PresenterLoggerProvider());
         return builder;
      }
   }
}
