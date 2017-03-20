using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.SBML;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;

namespace MoBi.IntegrationTests
{
   public class SBMLTestSuiteSpecs : ContextForSBMLIntegration<ISBMLTask>
   {
      private static string _directory;
      private IBuildConfigurationFactory _buildConfigurationFactory;
      private IModelConstructor _modelConstructor;
      private ISimModelExporter _simModelExporter;
      private IDataRepositoryTask _dateRepositoryTask;

      protected override void Context()
      {
         base.Context();

         _buildConfigurationFactory = IoC.Resolve<IBuildConfigurationFactory>();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _simModelExporter = IoC.Resolve<ISimModelExporter>();
         _dateRepositoryTask = IoC.Resolve<IDataRepositoryTask>();
         _directory = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\.."), "Core", "SBML", "Testfiles", "SBMLTestSuite"))));
      }

      [Observation, Ignore("Test incomplete")]
      public void should_be_able_to_read_all_files_and_run_the_most()
      {
         var directories = new DirectoryInfo(_directory).EnumerateDirectories();
         var messages = new List<string>();
         foreach (var directory in directories)
         {
            _sbmlTask = IoC.Resolve<ISBMLTask>();
            var caseName = directory.Name;
            Debug.Print(caseName);
            var fileName = Path.Combine(directory.FullName, String.Format("{0}-sbml-l3v1.xml", caseName));
            var context = IoC.Resolve<IMoBiContext>();
            context.NewProject();
            var project = context.CurrentProject;
            IMoBiCommand command;
            try
            {
               command = _sbmlTask.ImportModelFromSBML(fileName, project);
            }
            catch (Exception e)
            {
               messages.Add(String.Format("Import: {0} {1}:", caseName, e.Message));
               continue;
            }

            if (command.IsEmpty())
            {
               messages.Add(String.Format("Import: {0}", caseName));
               continue;
            }
            addjEmptyBBIfneeded(project);
            addSettings(project, Path.Combine(directory.FullName, String.Format("{0}-settings.txt", caseName)));
            var buildConfigurtion = genreateBuildConfiguration(project);
            var result = _modelConstructor.CreateModelFrom(buildConfigurtion, caseName);
            if (result.IsInvalid)
            {
               messages.Add(caseName);
               continue;
            }
            var simulation = new MoBiSimulation {BuildConfiguration = buildConfigurtion, Model = result.Model};
            var simModelManager = new SimModelManager(_simModelExporter, new SimModelSimulationFactory(), 
               new DataFactory(IoC.Resolve<IMoBiDimensionFactory>(), new SBMLTestDataNamingService(), IoC.Resolve<IObjectPathFactory>(), IoC.Resolve<IDisplayUnitRetriever>()));
            var runResults = simModelManager.RunSimulation(simulation);
            if (!runResults.Success)
            {
               messages.Add(caseName);
               continue;
            }
            var dt = _dateRepositoryTask.ToDataTable(runResults.Results);
            dt.First().ExportToCSV(Path.Combine(directory.FullName, String.Format("{0}-result_mobi.csv", caseName)));
            _sbmlTask = null;
         }
         messages.Count.ShouldBeEqualTo(0, messages.ToString("\n"));
      }

      private void addSettings(IMoBiProject project, string settingFileName)
      {
         var simulationSettings = IoC.Resolve<ISimulationSettingsFactory>().CreateDefault();
         project.AddBuildingBlock(simulationSettings);
      }

      private IMoBiBuildConfiguration genreateBuildConfiguration(IMoBiProject project)
      {
         var buildConfiguration = _buildConfigurationFactory.Create();
         buildConfiguration.SpatialStructure = project.SpatialStructureCollection.First();
         buildConfiguration.Molecules = project.MoleculeBlockCollection.First();
         buildConfiguration.Reactions = project.ReactionBlockCollection.First();
         buildConfiguration.PassiveTransports = project.PassiveTransportCollection.First();
         buildConfiguration.MoleculeStartValues = project.MoleculeStartValueBlockCollection.First();
         buildConfiguration.ParameterStartValues = project.ParametersStartValueBlockCollection.First();
         buildConfiguration.SimulationSettings = project.SimulationSettingsCollection.First();
         buildConfiguration.EventGroups = project.EventBlockCollection.First();
         buildConfiguration.Observers = project.ObserverBlockCollection.First();
         return buildConfiguration;
      }

      private void addjEmptyBBIfneeded(IMoBiProject project)
      {
         project.AddBuildingBlock(new ObserverBuildingBlock().WithName("Empty"));
         if (!project.EventBlockCollection.Any())
            project.AddBuildingBlock(new EventGroupBuildingBlock().WithName("Empty"));
         if (!project.ReactionBlockCollection.Any())
            project.AddBuildingBlock(new MoBiReactionBuildingBlock().WithName("Empty"));
         if (!project.PassiveTransportCollection.Any())
            project.AddBuildingBlock(new PassiveTransportBuildingBlock().WithName("Empty"));
         if (!project.MoleculeStartValueBlockCollection.Any())
            project.AddBuildingBlock(new MoleculeStartValuesBuildingBlock().WithName("Empty"));
         if (!project.ParametersStartValueBlockCollection.Any())
            project.AddBuildingBlock(new ParameterStartValuesBuildingBlock().WithName("Empty"));
      }
   }

   internal class SBMLTestDataNamingService : IDataNamingService
   {
      public string GetTimeName()
      {
         return AppConstants.TimeColumName;
      }

      public string GetNewRepositoryName()
      {
         return "results";
      }

      public string GetEntityName(string id)
      {
         return id;
      }
   }
}