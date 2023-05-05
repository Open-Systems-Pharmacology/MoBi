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
   internal class MoleculeStartValuesBuildingBlockReporter : BuildingBlockReporter<InitialConditionsBuildingBlock, InitialCondition>
   {
      private readonly ReportingHelper _reportingHelper;

      public MoleculeStartValuesBuildingBlockReporter(IDisplayUnitRetriever displayUnitRetriever)
         : base(Constants.MOLECULE_START_VALUES_BUILDING_BLOCK, Constants.MOLECULE_START_VALUES_BUILDING_BLOCKS)
      {
         _reportingHelper = new ReportingHelper(displayUnitRetriever);
      }

      protected override void AddBuildersReport(InitialConditionsBuildingBlock moleculeStartValues, List<object> listToReport, OSPSuiteTracker buildTracker)
      {
         var table = tableFor(moleculeStartValues, buildTracker.Settings.Verbose);
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

         if (!moleculeStartValues.FormulaCache.Any()) 
            return;
         listToReport.Add(new SubSection(Constants.FORMULA));
         listToReport.AddRange(moleculeStartValues.FormulaCache.OrderBy(f => f.Name));
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
            listToReport.Add(new Table(tableToReport, String.Format("{0} for {1} {2}", Constants.MOLECULE_START_VALUES, groupByName, value)));
         }
      }

      private static void createRowFilter(DataView view, string columnName, string value)
      {
         view.RowFilter = String.Format("[{0}] = '{1}'", columnName, value.Replace("'", "''"));
      }

      private DataTable tableFor(IEnumerable<InitialCondition> moleculeStartValues, bool verbose)
      {
         var moleculeStartValueTable = new DataTable(Constants.MOLECULE_START_VALUES);

         moleculeStartValueTable.Columns.Add(Constants.MOLECULE_PATH, typeof (string)).AsHidden();
         moleculeStartValueTable.Columns.Add(Constants.CONTAINER_PATH, typeof (string)).AsHidden();
         moleculeStartValueTable.Columns.Add(Constants.MOLECULE_NAME, typeof (string));
         moleculeStartValueTable.Columns.Add(Constants.START_VALUE, typeof(float));
         moleculeStartValueTable.Columns.Add(Constants.FORMULA, typeof(string));
         moleculeStartValueTable.Columns.Add(Constants.UNIT, typeof(string));
         moleculeStartValueTable.Columns.Add(Constants.IS_PRESENT, typeof(string));
         moleculeStartValueTable.Columns.Add(Constants.SCALE_DIVISOR, typeof(double));

         var descriptionTable = new DataTable();
         descriptionTable.Columns.Add(Constants.MOLECULE_PATH, typeof (string));
         descriptionTable.Columns.Add(Constants.DESCRIPTION, typeof (string));

         moleculeStartValueTable.BeginLoadData();
         descriptionTable.BeginLoadData();
         foreach (var moleculeStartValue in moleculeStartValues)
         {
            var newMoleculeStartValueRow = moleculeStartValueTable.NewRow();
            newMoleculeStartValueRow[Constants.MOLECULE_PATH] = moleculeStartValue.Path;
            newMoleculeStartValueRow[Constants.CONTAINER_PATH] = moleculeStartValue.ContainerPath;
            newMoleculeStartValueRow[Constants.MOLECULE_NAME] = moleculeStartValue.MoleculeName;
            if (moleculeStartValue.Value != null)
               newMoleculeStartValueRow[Constants.START_VALUE] = _reportingHelper.ConvertToDisplayUnit(moleculeStartValue, moleculeStartValue.Value);
            newMoleculeStartValueRow[Constants.UNIT] = _reportingHelper.GetDisplayUnitFor(moleculeStartValue);
            newMoleculeStartValueRow[Constants.IS_PRESENT] = moleculeStartValue.IsPresent;
            newMoleculeStartValueRow[Constants.SCALE_DIVISOR] = moleculeStartValue.ScaleDivisor;
            if (moleculeStartValue.Formula != null)
               newMoleculeStartValueRow[Constants.FORMULA] = moleculeStartValue.Formula.Name;
            moleculeStartValueTable.Rows.Add(newMoleculeStartValueRow);

            if (String.IsNullOrEmpty(moleculeStartValue.Description)) continue;
            var newDescriptionRow = descriptionTable.NewRow();
            newDescriptionRow[Constants.MOLECULE_PATH] = moleculeStartValue.Path;
            newDescriptionRow[Constants.DESCRIPTION] = moleculeStartValue.Description;
            descriptionTable.Rows.Add(newDescriptionRow);
         }
         descriptionTable.EndLoadData();
         moleculeStartValueTable.EndLoadData();

         if (verbose)
         {
            var ds = new DataSet();
            ds.Tables.Add(moleculeStartValueTable);
            ds.Tables.Add(descriptionTable);
            ds.Relations.Add(moleculeStartValueTable.Columns[Constants.MOLECULE_PATH],
                             descriptionTable.Columns[Constants.MOLECULE_PATH]);
         }

         return moleculeStartValueTable;
      }


   }
}