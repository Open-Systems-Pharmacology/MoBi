using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using ColumnInfo = OSPSuite.Infrastructure.Import.Core.ColumnInfo;
using OSPSuite.Infrastructure.Import.Services;
using Command = OSPSuite.Assets.Command;
using CoreConstants = OSPSuite.Core.Domain.Constants;
using DimensionInfo = OSPSuite.Infrastructure.Import.Core;

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
   }

   public class ObservedDataTask : OSPSuite.Core.Domain.Services.ObservedDataTask, IObservedDataTask
   {
      private readonly IDataImporter _dataImporter;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IMoBiContext _context;
      private readonly IInteractionTask _interactionTask;
      private readonly IDimension _molWeightDimension;
      private readonly IDialogCreator _mobiDialogCreator;


      public ObservedDataTask(
         IDataImporter dataImporter,
         IDimensionFactory dimensionFactory,
         IMoBiContext context,
         IDialogCreator dialogCreator,
         IInteractionTask interactionTask,
         IDataRepositoryExportTask dataRepositoryTask,
         IContainerTask containerTask,
         IObjectTypeResolver objectTypeResolver) : base(dialogCreator, context, dataRepositoryTask, containerTask, objectTypeResolver)
      {
         _dataImporter = dataImporter;
         _mobiDialogCreator = dialogCreator;
         _interactionTask = interactionTask;
         _dimensionFactory = dimensionFactory;
         _context = context;
         _molWeightDimension = dimensionFactory.Dimension(AppConstants.DimensionNames.MOL_WEIGHT);
      }

      public void AddObservedDataToProject()
      {
         var data = _dataImporter.ImportDataSets(createMetaData().ToList(), createColumnInfos().ToList(), createDataImportSettings());

         if (data.DataRepositories == null || data.Configuration == null) return;

         foreach (var repository in data.DataRepositories)
         {
            AddObservedDataToProject(repository);
            adjustRepositoryPaths(repository);
         }
         AddImporterConfigurationToProject(data.Configuration);
      }

      private void adjustRepositoryPaths(DataRepository repository)
      {
         var baseGrid = repository.BaseGrid;
         var baseGridName = baseGrid.Name.Replace(ObjectPath.PATH_DELIMITER, "\\");
         baseGrid.QuantityInfo = new QuantityInfo(baseGrid.Name, new[] { repository.Name, baseGridName }, QuantityType.Time);

         foreach (var col in repository.AllButBaseGrid())
         {
            var colName = col.Name.Replace(ObjectPath.PATH_DELIMITER, "\\");
            var quantityInfo = new QuantityInfo(col.Name, new[] { repository.Name, colName }, QuantityType.Undefined);
            col.QuantityInfo = quantityInfo;
         }
      }

      public void LoadObservedDataIntoProject()
      {
         string filename = _interactionTask.AskForFileToOpen(AppConstants.Dialog.Load(ObjectTypes.ObservedData), Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
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
         var macoCommand = new MoBiMacroCommand
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = AppConstants.Commands.DeleteAllResultsFromAllSimulations,
         };

         simulations.Each(s => macoCommand.AddCommand(deleteAllResultsFromSimulationCommand(s)));
         _context.AddToHistory(macoCommand.Run(_context));
      }

      private static MoBiMacroCommand deleteAllResultsFromSimulationCommand(IMoBiSimulation simulation)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = AppConstants.Commands.DeleteResultsFromSimulation(simulation.Name),
         };

         if (simulation.Results != null)
            macroCommand.AddCommand(new ClearResultsCommand(simulation));

         simulation.HistoricResults.Each(x => macroCommand.Add(new RemoveHistoricResultFromSimulationCommand(simulation, x)));
         return macroCommand;
      }

      private DimensionInfo.DataImporterSettings createDataImportSettings()
      {
         var settings = new DimensionInfo.DataImporterSettings
         {
            IconName = ApplicationIcons.MoBi.IconName,
            Caption = $"{AppConstants.PRODUCT_NAME} - {AppConstants.Captions.ImportObservedData}"
         };
         addNamingPatterns(settings);
         settings.NameOfMetaDataHoldingMolecularWeightInformation = AppConstants.Parameters.MOLECULAR_WEIGHT;
         return settings;
      }

      public override void Rename(DataRepository dataRepository)
      {
         var newName = _mobiDialogCreator.AskForInput(AppConstants.Dialog.AskForNewName(dataRepository.Name),
            AppConstants.Captions.NewName,
            dataRepository.Name, _context.CurrentProject.AllObservedData.Select(x => x.Name));

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
         return simulations.FirstOrDefault(sim => sim.Results != null && sim.Results.Id.Equals(repository.Id));
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
         var dataImporterSettings = createDataImportSettings();

         //do we really need this in MoBi????
         dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = Constants.ObservedData.MOLECULE;
         var colInfos = createColumnInfos().ToList();

         var importedObservedData = _dataImporter.ImportFromConfiguration(configuration, createMetaData().ToList(),
            colInfos, dataImporterSettings);
         return importedObservedData;
      }

      private ICommand removeResultFromSimulationCommand(DataRepository dataRepository)
      {
         var parentSimulation = getSimulationWithHistoricResult(dataRepository);
         if (parentSimulation != null)
         {
            return removeHistoricResultFromSimulationCommand(dataRepository, parentSimulation);
         }

         parentSimulation = getSimulationWithCurrentResult(dataRepository);
         if (parentSimulation != null)
         {
            return clearResultsCommand(parentSimulation);
         }

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

      private IEnumerable<DimensionInfo.ColumnInfo> createColumnInfos()
      {
         var timeDimension = _dimensionFactory.Dimension(Constants.Dimension.TIME);
         var timeColumn = new DimensionInfo.ColumnInfo
         {
            DefaultDimension = timeDimension,
            Name = "Time",
            Description = "Time",
            DisplayName = "Time",
            IsMandatory = true,
            NullValuesHandling = DimensionInfo.NullValuesHandlingType.DeleteRow,
         };

         timeColumn.DimensionInfos.Add(new DimensionInfo.DimensionInfo { Dimension = timeDimension, IsMainDimension = true });
         yield return timeColumn;

         var mainDimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         var measurementInfo = new DimensionInfo.ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = "Measurement",
            Description = "Measurement",
            DisplayName = "Measurement",
            IsMandatory = true,
            NullValuesHandling = DimensionInfo.NullValuesHandlingType.DeleteRow,
            BaseGridName = timeColumn.Name
         };

         addDimensionsTo(measurementInfo, mainDimension);
         yield return measurementInfo;

         var errorInfo = new DimensionInfo.ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = "Error",
            Description = "Error",
            DisplayName = "Error",
            IsMandatory = false,
            NullValuesHandling = DimensionInfo.NullValuesHandlingType.Allowed,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = measurementInfo.Name
         };

         addDimensionsTo(errorInfo, mainDimension);
         yield return errorInfo;
      }

      private void addDimensionsTo(DimensionInfo.ColumnInfo columnInfo, IDimension mainDimension)
      {
         var timeDimension = _dimensionFactory.Dimension(Constants.Dimension.TIME);

         foreach (var dimension in _dimensionFactory.DimensionsSortedByName.Where(x => x != timeDimension))
         {
            columnInfo.DimensionInfos.Add(new DimensionInfo.DimensionInfo
            {
               Dimension = dimension,
               IsMainDimension = (dimension == mainDimension)
            });
         }
      }

      private IEnumerable<DimensionInfo.MetaDataCategory> createMetaData()
      {
         yield return new DimensionInfo.MetaDataCategory
         {
            Name = AppConstants.Parameters.MOLECULAR_WEIGHT,
            DisplayName = $"{AppConstants.Parameters.MOLECULAR_WEIGHT} [{_molWeightDimension.DefaultUnit}]",
            Description = AppConstants.Parameters.MOLECULAR_WEIGHT,
            MetaDataType = typeof(double),
            IsMandatory = false,
            MinValue = 0,
            MinValueAllowed = false
         };

         yield return createMetaDataCategory<string>(Constants.ObservedData.ORGAN,
            isMandatory: false,
            isListOfValuesFixed: true,
            fixedValuesRetriever: addPredefinedOrganValues,
            description: ObservedData.ObservedDataOrganDescription);

         yield return createMetaDataCategory<string>(Constants.ObservedData.COMPARTMENT,
            isMandatory: false,
            isListOfValuesFixed: true,
            fixedValuesRetriever: addPredefinedCompartmentValues,
            description: ObservedData.ObservedDataCompartmentDescription);

         yield return createMetaDataCategory<string>(Constants.ObservedData.MOLECULE,
            isMandatory: false, isListOfValuesFixed: true,
            fixedValuesRetriever: addPredefinedMoleculeNames,
            description: ObservedData.ObservedDataMoleculeDescription);
      }

      private void addPredefinedMoleculeNames(DimensionInfo.MetaDataCategory metaDataCategory)
      {
         metaDataCategory.ShouldListOfValuesBeIncluded = true;
         allMolecules().OrderBy(molecule => molecule.Name).Each(molecule => addInfoToCategory(metaDataCategory, molecule));
      }

      private IEnumerable<IContainer> allMolecules()
      {
         return _context.CurrentProject.MoleculeBlockCollection.SelectMany(buildingBlock => { return buildingBlock.Select(builder => builder); }).DistinctBy(builder => builder.Name);
      }

      private static void addUndefinedValueTo(DimensionInfo.MetaDataCategory metaDataCategory)
      {
         metaDataCategory.ListOfValues.Add(AppConstants.Undefined, AppConstants.Undefined);
      }

      private void addPredefinedOrganValues(DimensionInfo.MetaDataCategory metaDataCategory)
      {
         addUndefinedValueTo(metaDataCategory);
         metaDataCategory.ShouldListOfValuesBeIncluded = true;
         allOrgans().OrderBy(org => org.Name).Each(organ => addInfoToCategory(metaDataCategory, organ));
      }

      private void addNamingPatterns(DimensionInfo.DataImporterSettings dataImporterSettings)
      {
         dataImporterSettings.AddNamingPatternMetaData(
            Constants.FILE
         );

         dataImporterSettings.AddNamingPatternMetaData(
            Constants.FILE,
            Constants.SHEET
         );

         dataImporterSettings.AddNamingPatternMetaData(
            Constants.ObservedData.MOLECULE,
            Constants.ObservedData.SPECIES,
            Constants.ObservedData.ORGAN,
            Constants.ObservedData.COMPARTMENT
         );

         dataImporterSettings.AddNamingPatternMetaData(
            Constants.ObservedData.MOLECULE,
            Constants.ObservedData.SPECIES,
            Constants.ObservedData.ORGAN,
            Constants.ObservedData.COMPARTMENT,
            Constants.ObservedData.STUDY_ID,
            Constants.ObservedData.GENDER,
            Constants.ObservedData.DOSE,
            Constants.ObservedData.ROUTE,
            Constants.ObservedData.PATIENT_ID
         );
      }

      private void addPredefinedCompartmentValues(DimensionInfo.MetaDataCategory metaDataCategory)
      {
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
         return _context.CurrentProject.SpatialStructureCollection.SelectMany(spatialStructure => spatialStructure.TopContainers);
      }

      private IEnumerable<IContainer> allSubContainers(IContainer container)
      {
         yield return container;
         foreach (var item in container.GetChildren<IContainer>().SelectMany(allSubContainers))
         {
            yield return item;
         }
      }

      private void addInfoToCategory(DimensionInfo.MetaDataCategory metaDataCategory, IContainer container)
      {
         metaDataCategory.ListOfValues.Add(container.Name, container.Name);

         var icon = ApplicationIcons.IconByName(container.Icon);
         if (icon != ApplicationIcons.EmptyIcon)
            metaDataCategory.ListOfImages.Add(container.Name, icon.IconName);
      }

      private DimensionInfo.MetaDataCategory createMetaDataCategory<T>(string categoryName, bool isMandatory = false, bool isListOfValuesFixed = false, Action<DimensionInfo.MetaDataCategory> fixedValuesRetriever = null, string description = null)
      {
         var category = new DimensionInfo.MetaDataCategory
         {
            Name = categoryName,
            DisplayName = categoryName,
            Description = description ?? categoryName,
            MetaDataType = typeof(T),
            IsMandatory = isMandatory,
            IsListOfValuesFixed = isListOfValuesFixed
         };

         fixedValuesRetriever?.Invoke(category);

         return category;
      }

      public IEnumerable<string> PredefinedValuesFor(string name)
      {
         if (string.Equals(name, Constants.ObservedData.ORGAN))
            return predefinedValuesForCategory(addPredefinedOrganValues);

         if (string.Equals(name, Constants.ObservedData.COMPARTMENT))
            return predefinedValuesForCategory(addPredefinedCompartmentValues);

         if (string.Equals(name, Constants.ObservedData.MOLECULE))
            return predefinedValuesForCategory(addPredefinedMoleculeNames);

         return Enumerable.Empty<string>();
      }

      private IEnumerable<string> predefinedValuesForCategory(Action<DimensionInfo.MetaDataCategory> action)
      {
         var category = new DimensionInfo.MetaDataCategory();
         action(category);
         return category.ListOfValues.Values;
      }

      public IReadOnlyList<string> DefaultMetaDataCategories => new[]
      {
         Constants.ObservedData.MOLECULE, Constants.ObservedData.COMPARTMENT, Constants.ObservedData.ORGAN
      };

      public IReadOnlyList<string> ReadOnlyMetaDataCategories => new List<string>();

      public bool MolWeightEditable => true;

      public bool MolWeightVisible => true;
   }
}