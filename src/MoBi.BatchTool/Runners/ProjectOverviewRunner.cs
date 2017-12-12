using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.BatchTool.Services;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using Newtonsoft.Json;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.BatchTool.Runners
{
   public class ProjectOverviewRunner : IBatchRunner
   {
      private readonly IBatchLogger _logger;
      private readonly IContextPersistor _contextPersistor;
      private readonly IMoBiContext _context;

      public ProjectOverviewRunner(IBatchLogger logger, IContextPersistor contextPersistor, IMoBiContext context)
      {
         _logger = logger;
         _contextPersistor = contextPersistor;
         _context = context;
      }

      public Task RunBatch(dynamic parameters)
      {
         return Task.Run(() =>
         {
            string inputFolder = parameters.inputFolder;

            clear();

            var inputDirectory = new DirectoryInfo(inputFolder);
            if (!inputDirectory.Exists)
               throw new ArgumentException($"Input folder '{inputFolder}' does not exist");

            var allProjectFiles = inputDirectory.GetFiles(AppConstants.Filter.MOBI_PROJECT_FILTER, SearchOption.AllDirectories);
            if (allProjectFiles.Length == 0)
               throw new ArgumentException($"No project file found in '{inputFolder}'");

            var outputFile = Path.Combine(inputFolder, "output.json");

            _logger.AddInfo($"Starting project overview run for {allProjectFiles.Length} projects: {DateTime.Now.ToIsoFormat()}");

            var begin = DateTime.UtcNow;

            var allProjects = new AllProjects {InputFolder = inputFolder};
            foreach (var projectFile in allProjectFiles)
            {
               allProjects.Projects.Add(addProjectInfo(projectFile));
            }

            exportAllProjects(allProjects, outputFile);
            var end = DateTime.UtcNow;
            var timeSpent = end - begin;

            _logger.AddInfo($"Finished project overview run for {allProjectFiles.Length} projects in {timeSpent.ToDisplay()}'");
         });
      }

      private ProjectInfo addProjectInfo(FileInfo projectFile)
      {
         var projectInfo = new ProjectInfo {FullPath = projectFile.FullName, Name = projectFile.Name};
         _logger.AddInfo($"Loading project file '{projectFile.FullName}'");

         try
         {
            _contextPersistor.Load(_context, projectFile.FullName);
            var project = _context.CurrentProject;
            projectInfo.CompoundNames = project.MoleculeBlockCollection.SelectMany(x => x).AllNames().ToList();

            foreach (var observedData in project.AllObservedData)
            {
               projectInfo.ObservedDataList.Add(observedDataInfoFrom(observedData));
            }
         }
         catch (Exception e)
         {
            projectInfo.Error = e.FullMessage();
            _logger.AddError(e.FullMessage());
            _logger.AddError(e.FullStackTrace());
         }
         finally
         {
            _contextPersistor.CloseProject(_context);
         }

         return projectInfo;
      }

      private void exportAllProjects(AllProjects allProjects, string outputFile)
      {
         // serialize JSON directly to a file
         using (var file = File.CreateText(outputFile))
         {
            var serializer = new JsonSerializer();
            serializer.Serialize(file, allProjects);
         }
      }

      private ObservedDataInfo observedDataInfoFrom(DataRepository observedData)
      {
         var observedDataInfo = new ObservedDataInfo {Name = observedData.Name};
         observedData.ExtendedProperties.Each(prop => { observedDataInfo.MetaData.Add($"{prop.DisplayName}={prop.ValueAsObject}"); });
         return observedDataInfo;
      }

      private void clear()
      {
         _logger.Clear();
      }

      internal class AllProjects
      {
         public List<ProjectInfo> Projects { get; set; } = new List<ProjectInfo>();
         public string InputFolder { get; set; }
      }

      internal class ProjectInfo
      {
         public string Name { get; set; }
         public string FullPath { get; set; }
         public string Error { get; set; } = string.Empty;
         public List<string> CompoundNames { get; set; } = new List<string>();
         public List<ObservedDataInfo> ObservedDataList { get; set; } = new List<ObservedDataInfo>();
      }

      internal class ObservedDataInfo
      {
         public string Name { get; set; }
         public List<string> MetaData { get; set; } = new List<string>();
      }
   }
}