using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.SBML;
using MoBi.Core.Services;
using MoBi.Engine.Sbml;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   public class SBMLTestSuiteSpecs : ContextForSBMLIntegration<ISbmlTask>
   {
      private static string _directory;
      private IModelConstructor _modelConstructor;
      private ISimModelExporter _simModelExporter;
      private IDataRepositoryExportTask _dateRepositoryTask;

      protected override void Context()
      {
         base.Context();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _simModelExporter = IoC.Resolve<ISimModelExporter>();
         _dateRepositoryTask = IoC.Resolve<IDataRepositoryExportTask>();
         _directory = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\.."), "Core", "SBML", "Testfiles", "SBMLTestSuite"))));
      }

      [Observation, Ignore("Test incomplete")]
      public void should_be_able_to_read_all_files_and_run_the_most()
      {
         var directories = new DirectoryInfo(_directory).EnumerateDirectories();
         var messages = new List<string>();
         foreach (var directory in directories)
         {
            _sbmlTask = IoC.Resolve<ISbmlTask>().DowncastTo<SbmlTask>();
            var caseName = directory.Name;
            Debug.Print(caseName);
            var fileName = Path.Combine(directory.FullName, $"{caseName}-sbml-l3v1.xml");
            var context = IoC.Resolve<IMoBiContext>();
            context.NewProject();
            var project = context.CurrentProject;
            IMoBiCommand command;
            try
            {
               command = _sbmlTask.ImportModelFromSbml(fileName, project);
            }
            catch (Exception e)
            {
               messages.Add($"Import: {caseName} {e.Message}:");
               continue;
            }

            if (command.IsEmpty())
            {
               messages.Add($"Import: {caseName}");
               continue;
            }

            addSettings(project, Path.Combine(directory.FullName, $"{caseName}-settings.txt"));
            var buildConfiguration = generateBuildConfiguration(project);
            var result = _modelConstructor.CreateModelFrom(buildConfiguration, caseName);
            if (result.IsInvalid)
            {
               messages.Add(caseName);
               continue;
            }

            var simulation = new MoBiSimulation {Configuration = buildConfiguration, Model = result.Model};
            var simModelManager = new SimModelManager(_simModelExporter, new SimModelSimulationFactory(),
               new DataFactory(IoC.Resolve<IMoBiDimensionFactory>(), IoC.Resolve<IDisplayUnitRetriever>(), IoC.Resolve<IDataRepositoryTask>()));
            var runResults = simModelManager.RunSimulation(simulation);
            if (!runResults.Success)
            {
               messages.Add(caseName);
               continue;
            }

            var dt = _dateRepositoryTask.ToDataTable(runResults.Results);
            dt.First().ExportToCSV(Path.Combine(directory.FullName, $"{caseName}-result_mobi.csv"));
            _sbmlTask = null;
         }

         messages.Count.ShouldBeEqualTo(0, messages.ToString("\n"));
      }

      private void addSettings(MoBiProject project, string settingFileName)
      {
         var simulationSettings = IoC.Resolve<ISimulationSettingsFactory>().CreateDefault();
      }

      private SimulationConfiguration generateBuildConfiguration(MoBiProject project)
      {
         var simulationConfiguration = new SimulationConfiguration();

         var module = new Module
         {
            project.Modules.First().SpatialStructure,
            project.Modules.First().Molecules,
            project.Modules.First().Reactions,
            project.Modules.First().PassiveTransports,
            project.Modules.First().EventGroups,
            project.Modules.First().Observers
         };


         var moduleConfiguration = new ModuleConfiguration(module, project.Modules.First().InitialConditionsCollection.First(), project.Modules.First().ParameterValuesCollection.First());

         simulationConfiguration.SimulationSettings = project.SimulationSettings;


         simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         return simulationConfiguration;
      }
   }
}