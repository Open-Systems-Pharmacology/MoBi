using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Snapshots;
using MoBi.Core.Snapshots.Services;
using OSPSuite.CLI.Core.RunOptions;
using OSPSuite.CLI.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Qualification;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Assets.Error;
using ModelProject = MoBi.Core.Domain.Model.MoBiProject;
using SnapshotProject = MoBi.Core.Snapshots.Project;

namespace MoBi.CLI.Core.Services
{
   public class QualificationRunner : QualificationRunner<SnapshotProject, ModelProject>
   {
      private readonly IMoBiContext _context;
      private readonly IProjectPersistor _projectPersistor;
      private readonly ISimulationPersistor _simulationPersistor;

      public QualificationRunner(IMoBiContext context,
         IProjectPersistor projectPersistor,
         IOSPSuiteLogger logger,
         IDataRepositoryExportTask dataRepositoryExportTask,
         IJsonSerializer jsonSerializer,
         ISnapshotTask snapshotTask,
         ISimulationPersistor simulationPersistor) : base(logger, dataRepositoryExportTask, jsonSerializer, snapshotTask)
      {
         _context = context;
         _projectPersistor = projectPersistor;
         _simulationPersistor = simulationPersistor;
      }

      protected override void LoadProjectContext(ModelProject project)
      {
         _context.LoadFrom(project);
      }

      protected override IEnumerable<PlotMapping> RetrievePlotDefinitionsForSimulation(SimulationPlot simulationPlot, SnapshotProject snapshotProject)
      {
         var simulationName = simulationPlot.Simulation;
         var simulation = simulationFrom(snapshotProject, simulationName);

         var charts = new[] { simulation.Chart, simulation.SimulationResidualVsTimeChart };

         return charts.Select(chart => new PlotMapping
         {
            Plot = chart,
            SectionId = simulationPlot.SectionId,
            SectionReference = simulationPlot.SectionReference,
            Simulation = simulationName,
            Project = snapshotProject.Name
         });
      }

      private Simulation simulationFrom(SnapshotProject snapshotProject, string simulationName)
      {
         var referenceSimulation = snapshotProject.Simulations?.FindByName(simulationName);
         return referenceSimulation ?? throw new QualificationRunException(CannotFindSimulationInSnapshot(simulationName, snapshotProject.Name));
      }

      protected override async Task SwapSimulationParametersIn(SnapshotProject projectSnapshot, SimulationParameterSwap simulationParameter)
      {
         var (parameterPath, simulationName, snapshotPath) = simulationParameter;
         var referenceSnapshot = await SnapshotProjectFromFile(snapshotPath);

         var referenceSimulation = simulationFrom(referenceSnapshot, simulationName);

         var referenceParameter = referenceSimulation.ParameterByPath(parameterPath);
         if (referenceParameter == null)
            throw new QualificationRunException(CannotFindSimulationParameterInSnapshot(parameterPath, simulationName, referenceSnapshot.Name));

         simulationParameter.TargetSimulations?.Each(targetSimulationName =>
         {
            var targetSimulation = simulationFrom(projectSnapshot, targetSimulationName);
            targetSimulation.AddOrUpdate(referenceParameter);
         });
      }
       
      protected override Task SwapBuildingBlockIn(SnapshotProject projectSnapshot, BuildingBlockSwap buildingBlockSwap)
      {
         // TODO https://github.com/Open-Systems-Pharmacology/MoBi/issues/1990
         return Task.CompletedTask;
      }

      protected override void ValidateInputs(SnapshotProject snapshotProject, QualificationConfiguration configuration)
      {
         if (configuration.Inputs?.Any() == true)
            throw new QualificationRunException(AppConstants.Validation.InputsAreNotSupportedInMoBiQualification);
      }

      protected override string ProjectExtension => AppConstants.Filter.MOBI_PROJECT_EXTENSION;

      protected override SimulationExportMode ExportMode(QualificationRunOptions runOptions) => SimulationExportMode.Pkml;

      protected override Task ExportToMarkdown(object buildingBlock, string fileFullPath, int? inputSectionLevel)
      {
         // TODO https://github.com/Open-Systems-Pharmacology/MoBi/issues/1958
         return Task.CompletedTask;
      }

      protected override Task<SimulationMapping[]> ExportSimulationsIn(ModelProject project, ExportRunOptions exportRunOptions)
      {
         var nameOfSimulationsToExport = (exportRunOptions.Simulations ?? Enumerable.Empty<string>()).ToList();
         if (!nameOfSimulationsToExport.Any() && exportRunOptions.ExportAllSimulationsIfListIsEmpty)
            nameOfSimulationsToExport.AddRange(project.Simulations.AllNames());

         var simulationExports = new List<SimulationMapping>();

         foreach (var simulationName in nameOfSimulationsToExport)
         {
            var simulation = project.Simulations.FindByName(simulationName);
            if (simulation == null)
            {
               _logger.AddWarning($"Simulation '{simulationName}' was not found in project '{project.Name}'", project.Name);
               continue;
            }

            simulationExports.Add(exportSimulation(simulation, exportRunOptions, project));
         }

         return Task.FromResult(simulationExports.ToArray());
      }

      private SimulationMapping exportSimulation(IMoBiSimulation simulation, ExportRunOptions exportRunOptions, ModelProject project)
      {
         var simulationFile = FileHelper.RemoveIllegalCharactersFrom(simulation.Name);
         var simulationFolder = Path.Combine(exportRunOptions.OutputFolder, simulationFile);

         DirectoryHelper.CreateDirectory(simulationFolder);

         var simulationExport = new SimulationMapping
         {
            Project = project.Name,
            Path = simulationFolder,
            Simulation = simulation.Name,
            SimulationFile = simulationFile
         };

         var simulationExportOptions = new SimulationExportOptions
         {
            OutputFolder = simulationFolder,
            ExportMode = exportRunOptions.ExportMode,
            ProjectName = project.Name,
         };

         _simulationPersistor.Save(new SimulationTransfer { Simulation = simulation }, simulationExportOptions.TargetPathFor(simulation, Constants.Filter.PKML_EXTENSION));

         return simulationExport;
      }

      protected override object BuildingBlockBy(ModelProject project, Input input)
      {
         throw new NotImplementedException();
      }

      protected override void SaveProjectContext(string projectFile)
      {
         _projectPersistor.Save(_context.CurrentProject, _context);
      }
   }
}