using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using static OSPSuite.Core.Domain.Constants;

namespace MoBi.Presentation.Tasks
{
   public interface ITableFormulaTask
   {
      /// <summary>
      ///    Returns a command that can be run to set the X axis display unit of <paramref name="tableFormula" />to
      ///    <paramref name="newUnit" />
      /// </summary>
      /// <returns>The un-run command</returns>
      IMoBiCommand SetXUnit(TableFormula tableFormula, Unit newUnit, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Returns a command that can be run to set the Y axis display unit of <paramref name="tableFormula" />to
      ///    <paramref name="newUnit" />
      /// </summary>
      /// <returns>The un-run command</returns>
      IMoBiCommand SetYUnit(TableFormula tableFormula, Unit newUnit, IBuildingBlock buildingBlock);

      IMoBiCommand SetXValuePoint(TableFormula tableFormula, ValuePoint valuePoint, double newValueInDisplayUnit, IBuildingBlock buildingBlock);
      IMoBiCommand SetYValuePoint(TableFormula tableFormula, ValuePoint valuePoint, double newValueInDisplayUnit, IBuildingBlock buildingBlock);
      IMoBiCommand SetRestartSolver(TableFormula formula, ValuePoint valuePoint, bool newRestartSolverValue, IBuildingBlock buildingBlock);
      TableFormula ImportTableFormula();
   }

   public class TableFormulaTask : ITableFormulaTask
   {
      private readonly IDataImporter _dataImporter;
      private readonly IMoBiContext _context;
      private readonly IDialogCreator _dialogCreator;

      public TableFormulaTask(IDataImporter dataImporter, IMoBiContext context, IDialogCreator dialogCreator)
      {
         _dataImporter = dataImporter;
         _context = context;
         _dialogCreator = dialogCreator;
      }

      private IMoBiCommand setXValuePoint(ValuePoint valuePoint, TableFormula tableFormula,
         double newValueInBaseUnit, IBuildingBlock buildingBlock)
      {
         return new SetValuePointXValueCommand(tableFormula, valuePoint, newValueInBaseUnit, buildingBlock);
      }

      private IMoBiCommand setYValuePoint(ValuePoint valuePoint, TableFormula tableFormula,
         double newValueInBaseUnit, IBuildingBlock buildingBlock)
      {
         return new SetValuePointYValueCommand(tableFormula, valuePoint, newValueInBaseUnit, buildingBlock);
      }

      public IMoBiCommand SetXUnit(TableFormula tableFormula, Unit newUnit, IBuildingBlock buildingBlock)
      {
         return new SetTableFormulaXDisplayUnitCommand(newUnit, tableFormula.XDisplayUnit, tableFormula, buildingBlock);
      }

      public IMoBiCommand SetXValuePoint(TableFormula tableFormula, ValuePoint valuePoint, double newValueInDisplayUnit, IBuildingBlock buildingBlock)
      {
         var newValueInBaseUnit = tableFormula.XBaseValueFor(newValueInDisplayUnit);
         return setXValuePoint(valuePoint, tableFormula, newValueInBaseUnit, buildingBlock);
      }

      public IMoBiCommand SetYValuePoint(TableFormula tableFormula, ValuePoint valuePoint, double newValueInDisplayUnit, IBuildingBlock buildingBlock)
      {
         var newValueInBaseUnit = tableFormula.YBaseValueFor(newValueInDisplayUnit);
         return setYValuePoint(valuePoint, tableFormula, newValueInBaseUnit, buildingBlock);
      }

      public IMoBiCommand SetYUnit(TableFormula tableFormula, Unit newUnit, IBuildingBlock buildingBlock)
      {
         return new SetTableFormulaYDisplayUnitCommand(newUnit, tableFormula.YDisplayUnit, tableFormula, buildingBlock);
      }

      public IMoBiCommand SetRestartSolver(TableFormula formula, ValuePoint valuePoint, bool newRestartSolverValue, IBuildingBlock buildingBlock)
      {
         return new SetRestartSolverInValuePointCommand(formula, valuePoint, newRestartSolverValue, buildingBlock);
      }

      public TableFormula ImportTableFormula()
      {
         var dataImporterSettings = new DataImporterSettings
         {
            Caption = $"{AppConstants.PRODUCT_NAME_WITH_TRADEMARK} - {"Import Table Formula"}",
            IconName = ApplicationIcons.Formula.IconName
         };
         dataImporterSettings.AddNamingPatternMetaData("File");

         var importedFormula = _dataImporter.ImportDataSets(
            new List<MetaDataCategory>(),
            _dataImporter.ColumnInfosForObservedData(),
            dataImporterSettings,
            _dialogCreator.AskForFileToOpen(Captions.Importer.OpenFile, Captions.Importer.ImportFileFilter, DirectoryKey.OBSERVED_DATA)
         ).DataRepositories.FirstOrDefault();
         return importedFormula == null ? null : formulaFrom(importedFormula);
      }

      private TableFormula formulaFrom(DataRepository dataRepository)
      {
         var baseGrid = dataRepository.BaseGrid;
         var valueColumn = dataRepository.AllButBaseGrid().Single();
         var formula = newTableFormula();

         formula.XDisplayUnit = baseGrid.Dimension.Unit(baseGrid.DataInfo.DisplayUnitName);
         formula.YDisplayUnit = valueColumn.Dimension.Unit(valueColumn.DataInfo.DisplayUnitName);

         foreach (var timeValue in baseGrid.Values)
         {
            formula.AddPoint(timeValue, valueColumn.GetValue(timeValue).ToDouble());
         }

         return formula;
      }

      private TableFormula newTableFormula()
      {
         var tableFormula = _context.Create<TableFormula>();
         tableFormula.XDimension = _context.DimensionFactory.Dimension(TIME);
         tableFormula.XName = TIME;
         tableFormula.Dimension = _context.DimensionFactory.Dimension(AppConstants.DimensionNames.FRACTION);

         return tableFormula;
      }
   }
}