﻿using System;
using System.IO;
using System.Threading.Tasks;
using MoBi.BatchTool.Services;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.BatchTool.Runners
{
   public class PkmlFileFromFolderRunner : IBatchRunner
   {
      private readonly IMoBiContext _context;
      private readonly IBatchLogger _logger;
      private readonly ISimulationBatchRunner _simulationBatchRunner;
      private readonly IProjectTask _projectTask;

      public PkmlFileFromFolderRunner(IMoBiContext context, IBatchLogger logger, ISimulationBatchRunner simulationBatchRunner, IProjectTask projectTask)
      {
         _context = context;
         _logger = logger;
         _simulationBatchRunner = simulationBatchRunner;
         _projectTask = projectTask;
      }

      public Task RunBatch(dynamic parameters)
      {
         var inputFolder = parameters.inputFolder;
         return Task.Run(() =>
         {
            _logger.AddInSeparator("Starting batch run: {0}".FormatWith(DateTime.Now.ToIsoFormat(withSeconds: true)));

            var inputDirectory = new DirectoryInfo(inputFolder);
            if (!inputDirectory.Exists)
            {
               _logger.AddError($"Input folder '{inputFolder}' does not exist");
               return;
            }

            var allSimulationFiles = inputDirectory.GetFiles(Constants.Filter.PKML_FILE_FILTER);
            if (allSimulationFiles.Length == 0)
            {
               _logger.AddError($"No Pkml file found in '{inputFolder}'");
               return;
            }

            var begin = DateTime.UtcNow;
            foreach (var simulationFile in allSimulationFiles)
            {
               try
               {
                  compute(simulationFile.FullName);
               }
               catch (Exception e)
               {
                  _logger.AddError(e.FullMessage());
               }
            }
            var end = DateTime.UtcNow;
            var timeSpent = end - begin;
            _logger.AddInSeparator("{0} simulations computed in '{1}'".FormatWith(allSimulationFiles.Length, timeSpent.ToDisplay()));

            _logger.AddInSeparator("Batch run finished: {0}".FormatWith(DateTime.Now.ToIsoFormat(withSeconds: true)));
         });
      }

      private void compute(string pkmlFile)
      {
         _logger.AddInfo("Computing file '{0}'".FormatWith(pkmlFile));
         var simulation = loadSimulationFromFile(pkmlFile);
         _simulationBatchRunner.Compute(simulation);
      }

      private MoBiSimulation loadSimulationFromFile(string pkmlFile)
      {
         _context.NewProject();
         return _projectTask.LoadSimulationTransferDataFromFile(pkmlFile).Simulation.DowncastTo<MoBiSimulation>();
      }
   }
}