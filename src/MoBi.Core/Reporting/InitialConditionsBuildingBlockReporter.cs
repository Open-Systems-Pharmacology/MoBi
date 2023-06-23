using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.TeXReporting.Data;
using OSPSuite.TeXReporting.Items;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting
{
   internal class InitialConditionsBuildingBlockReporter : BuildingBlockReporter<InitialConditionsBuildingBlock, InitialCondition>
   {
      private readonly ReportingHelper _reportingHelper;

      public InitialConditionsBuildingBlockReporter(IDisplayUnitRetriever displayUnitRetriever)
         : base(Constants.INITIAL_CONDITIONS_BUILDING_BLOCK, Constants.INITIAL_CONDITIONS_BUILDING_BLOCKS)
      {
         _reportingHelper = new ReportingHelper(displayUnitRetriever);
      }

      protected override void AddBuildersReport(InitialConditionsBuildingBlock initialConditions, List<object> listToReport, OSPSuiteTracker buildTracker)
      {
         var table = tableFor(initialConditions, buildTracker.Settings.Verbose);
         var view = table.DefaultView;

         // grouped by molecule
         listToReport.Add(new SubSection(Constants.BY_MOLECULES));
         table.Columns[Constants.MOLECULE_NAME].SetHidden(true);
         table.Columns[Constants.CONTAINER_PATH].SetHidden(false);
         createTables(listToReport, table, view, Constants.MOLECULE_NAME, Constants.MOLECULE);

         // grouped by container
         listToReport.Add(new SubSection(Constants.BY_CONTAINERS));
         table.Columns[Constants.MOLECULE_NAME].SetHidden(false);
         table.Columns[Constants.CONTAINER_PATH].SetHidden(true); 
         createTables(listToReport, table, view, Constants.CONTAINER_PATH, Constants.CONTAINER);

         if (!initialConditions.FormulaCache.Any()) 
            return;
         listToReport.Add(new SubSection(Constants.FORMULA));
         listToReport.AddRange(initialConditions.FormulaCache.OrderBy(f => f.Name));
      }

      private static void createTables(List<Object> listToReport, DataTable table, DataView view, string columnName, string groupByName)
      {
         var entities = table.DefaultView.ToTable(true, columnName).DefaultView;

         entities.Sort = columnName;

         foreach (DataRowView dataRow in entities)
         {
            var value = dataRow[columnName].ToString();
            listToReport.Add(new SubSubSection(value));
            createRowFilter(view, columnName, value);

            view.CheckColumnVisibility(Constants.UNIT);
            view.CheckColumnVisibility(Constants.START_VALUE);
            view.CheckColumnVisibility(Constants.FORMULA);
            view.CheckColumnVisibility(Constants.SCALE_DIVISOR, 1D);

            var tableToReport = view.CreateDataTableWithPreservedChildRelations().DefaultView;
            tableToReport.Sort = Constants.MOLECULE_NAME;
            listToReport.Add(new Table(tableToReport, $"{Constants.INITIAL_CONDITIONS} for {groupByName} {value}"));
         }
      }

      private static void createRowFilter(DataView view, string columnName, string value)
      {
         view.RowFilter = $"[{columnName}] = '{value.Replace("'", "''")}'";
      }

      private DataTable tableFor(IEnumerable<InitialCondition> initialConditions, bool verbose)
      {
         var initialConditionTable = new DataTable(Constants.INITIAL_CONDITIONS);

         initialConditionTable.Columns.Add(Constants.MOLECULE_PATH, typeof (string)).AsHidden();
         initialConditionTable.Columns.Add(Constants.CONTAINER_PATH, typeof (string)).AsHidden();
         initialConditionTable.Columns.Add(Constants.MOLECULE_NAME, typeof (string));
         initialConditionTable.Columns.Add(Constants.START_VALUE, typeof(float));
         initialConditionTable.Columns.Add(Constants.FORMULA, typeof(string));
         initialConditionTable.Columns.Add(Constants.UNIT, typeof(string));
         initialConditionTable.Columns.Add(Constants.IS_PRESENT, typeof(string));
         initialConditionTable.Columns.Add(Constants.SCALE_DIVISOR, typeof(double));

         var descriptionTable = new DataTable();
         descriptionTable.Columns.Add(Constants.MOLECULE_PATH, typeof (string));
         descriptionTable.Columns.Add(Constants.DESCRIPTION, typeof (string));

         initialConditionTable.BeginLoadData();
         descriptionTable.BeginLoadData();
         foreach (var initialCondition in initialConditions)
         {
            var newInitialConditionRow = initialConditionTable.NewRow();
            newInitialConditionRow[Constants.MOLECULE_PATH] = initialCondition.Path;
            newInitialConditionRow[Constants.CONTAINER_PATH] = initialCondition.ContainerPath;
            newInitialConditionRow[Constants.MOLECULE_NAME] = initialCondition.MoleculeName;
            if (initialCondition.Value != null)
               newInitialConditionRow[Constants.START_VALUE] = _reportingHelper.ConvertToDisplayUnit(initialCondition, initialCondition.Value);
            newInitialConditionRow[Constants.UNIT] = _reportingHelper.GetDisplayUnitFor(initialCondition);
            newInitialConditionRow[Constants.IS_PRESENT] = initialCondition.IsPresent;
            newInitialConditionRow[Constants.SCALE_DIVISOR] = initialCondition.ScaleDivisor;
            if (initialCondition.Formula != null)
               newInitialConditionRow[Constants.FORMULA] = initialCondition.Formula.Name;
            initialConditionTable.Rows.Add(newInitialConditionRow);

            if (String.IsNullOrEmpty(initialCondition.Description)) continue;
            var newDescriptionRow = descriptionTable.NewRow();
            newDescriptionRow[Constants.MOLECULE_PATH] = initialCondition.Path;
            newDescriptionRow[Constants.DESCRIPTION] = initialCondition.Description;
            descriptionTable.Rows.Add(newDescriptionRow);
         }
         descriptionTable.EndLoadData();
         initialConditionTable.EndLoadData();

         if (verbose)
         {
            var ds = new DataSet();
            ds.Tables.Add(initialConditionTable);
            ds.Tables.Add(descriptionTable);
            ds.Relations.Add(initialConditionTable.Columns[Constants.MOLECULE_PATH],
                             descriptionTable.Columns[Constants.MOLECULE_PATH]);
         }

         return initialConditionTable;
      }
   }
}