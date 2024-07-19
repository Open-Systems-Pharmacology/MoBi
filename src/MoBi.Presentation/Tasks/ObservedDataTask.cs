using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Core.Domain.Constants.ObservedData;
using Command = OSPSuite.Assets.Command;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace MoBi.Presentation.Tasks
{
   public interface IObservedDataTask : OSPSuite.Core.Domain.Services.IObservedDataTask, IObservedDataConfiguration
   {
      void AddObservedDataToProject();
      void LoadObservedDataIntoProject();
      void DeleteAllResultsFrom(IMoBiSimulation simulation);
      void DeleteAllResultsFromAllSimulation();

      /// <summary>
      ///    Removes selected <paramref name="resultsToRemove" /> from their respective simulations
      /// </summary>
      void RemoveResultsFromSimulations(IReadOnlyList<DataRepository> resultsToRemove);

      void AddAndReplaceObservedDataFromConfigurationToProject(ImporterConfiguration configuration, IReadOnlyList<DataRepository> observedDataFromSameFile);

      void RemoveMultipleModules(IReadOnlyList<Module> modulesToRemove);
   }

   public class ObservedDataTask : OSPSuite.Core.Domain.Services.ObservedDataTask, IObservedDataTask
   {
      private readonly IDataImporter _dataImporter;
      private readonly IMoBiContext _context;
      private readonly IInteractionTask _interactionTask;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IObjectBaseNamingTask _namingTask;

      public ObservedDataTask(
         IDataImporter dataImporter,
         IMoBiContext context,
         IDialogCreator dialogCreator,
         IInteractionTask interactionTask,
         IDataRepositoryExportTask dataRepositoryTask,
         IContainerTask containerTask,
         IObjectTypeResolver objectTypeResolver,
         IBuildingBlockRepository buildingBlockRepository,
         IObjectBaseNamingTask namingTask,
         IConfirmationManager confirmationManager) : base(dialogCreator, context, dataRepositoryTask, containerTask, objectTypeResolver, confirmationManager)
      {
         _dataImporter = dataImporter;
         _interactionTask = interactionTask;
         _buildingBlockRepository = buildingBlockRepository;
         _namingTask = namingTask;
         _context = context;
      }

      public void AddObservedDataToProject()
      {
         var (metaDataCategories, settings) = initializeSettings();

         var (dataRepositories, configuration) = _dataImporter.ImportDataSets(
            metaDataCategories,
            _dataImporter.ColumnInfosForObservedData(),
            settings,
            _dialogCreator.AskForFileToOpen(Captions.Importer.OpenFile, Captions.Importer.ImportFileFilter, DirectoryKey.OBSERVED_DATA)
         );

         if (dataRepositories == null || configuration == null)
            return;

         foreach (var repository in dataRepositories)
         {
            AddObservedDataToProject(repository);
            adjustRepositoryPaths(repository);
         }

         AddImporterConfigurationToProject(configuration);
      }

      private void adjustRepositoryPaths(DataRepository repository)
      {
         var baseGrid = repository.BaseGrid;
         var baseGridName = baseGrid.Name.Replace(ObjectPath.PATH_DELIMITER, "\\");
         baseGrid.QuantityInfo = new QuantityInfo(new[] { repository.Name, baseGridName }, QuantityType.Time);

         foreach (var col in repository.AllButBaseGrid())
         {
            var colName = col.Name.Replace(ObjectPath.PATH_DELIMITER, "\\");
            var quantityInfo = new QuantityInfo(new[] { repository.Name, colName }, QuantityType.Undefined);
            col.QuantityInfo = quantityInfo;
         }
      }

      public void LoadObservedDataIntoProject()
      {
         var filename = _interactionTask.AskForFileToOpen(AppConstants.Dialog.Load(ObjectTypes.ObservedData), Filter.PKML_FILE_FILTER, DirectoryKey.MODEL_PART);
         if (filename.IsNullOrEmpty())
            return;

         var dataRepository = _interactionTask.LoadItems<DataRepository>(filename).FirstOrDefault();
         if (dataRepository == null)
            return;

         AddObservedDataToProject(dataRepository);
      }

      public void DeleteAllResultsFrom(IMoBiSimulation simulation)
      {
         var viewResult = _dialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveAllResultsFrom(simulation.Name));
         if (viewResult == ViewResult.No)
            return;

         _context.AddToHistory(deleteAllResultsFromSimulationCommand(simulation).Run(_context));
      }

      public void DeleteAllResultsFromAllSimulation()
      {
         var viewResult = _dialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveAllResultsFromProject());
         if (viewResult == ViewResult.No)
            return;

         var simulations = _context.CurrentProject.Simulations;
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = AppConstants.Commands.DeleteAllResultsFromAllSimulations,
         };

         simulations.Each(s => macroCommand.AddCommand(deleteAllResultsFromSimulationCommand(s)));
         _context.AddToHistory(macroCommand.Run(_context));
      }

      private static MoBiMacroCommand deleteAllResultsFromSimulationCommand(IMoBiSimulation simulation)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = AppConstants.Commands.DeleteResultsFromSimulation(simulation.Name),
         };

         if (simulation.ResultsDataRepository != null)
            macroCommand.AddCommand(new ClearResultsCommand(simulation));

         simulation.HistoricResults.Each(x => macroCommand.Add(new RemoveHistoricResultFromSimulationCommand(simulation, x)));
         return macroCommand;
      }

      private (IReadOnlyList<MetaDataCategory>, DataImporterSettings) initializeSettings()
      {
         var dataImporterSettings = new DataImporterSettings
         {
            IconName = ApplicationIcons.MoBi.IconName,
            Caption = $"{AppConstants.PRODUCT_NAME} - {AppConstants.Captions.ImportObservedData}",
            CheckMolWeightAgainstMolecule = false
         };

         addNamingPatterns(dataImporterSettings);
         dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = MOLECULE;
         dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = MOLECULAR_WEIGHT;

         var metaDataCategories = _dataImporter.DefaultMetaDataCategoriesForObservedData().ToList();
         populateMetaDataLists(metaDataCategories);

         return (metaDataCategories, dataImporterSettings);
      }

      private void populateMetaDataLists(IList<MetaDataCategory> metaDataCategories)
      {
         addPredefinedOrganValues(metaDataCategories.FindByName(ORGAN));
         addPredefinedCompartmentValues(metaDataCategories.FindByName(COMPARTMENT));
         addPredefinedMoleculesForImporter(metaDataCategories.FindByName(MOLECULE));
      }

      public override void Rename(DataRepository dataRepository)
      {
         var newName = _namingTask.RenameFor(dataRepository, _context.CurrentProject.AllObservedData.Select(x => x.Name).ToList());

         if (string.IsNullOrEmpty(newName))
            return;

         _context.AddToHistory(new RenameObservedDataCommand(dataRepository, newName).Run(_context));
      }

      public override void UpdateMolWeight(DataRepository observedData)
      {
      }

      private IMoBiSimulation getSimulationWithHistoricResult(DataRepository repository)
      {
         var simulations = _context.CurrentProject.Simulations;
         return simulations.FirstOrDefault(sim => sim.HistoricResults.Contains(repository.Id));
      }

      private IMoBiSimulation getSimulationWithCurrentResult(DataRepository repository)
      {
         var simulations = _context.CurrentProject.Simulations;
         return simulations.FirstOrDefault(sim => sim.ResultsDataRepository != null && sim.ResultsDataRepository.Id.Equals(repository.Id));
      }

      public void RemoveResultsFromSimulations(IReadOnlyList<DataRepository> resultsToRemove)
      {
         if (_dialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveSelectedResultsFromSimulations) != ViewResult.Yes)
            return;

         var macroCommand = new MoBiMacroCommand
         {
            Description = AppConstants.Commands.RemoveMultipleResultsFromSimulations,
            ObjectType = AppConstants.MoBiObjectTypes.Data,
            CommandType = AppConstants.Commands.DeleteCommand
         };

         resultsToRemove.Each(result => { macroCommand.Add(removeResultFromSimulationCommand(result)); });

         _context.AddToHistory(macroCommand.Run(_context));
      }

      public void AddAndReplaceObservedDataFromConfigurationToProject(ImporterConfiguration configuration,
         IReadOnlyList<DataRepository> observedDataFromSameFile)
      {
         var importedObservedData = getObservedDataFromImporter(configuration);
         var reloadDataSets =
            _dataImporter.CalculateReloadDataSetsFromConfiguration(importedObservedData.ToList(), observedDataFromSameFile.ToList());

         foreach (var dataSet in reloadDataSets.NewDataSets)
         {
            AddObservedDataToProject(dataSet);
            adjustRepositoryPaths(dataSet);
         }

         foreach (var dataSet in reloadDataSets.DataSetsToBeDeleted.ToArray()) //toDo it should be checked if to array solves the deleting problem
         {
            Delete(dataSet);
         }

         foreach (var dataSet in reloadDataSets.OverwrittenDataSets)
         {
            //TODO this here should be tested
            var existingDataSet = findDataRepositoryInList(observedDataFromSameFile, dataSet);

            foreach (var column in dataSet.Columns)
            {
               var datacolumn = new DataColumn(column.Id, column.Name, column.Dimension, column.BaseGrid)
               {
                  QuantityInfo = column.QuantityInfo,
                  DataInfo = column.DataInfo,
                  IsInternal = column.IsInternal,
                  Values = column.Values
               };

               if (column.IsBaseGrid())
               {
                  existingDataSet.BaseGrid.Values = datacolumn.Values;
               }
               else
               {
                  var existingColumn = existingDataSet.FirstOrDefault(x => x.Name == column.Name);
                  if (existingColumn == null)
                     existingDataSet.Add(column);
                  else
                     existingColumn.Values = column.Values;
               }
            }
         }
      }

      public void RemoveMultipleModules(IReadOnlyList<Module> modulesToRemove)
      {
         if (_dialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveMultipleModules) != ViewResult.Yes)
            return;

         var macroCommand = new MoBiMacroCommand
         {
            Description = AppConstants.Commands.RemoveMultipleModules,
            ObjectType = AppConstants.MoBiObjectTypes.Module,
            CommandType = AppConstants.Commands.DeleteCommand
         };

         modulesToRemove.Each(x => macroCommand.Add(new RemoveModuleCommand(x)));
         _context.AddToHistory(macroCommand.Run(_context));
      }

      private DataRepository findDataRepositoryInList(IEnumerable<DataRepository> dataRepositoryList, DataRepository targetDataRepository)
      {
         return (from dataRepo in dataRepositoryList
            let result = targetDataRepository.ExtendedProperties.KeyValues.All(keyValuePair =>
               dataRepo.ExtendedProperties[keyValuePair.Key].ValueAsObject.ToString() == keyValuePair.Value.ValueAsObject.ToString())
            where result
            select dataRepo).FirstOrDefault();
      }

      private IEnumerable<DataRepository> getObservedDataFromImporter(ImporterConfiguration configuration)
      {
         var (metaDataCategories, dataImporterSettings) = initializeSettings();

         var importedObservedData = _dataImporter.ImportFromConfiguration(
            configuration,
            metaDataCategories,
            _dataImporter.ColumnInfosForObservedData(),
            dataImporterSettings,
            _dialogCreator.AskForFileToOpen(Captions.Importer.OpenFile, Captions.Importer.ImportFileFilter, DirectoryKey.OBSERVED_DATA)
         );
         return importedObservedData;
      }

      private ICommand removeResultFromSimulationCommand(DataRepository dataRepository)
      {
         var parentSimulation = getSimulationWithHistoricResult(dataRepository);
         if (parentSimulation != null)
            return removeHistoricResultFromSimulationCommand(dataRepository, parentSimulation);

         parentSimulation = getSimulationWithCurrentResult(dataRepository);
         if (parentSimulation != null)
            return clearResultsCommand(parentSimulation);

         return new MoBiEmptyCommand();
      }

      private static ClearResultsCommand clearResultsCommand(IMoBiSimulation parentSimulation)
      {
         return new ClearResultsCommand(parentSimulation);
      }

      private static RemoveHistoricResultFromSimulationCommand removeHistoricResultFromSimulationCommand(DataRepository repository,
         IMoBiSimulation parentSimulation)
      {
         return new RemoveHistoricResultFromSimulationCommand(parentSimulation, repository);
      }

      private void addPredefinedMoleculesForImporter(MetaDataCategory metaDataCategory)
      {
         if (metaDataCategory == null)
            return;

         metaDataCategory.ShouldListOfValuesBeIncluded = true;
         allMolecules().OrderBy(molecule => molecule.Name).Each(molecule => addInfoToCategory(metaDataCategory, molecule));
      }

      private IEnumerable<IContainer> allMolecules()
      {
         return _buildingBlockRepository.MoleculeBlockCollection.SelectMany(buildingBlock => { return buildingBlock.Select(builder => builder); }).DistinctBy(builder => builder.Name);
      }

      private static void addUndefinedValueTo(MetaDataCategory metaDataCategory)
      {
         metaDataCategory.ListOfValues.Add(AppConstants.Undefined, AppConstants.Undefined);
      }

      private void addPredefinedOrganValues(MetaDataCategory metaDataCategory)
      {
         if (metaDataCategory == null)
            return;

         addUndefinedValueTo(metaDataCategory);
         metaDataCategory.ShouldListOfValuesBeIncluded = true;
         allOrgans().OrderBy(org => org.Name).Each(organ => addInfoToCategory(metaDataCategory, organ));
      }

      private void addNamingPatterns(DataImporterSettings dataImporterSettings)
      {
         dataImporterSettings.AddNamingPatternMetaData(
            FILE
         );

         dataImporterSettings.AddNamingPatternMetaData(
            FILE,
            SHEET
         );

         dataImporterSettings.AddNamingPatternMetaData(
            MOLECULE,
            SPECIES,
            ORGAN,
            COMPARTMENT
         );

         dataImporterSettings.AddNamingPatternMetaData(
            MOLECULE,
            SPECIES,
            ORGAN,
            COMPARTMENT,
            STUDY_ID,
            GENDER,
            DOSE,
            ROUTE,
            SUBJECT_ID
         );
      }

      private void addPredefinedCompartmentValues(MetaDataCategory metaDataCategory)
      {
         if (metaDataCategory == null)
            return;

         addUndefinedValueTo(metaDataCategory);
         metaDataCategory.ShouldListOfValuesBeIncluded = true;
         allCompartments().OrderBy(comp => comp.Name).Each(compartment => addInfoToCategory(metaDataCategory, compartment));
      }

      private IEnumerable<IContainer> allOrgans()
      {
         return allTopContainers().SelectMany(allSubContainers).Where(container => container.ContainerType == ContainerType.Organ)
            .DistinctBy(x => x.Name);
      }

      private IEnumerable<IContainer> allCompartments()
      {
         return allTopContainers().SelectMany(allSubContainers).Where(container => container.ContainerType == ContainerType.Compartment)
            .DistinctBy(x => x.Name);
      }

      private IEnumerable<IContainer> allTopContainers()
      {
         return _buildingBlockRepository.SpatialStructureCollection.SelectMany(spatialStructure => spatialStructure.TopContainers);
      }

      private IEnumerable<IContainer> allSubContainers(IContainer container)
      {
         yield return container;
         foreach (var item in container.GetChildren<IContainer>().SelectMany(allSubContainers))
         {
            yield return item;
         }
      }

      private void addInfoToCategory(MetaDataCategory metaDataCategory, IContainer container)
      {
         metaDataCategory.ListOfValues.Add(container.Name, container.Name);

         var icon = ApplicationIcons.IconByName(container.Icon);
         if (icon != ApplicationIcons.EmptyIcon)
            metaDataCategory.ListOfImages.Add(container.Name, icon.IconName);
      }

      public IEnumerable<string> PredefinedValuesFor(string name)
      {
         if (string.Equals(name, ORGAN))
            return predefinedValuesForCategory(addPredefinedOrganValues);

         if (string.Equals(name, COMPARTMENT))
            return predefinedValuesForCategory(addPredefinedCompartmentValues);

         if (string.Equals(name, MOLECULE))
            return predefinedValuesForCategory(addPredefinedMoleculesForImporter);

         return Enumerable.Empty<string>();
      }

      private IEnumerable<string> predefinedValuesForCategory(Action<MetaDataCategory> action)
      {
         var category = new MetaDataCategory();
         action(category);
         return category.ListOfValues.Values;
      }

      public IReadOnlyList<string> DefaultMetaDataCategories => new[]
      {
         MOLECULE, COMPARTMENT, ORGAN
      };

      public IReadOnlyList<string> ReadOnlyMetaDataCategories => new List<string>();

      public bool MolWeightAlwaysEditable => true;

      public bool MolWeightVisible => true;
   }
}