using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Infrastructure.Serialization.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Core.Serialization.ORM
{
   public interface IContextPersistor
   {
      void Load(IMoBiContext context, string projectFullPath);
      void Save(IMoBiContext context);
      MoBiProject NewProject(IMoBiContext context);
      void CloseProject(IMoBiContext context);
      void LoadJournal(IMoBiContext context, string journalPath, string projectFullPath = null, bool showJournal = false);
   }

   public class ContextPersistor : IContextPersistor
   {
      private readonly IProjectConverterLogger _projectConverterLogger;
      private readonly ISessionManager _sessionManager;
      private readonly IProjectPersistor _projectPersistor;
      private readonly IHistoryManagerPersistor _historyManagerPersistor;
      private readonly IProjectFileCompressor _projectFileCompressor;
      private readonly IEventPublisher _eventPublisher;
      private readonly IJournalTask _journalTask;
      private readonly IJournalLoader _journalLoader;
      private readonly ISimulationSettingsFactory _simulationSettingsFactory;

      public ContextPersistor(
         IProjectConverterLogger projectConverterLogger, 
         ISessionManager sessionManager, 
         IProjectPersistor projectPersistor,
         IHistoryManagerPersistor historyManagerPersistor, 
         IProjectFileCompressor projectFileCompressor, 
         IEventPublisher eventPublisher, 
         IJournalTask journalTask, 
         IJournalLoader journalLoader,
         ISimulationSettingsFactory simulationSettingsFactory)
      {
         _projectConverterLogger = projectConverterLogger;
         _sessionManager = sessionManager;
         _projectPersistor = projectPersistor;
         _historyManagerPersistor = historyManagerPersistor;
         _projectFileCompressor = projectFileCompressor;
         _eventPublisher = eventPublisher;
         _journalTask = journalTask;
         _journalLoader = journalLoader;
         _simulationSettingsFactory = simulationSettingsFactory;
      }

      public void Load(IMoBiContext context, string projectFullPath)
      {
         try
         {
            _projectConverterLogger.Clear();
            _sessionManager.OpenFactoryFor(projectFullPath);
            using (_sessionManager.OpenSession())
            {
               //load history first so that possible conversion command can be added to history
               context.HistoryManager = _historyManagerPersistor.Load(_sessionManager.CurrentSession) as IMoBiHistoryManager;

               var project = _projectPersistor.Load(context);

               if (project == null) return;

               project.FilePath = projectFullPath;
               project.Name = FileHelper.FileNameFromFileFullPath(projectFullPath);
               project.HasChanged = false;

               LoadJournal(context, project.JournalPath, projectFullPath);
            }
         }
         catch (Exception)
         {
            // Exception occurs while opening the project! 
            // close the file and rethrow the exception
            _sessionManager.CloseFactory();
            context.Clear();
            throw;
         }

         var notificationMessages = _projectConverterLogger.AllMessages();
         if (notificationMessages.Any())
            _eventPublisher.PublishEvent(new ShowProjectConversionNotificationsEvent(new ReadOnlyCollection<NotificationMessage>(notificationMessages.ToList())));
      }

      public void Save(IMoBiContext context)
      {
         var project = context.CurrentProject;

         verifyFileNotReadOnly(project.FilePath);
         _sessionManager.CreateFactoryFor(project.FilePath);

         using (var session = _sessionManager.OpenSession())
         using (var transaction = session.BeginTransaction())
         {
            context.UpdateJournalPathRelativeTo(project.FilePath);
            _projectPersistor.Save(project, context);
            _historyManagerPersistor.Save(context.HistoryManager, _sessionManager.CurrentSession);
            transaction.Commit();
         }

         // after save was successful, compress file
         _projectFileCompressor.Compress(project.FilePath);
         project.Name = FileHelper.FileNameFromFileFullPath(project.FilePath);
         project.HasChanged = false;
         context.ProjectIsReadOnly = false;
         GC.Collect();
      }

      private void verifyFileNotReadOnly(string projectFilePath)
      {
         var fileInfo = new FileInfo(projectFilePath);
         if (!fileInfo.Exists || !fileInfo.IsReadOnly)
            return;

         throw new OSPSuiteException(AppConstants.Exceptions.FileIsReadOnly(projectFilePath));
      }

      public MoBiProject NewProject(IMoBiContext context)
      {
         _sessionManager.CloseFactory();
         context.NewProject();
         var project = context.CurrentProject;
         project.SimulationSettings = _simulationSettingsFactory.CreateDefault();
         return project;
      }

      public void CloseProject(IMoBiContext context)
      {
         _sessionManager.CloseFactory();
         context.Clear();
      }

      public void LoadJournal(IMoBiContext context, string journalPath, string projectFullPath = null, bool showJournal = false)
      {
         var journal = _journalLoader.Load(journalPath, projectFullPath);
         context.Journal = journal;

         if (showJournal)
            _journalTask.ShowJournal();
      }
   }
}