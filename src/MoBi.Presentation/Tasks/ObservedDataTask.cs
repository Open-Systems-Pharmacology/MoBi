using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;
using ColumnInfo = OSPSuite.Core.Importer.ColumnInfo;
using Command = OSPSuite.Assets.Command;
using CoreConstants = OSPSuite.Core.Domain.Constants;
using DataImporterSettings = OSPSuite.Core.Importer.DataImporterSettings;
using DimensionInfo = OSPSuite.Core.Importer.DimensionInfo;
using IDataImporter = OSPSuite.Core.Importer.IDataImporter;
using MetaDataCategory = OSPSuite.Core.Importer.MetaDataCategory;
using NullValuesHandlingType = OSPSuite.Core.Importer.NullValuesHandlingType;

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
   }

   public class ObservedDataTask : OSPSuite.Core.Domain.Services.ObservedDataTask, IObservedDataTask
   {
      private readonly IDataImporter _dataImporter;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IMoBiContext _context;
      private readonly IInteractionTask _interactionTask;
      private readonly IDimension _molWeightDimension;
      private readonly IDialogCreator _mobiDialogCreator;

      public ObservedDataTask(IDataImporter dataImporter, IDimensionFactory dimensionFactory, IMoBiContext context,
         IDialogCreator dialogCreator, IInteractionTask interactionTask, IDataRepositoryTask dataRepositoryTask, IContainerTask containerTask, IObjectTypeResolver objectTypeResolver) : base(dialogCreator, context, dataRepositoryTask, containerTask, objectTypeResolver)
      {
         _dataImporter = dataImporter;
         _mobiDialogCreator = dialogCreator;
         _interactionTask = interactionTask;
         _dimensionFactory = dimensionFactory;
         _context = context;
         _molWeightDimension = dimensionFactory.GetDimension(AppConstants.DimensionNames.MOL_WEIGHT);
      }

      public void AddObservedDataToProject()
      {
         var data = _dataImporter.ImportDataSets(createMetaData().ToList(), createColumnInfos().ToList(), createDataImportSettings());
         foreach (var repository in data)
         {
            adjustMolWeight(repository);
            AddObservedDataToProject(repository);
            adjustRepositoryPaths(repository);
         }
      }

      private void adjustRepositoryPaths(DataRepository repository)
      {
         var baseGrid = repository.BaseGrid;
         var baseGridName = baseGrid.Name.Replace(ObjectPath.PATH_DELIMITER, "\\");
         baseGrid.QuantityInfo = new QuantityInfo(baseGrid.Name, new[] {repository.Name, baseGridName}, QuantityType.Time);

         foreach (var col in repository.AllButBaseGrid())
         {
            var colName = col.Name.Replace(ObjectPath.PATH_DELIMITER, "\\");
            var quantityInfo = new QuantityInfo(col.Name, new[] {repository.Name, colName}, QuantityType.Undefined);
            col.QuantityInfo = quantityInfo;
         }
      }

      public void LoadObservedDataIntoProject()
      {
         string filename = _interactionTask.AskForFileToOpen(AppConstants.Dialog.Load(ObjectTypes.ObservedData), CoreConstants.Filter.PKML_FILE_FILTER, CoreConstants.DirectoryKey.MODEL_PART);
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
         var macoCommand = new MoBiMacroCommand
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = AppConstants.Commands.DeleteResultsFromSimulation(simulation.Name),
         };

         if (simulation.Results != null)
            macoCommand.AddCommand(new ClearResultsCommand(simulation));

         simulation.HistoricResults.Each(x => macoCommand.Add(new RemoveHistoricResultFromSimulationCommand(simulation, x)));
         return macoCommand;
      }

      private DataImporterSettings createDataImportSettings()
      {
         var settings = new DataImporterSettings
         {
            Icon = ApplicationIcons.MoBi,
            Caption = $"{AppConstants.ProductName} - {AppConstants.Captions.ImportObservedData}"
         };
         settings.AddNamingPatternMetaData(Constants.FILE);
         return settings;
      }

      private void adjustMolWeight(DataRepository observedData)
      {
         if (!observedData.ExtendedProperties.Contains(AppConstants.MolWeight))
            return;

         // molweight is provided in default unit should be saved in core unit
         var molWeightExtendedProperty = observedData.ExtendedProperties[AppConstants.MolWeight].DowncastTo<IExtendedProperty<double>>();
         var molWeight = _molWeightDimension.UnitValueToBaseUnitValue(_molWeightDimension.DefaultUnit, molWeightExtendedProperty.Value);
         observedData.AllButBaseGrid().Each(x => x.DataInfo.MolWeight = molWeight);

         //Remove Molweight extended properties
         observedData.ExtendedProperties.Remove(AppConstants.MolWeight);
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

         resultsToRemove.Each(result =>
         {
            macroCommand.Add(removeResultFromSimulationCommand(result));
         });

         _context.AddToHistory(macroCommand.Run(_context));
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

      private static RemoveHistoricResultFromSimulationCommand removeHistoricResultFromSimulationCommand(DataRepository repository, IMoBiSimulation parentSimulation)
      {
         return new RemoveHistoricResultFromSimulationCommand(parentSimulation, repository);
      }

      private IEnumerable<ColumnInfo> createColumnInfos()
      {
         var timeDimension = _dimensionFactory.GetDimension(CoreConstants.Dimension.TIME);
         var timeColumn = new ColumnInfo
         {
            DefaultDimension = timeDimension,
            Name = "Time",
            Description = "Time",
            DisplayName = "Time",
            IsMandatory = true,
            NullValuesHandling = NullValuesHandlingType.DeleteRow,
         };

         timeColumn.DimensionInfos.Add(new DimensionInfo {Dimension = timeDimension, IsMainDimension = true});
         yield return timeColumn;

         var mainDimension = _dimensionFactory.GetDimension(CoreConstants.Dimension.MOLAR_CONCENTRATION);
         var measurementInfo = new ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = "Measurement",
            Description = "Measurement",
            DisplayName = "Measurement",
            IsMandatory = true,
            NullValuesHandling = NullValuesHandlingType.DeleteRow,
            BaseGridName = timeColumn.Name
         };

         addDimensionsTo(measurementInfo, mainDimension);
         yield return measurementInfo;

         var errorInfo = new ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = "Error",
            Description = "Error",
            DisplayName = "Error",
            IsMandatory = false,
            NullValuesHandling = NullValuesHandlingType.Allowed,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = measurementInfo.Name
         };

         addDimensionsTo(errorInfo, mainDimension);
         yield return errorInfo;
      }

      private void addDimensionsTo(ColumnInfo columnInfo, IDimension mainDimension)
      {
         var timeDimension = _dimensionFactory.GetDimension(CoreConstants.Dimension.TIME);

         foreach (var dimension in _dimensionFactory.Dimensions.Where(x => x != timeDimension))
         {
            columnInfo.DimensionInfos.Add(new DimensionInfo
            {
               Dimension = dimension,
               IsMainDimension = (dimension == mainDimension)
            });
         }
      }

      private IEnumerable<MetaDataCategory> createMetaData()
      {
         yield return new MetaDataCategory
         {
            Name = AppConstants.MolWeight,
            DisplayName = $"{AppConstants.Parameters.MOLECULAR_WEIGHT} [{_molWeightDimension.DefaultUnit}]",
            Description = AppConstants.Parameters.MOLECULAR_WEIGHT,
            MetaDataType = typeof(double),
            IsMandatory = false,
            MinValue = 0,
            MinValueAllowed = false
         };

         yield return createMetaDataCategory<string>(ObservedData.ORGAN,
            isMandatory: false,
            isListOfValuesFixed: true,
            fixedValuesRetriever: addPredefinedOrganValues,
            description: ObservedData.ObservedDataOrganDescription);

         yield return createMetaDataCategory<string>(ObservedData.COMPARTMENT,
            isMandatory: false,
            isListOfValuesFixed: true,
            fixedValuesRetriever: addPredefinedCompartmentValues,
            description: ObservedData.ObservedDataCompartmentDescription);

         yield return createMetaDataCategory<string>(ObservedData.MOLECULE,
            isMandatory: false, isListOfValuesFixed: true,
            fixedValuesRetriever: addPredefinedMoleculeNames,
            description: ObservedData.ObservedDataMoleculeDescription);
      }

      private void addPredefinedMoleculeNames(MetaDataCategory metaDataCategory)
      {
         addUndefinedValueTo(metaDataCategory);
         allMolecules().OrderBy(molecule => molecule.Name).Each(molecule => addInfoToCategory(metaDataCategory, molecule));
      }

      private IEnumerable<IContainer> allMolecules()
      {
         return _context.CurrentProject.MoleculeBlockCollection.SelectMany(buildingBlock =>
         {
            return buildingBlock.Select(builder => builder);
         }).DistinctBy(builder => builder.Name);
      }

      private static void addUndefinedValueTo(MetaDataCategory metaDataCategory)
      {
         metaDataCategory.ListOfValues.Add(AppConstants.Undefined, AppConstants.Undefined);
      }

      private void addPredefinedOrganValues(MetaDataCategory metaDataCategory)
      {
         addUndefinedValueTo(metaDataCategory);
         allOrgans().OrderBy(org => org.Name).Each(organ => addInfoToCategory(metaDataCategory, organ));
      }

      private void addPredefinedCompartmentValues(MetaDataCategory metaDataCategory)
      {
         addUndefinedValueTo(metaDataCategory);
         allCompartments().OrderBy(comp => comp.Name).Each(compartment => addInfoToCategory(metaDataCategory, compartment));
      }

      private IEnumerable<IContainer> allOrgans()
      {
         return allTopContainers().SelectMany(allSubContainers).Where(container => container.ContainerType == ContainerType.Organ).DistinctBy(x => x.Name);
      }

      private IEnumerable<IContainer> allCompartments()
      {
         return allTopContainers().SelectMany(allSubContainers).Where(container => container.ContainerType == ContainerType.Compartment).DistinctBy(x => x.Name);
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

      private void addInfoToCategory(MetaDataCategory metaDataCategory, IContainer container)
      {
         metaDataCategory.ListOfValues.Add(container.Name, container.Name);

         var icon = ApplicationIcons.IconByName(container.Icon);
         if (icon != ApplicationIcons.EmptyIcon)
            metaDataCategory.ListOfImages.Add(container.Name, icon);
      }

      private MetaDataCategory createMetaDataCategory<T>(string categoryName, bool isMandatory = false, bool isListOfValuesFixed = false, Action<MetaDataCategory> fixedValuesRetriever = null, string description = null)
      {
         var category = new MetaDataCategory
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
         if (string.Equals(name, ObservedData.ORGAN))
            return predefinedValuesForCategory(addPredefinedOrganValues);

         if (string.Equals(name, ObservedData.COMPARTMENT))
            return predefinedValuesForCategory(addPredefinedCompartmentValues);

         if (string.Equals(name, ObservedData.MOLECULE))
            return predefinedValuesForCategory(addPredefinedMoleculeNames);

         return Enumerable.Empty<string>();
      }

      private IEnumerable<string> predefinedValuesForCategory(Action<MetaDataCategory> action)
      {
         var category = new MetaDataCategory();
         action(category);
         return category.ListOfValues.Values;
      }

      public IReadOnlyList<string> DefaultMetaDataCategories => AppConstants.DefaultObservedDataCategories;

      public IReadOnlyList<string> ReadOnlyMetaDataCategories => new List<string>();

      public bool MolWeightEditable => true;

      public bool MolWeightVisible => true;
   }
}