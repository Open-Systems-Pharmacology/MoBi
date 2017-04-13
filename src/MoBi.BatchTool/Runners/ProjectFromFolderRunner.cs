using System;
using System.IO;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.BatchTool.Services;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.BatchTool.Runners
{
   public class ProjectFromFolderRunner : IBatchRunner
   {
      private readonly IBatchLogger _logger;
      private readonly ISerializationTask _serializationTask;
      private readonly IMoBiContext _context;
      private readonly ISimulationBatchRunner _simulationBatchRunner;

      public ProjectFromFolderRunner(IBatchLogger logger, ISerializationTask serializationTask, IMoBiContext context, ISimulationBatchRunner simulationBatchRunner)
      {
         _logger = logger;
         _serializationTask = serializationTask;
         _context = context;
         _simulationBatchRunner = simulationBatchRunner;
      }

      public Task RunBatch(dynamic parameters)
      {
         var inputFolder = parameters.inputFolder;
         return Task.Run(() =>
         {
            _logger.AddInSeparator($"Starting batch run: {DateTime.Now.ToIsoFormat(withSeconds: true)}");
            var inputDirectory = new DirectoryInfo(inputFolder);

            if (!inputDirectory.Exists)
            {
               _logger.AddError($"Input folder '{inputFolder}' does not exist");
               return;
            }

            var allProjectFiles = inputDirectory.GetFiles(AppConstants.FileFilter.MOBI_PROJECT_FILTER);
            if (allProjectFiles.Length == 0)
            {
               _logger.AddError($"No MoBi file found in '{inputFolder}'");
               return;
            }

            var begin = DateTime.UtcNow;
            foreach (var projectFile in allProjectFiles)
            {
               try
               {
                  compute(projectFile.FullName);
               }
               catch (Exception e)
               {
                  _logger.AddError(e.FullMessage());
               }
            }
            var end = DateTime.UtcNow;
            var timeSpent = end - begin;
            _logger.AddInSeparator($"{allProjectFiles.Length} projects computed in '{timeSpent.ToDisplay()}'");
            _logger.AddInSeparator($"Batch run finished: {DateTime.Now.ToIsoFormat(withSeconds: true)}");
         });
      }

      private void compute(string projectFile)
      {
         _logger.AddInfo($"Computing file '{projectFile}'");
         _serializationTask.LoadProject(projectFile);
         var project = _context.CurrentProject;

         foreach (var simulation in project.Simulations)
         {
            _logger.AddInfo($"Computing simulation '{simulation.Name}'");
            try
            {
               _simulationBatchRunner.Compute(simulation);
            }
            catch (Exception e)
            {
               _logger.AddError(e.FullMessage());
            }
         }
      }
   }
}