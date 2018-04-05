using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Data;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility.Extensions;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace MoBi.Core.Reporting.TEXBuilder
{
   public class DataRepositoryTEXBuilder : OSPSuiteTeXBuilder<DataRepository>
   {
      private readonly ITeXBuilderRepository _builderRepository;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly ReportingHelper _reportingHelper;

      public DataRepositoryTEXBuilder(ITeXBuilderRepository builderRepository, IDimensionFactory dimensionFactory, IDisplayUnitRetriever displayUnitRetriever)
      {
         _builderRepository = builderRepository;
         _dimensionFactory = dimensionFactory;
         _reportingHelper = new ReportingHelper(displayUnitRetriever);
      }

      private IEnumerable<DataTable> tablesFor(DataRepository dataRepository)
      {
         var tableList = new List<DataTable>();

         foreach (var col in dataRepository.AllButBaseGrid())
         {
            if (col.DataInfo.AuxiliaryType != AuxiliaryType.Undefined) continue;

            var table = new DataTable(col.Name);
            table.AddColumn<float>(col.BaseGrid.Name).SetUnit(col.BaseGrid.DataInfo.DisplayUnitName);

            var column = table.AddColumn<float>(col.Name);
            column.SetUnit(col.DataInfo.DisplayUnitName);

            addMolWeight(col, column);

            var geometricMeanPopError = createRelatedColumnFor(table, col, AuxiliaryType.GeometricMeanPop, Constants.GEOMETRIC_MEAN_POP);
            var arithmeticMeanPopError = createRelatedColumnFor(table, col, AuxiliaryType.ArithmeticMeanPop, Constants.ARITHMETRIC_MEAN_POP);
            var geometricStdDevError = createRelatedColumnFor(table, col, AuxiliaryType.GeometricStdDev, Constants.GEOMETRIC_STD_DEV);
            var arithmeticStdDevError = createRelatedColumnFor(table, col, AuxiliaryType.ArithmeticStdDev, Constants.ARITHMETRIC_STD_DEV);

            var dimension = _dimensionFactory.MergedDimensionFor(col);
            var baseGridUnit = _reportingHelper.GetDisplayUnitFor(col.BaseGrid.Dimension.Unit(col.BaseGrid.DataInfo.DisplayUnitName), col.BaseGrid.Dimension);
            var colUnit = _reportingHelper.GetDisplayUnitFor(dimension.Unit(col.DataInfo.DisplayUnitName), dimension);

            table.BeginLoadData();
            for (var i = 0; i < col.BaseGrid.InternalValues.Count(); i++)
            {
               var newRow = table.NewRow();
               newRow[col.BaseGrid.Name] = _reportingHelper.ConvertToDisplayUnit(baseGridUnit, col.BaseGrid.Dimension, col.BaseGrid.Values[i]);
               newRow[col.Name] = _reportingHelper.ConvertToDisplayUnit(colUnit, col.Dimension, col.Values[i]);
               updateRelateColumnValue(newRow, geometricMeanPopError, i);
               updateRelateColumnValue(newRow, arithmeticMeanPopError, i);
               updateRelateColumnValue(newRow, geometricStdDevError, i);
               updateRelateColumnValue(newRow, arithmeticStdDevError, i);

               table.Rows.Add(newRow);
            }

            table.EndLoadData();
            table.AcceptChanges();
            tableList.Add(table);
         }

         return tableList;
      }

      private DataColumn createRelatedColumnFor(DataTable dataTable, DataColumn dataColumn, AuxiliaryType auxiliaryType, string caption)
      {
         if (!dataColumn.ContainsRelatedColumn(auxiliaryType))
            return null;

         var realatedColumn = dataColumn.GetRelatedColumn(AuxiliaryType.ArithmeticStdDev);

         if (realatedColumn == null)
            return null;

         var errCol = dataTable.AddColumn<float>(realatedColumn.Name);
         errCol.SetUnit(realatedColumn.DataInfo.DisplayUnitName);
         errCol.SetNotes(new[] {caption});
         return realatedColumn;
      }

      private void updateRelateColumnValue(DataRow row, DataColumn dataColumn, int index)
      {
         if (dataColumn == null)
            return;

         var dimension = _dimensionFactory.MergedDimensionFor(dataColumn);
         row[dataColumn.Name] = _reportingHelper.ConvertToDisplayUnit(dimension.Unit(dataColumn.DataInfo.DisplayUnitName), dimension, dataColumn.Values[index]);
      }

      private void addMolWeight(DataColumn col, System.Data.DataColumn column)
      {
         if (col.DataInfo.MolWeight == null) return;

         var dimension = _dimensionFactory.Dimension(AppConstants.Parameters.MOLECULAR_WEIGHT);
         var displayUnit = _reportingHelper.GetDisplayUnitFor(dimension);
         column.SetNotes(new[]
         {
            string.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.MOLECULE_WEIGHT,
               $"{_reportingHelper.ConvertToDisplayUnit(displayUnit, dimension, (float) col.DataInfo.MolWeight)} {_reportingHelper.GetDisplayUnitFor(displayUnit, dimension)}")
         });
      }

      public override void Build(DataRepository dataRepository, OSPSuiteTracker tracker)
      {
         _builderRepository.Report(new Section(dataRepository.Name), tracker);

         foreach (var prop in dataRepository.ExtendedProperties)
         {
            _builderRepository.Report(string.Format(Constants.PROPERTY_PROMPT_FORMAT, prop.Name, prop.ValueAsObject),
               tracker);
         }

         foreach (var table in tablesFor(dataRepository))
         {
            _builderRepository.Report(new SubSection(table.TableName), tracker);
            _builderRepository.Report(table, tracker);
         }
      }
   }
}