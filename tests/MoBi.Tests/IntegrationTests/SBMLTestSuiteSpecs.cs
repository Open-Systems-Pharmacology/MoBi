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

            addEmptyBBIfNeeded(project);
            addSettings(project, Path.Combine(directory.FullName, $"{caseName}-settings.txt"));
            var buildConfigurtion = generateBuildConfiguration(project);
            var result = _modelConstructor.CreateModelFrom(buildConfigurtion, caseName);
            if (result.IsInvalid)
            {
               messages.Add(caseName);
               continue;
            }

            var simulation = new MoBiSimulation {Configuration = buildConfigurtion, Model = result.Model};
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
            project.SpatialStructureCollection.First(),
            project.MoleculeBlockCollection.First(),
            project.ReactionBlockCollection.First(),
            project.PassiveTransportCollection.First(),
            project.EventBlockCollection.First(),
            project.ObserverBlockCollection.First()
         };


         var moduleConfiguration = new ModuleConfiguration(module, project.MoleculeStartValueBlockCollection.First(), project.ParametersStartValueBlockCollection.First());

         simulationConfiguration.SimulationSettings = project.SimulationSettings;


         simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         return simulationConfiguration;
      }

      private void addEmptyBBIfNeeded(MoBiProject project)
      {
         project.AddBuildingBlock(new ObserverBuildingBlock().WithName("Empty"));
         if (!project.EventBlockCollection.Any())
            project.AddBuildingBlock(new EventGroupBuildingBlock().WithName("Empty"));
         if (!project.ReactionBlockCollection.Any())
            project.AddBuildingBlock(new MoBiReactionBuildingBlock().WithName("Empty"));
         if (!project.PassiveTransportCollection.Any())
            project.AddBuildingBlock(new PassiveTransportBuildingBlock().WithName("Empty"));
         if (!project.MoleculeStartValueBlockCollection.Any())
            project.AddBuildingBlock(new InitialConditionsBuildingBlock().WithName("Empty"));
         if (!project.ParametersStartValueBlockCollection.Any())
            project.AddBuildingBlock(new ParameterValuesBuildingBlock().WithName("Empty"));
      }
   }
}