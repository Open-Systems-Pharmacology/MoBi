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
   internal class ParameterStartValuesBuildingBlockReporter : BuildingBlockReporter<ParameterStartValuesBuildingBlock, ParameterStartValue>
   {
      private readonly ReportingHelper _reportingHelper;

      public ParameterStartValuesBuildingBlockReporter(IDisplayUnitRetriever displayUnitRetriever) : base(Constants.PARAMETER_START_VALUES_BUILDING_BLOCK, Constants.PARAMETER_START_VALUES_BUILDING_BLOCKS)
      {
         _reportingHelper = new ReportingHelper(displayUnitRetriever);
      }

      protected override void AddBuildersReport(ParameterStartValuesBuildingBlock parameterStartValues, List<object> listToReport, OSPSuiteTracker buildTracker)
      {
         var table = tableFor(parameterStartValues, buildTracker.Settings.Verbose);
         var containers = table.DefaultView.ToTable(true, Constants.CONTAINER_PATH, Constants.LEVELS).DefaultView;
         containers.Sort = String.Format("{0}, {1}", Constants.CONTAINER_PATH, Constants.LEVELS);

         var view = table.DefaultView;
         foreach (DataRowView containerRow in containers)
         {
            var containerPath = containerRow[Constants.CONTAINER_PATH].ToString();
            listToReport.Add(new SubSection(containerPath));
            view.RowFilter = String.Format("[{0}] = '{1}'", Constants.CONTAINER_PATH, containerPath.Replace("'", "''"));
            view.Sort = Constants.PARAMETER;

            view.CheckColumnVisibility(Constants.FORMULA);
            view.CheckColumnVisibility(Constants.UNIT);

            var tableToReport = view.CreateDataTableWithPreservedChildRelations();
            listToReport.Add(new Table(tableToReport.DefaultView, String.Format("{0} for {1}", Constants.PARAMETER_START_VALUES, containerPath)));
         }

         if (!parameterStartValues.FormulaCache.Any())
            return;
         listToReport.Add(new SubSection(Constants.FORMULA));
         listToReport.AddRange(parameterStartValues.FormulaCache.OrderBy(f => f.Name));
      }

      private DataTable tableFor(IEnumerable<ParameterStartValue> parameterStartValues, bool verbose)
      {
         var parameterStartValueTable = new DataTable(Constants.PARAMETER_START_VALUES);

         parameterStartValueTable.Columns.Add(Constants.ID, typeof (string)).AsHidden();
         parameterStartValueTable.Columns.Add(Constants.PARAMETER, typeof (string));
         parameterStartValueTable.Columns.Add(Constants.CONTAINER_PATH, typeof (string)).AsHidden();
         parameterStartValueTable.Columns.Add(Constants.LEVELS, typeof (int)).AsHidden();
         parameterStartValueTable.Columns.Add(Constants.START_VALUE, typeof(double));
         parameterStartValueTable.Columns.Add(Constants.UNIT, typeof (string));
         parameterStartValueTable.Columns.Add(Constants.FORMULA, typeof(string));

         var descriptionTable = new DataTable();
         descriptionTable.Columns.Add(Constants.ID, typeof (string));
         descriptionTable.Columns.Add(Constants.DESCRIPTION, typeof (string));

         parameterStartValueTable.BeginLoadData();
         descriptionTable.BeginLoadData();
         foreach (var parameterStartValue in parameterStartValues)
         {
            var newParameterStartValueRow = parameterStartValueTable.NewRow();
            var levels = parameterStartValue.Path.PathAsString.Split('|');
            newParameterStartValueRow[Constants.ID] = parameterStartValue.Id;
            newParameterStartValueRow[Constants.PARAMETER] = levels[levels.Length - 1];
            newParameterStartValueRow[Constants.CONTAINER_PATH] = String.Join("|", levels.Take(levels.Length - 1));
            newParameterStartValueRow[Constants.LEVELS] = levels.Length;
            if (parameterStartValue.Value != null)
               newParameterStartValueRow[Constants.START_VALUE] = _reportingHelper.ConvertToDisplayUnit(parameterStartValue, parameterStartValue.Value);
            newParameterStartValueRow[Constants.UNIT] = _reportingHelper.GetDisplayUnitFor(parameterStartValue);
            newParameterStartValueRow[Constants.FORMULA] = (parameterStartValue.Formula == null) ? (object)DBNull.Value : parameterStartValue.Formula.Name;
            parameterStartValueTable.Rows.Add(newParameterStartValueRow);

            if (String.IsNullOrEmpty(parameterStartValue.Description)) continue;
            var newDescriptionRow = descriptionTable.NewRow();
            newDescriptionRow[Constants.ID] = parameterStartValue.Id;
            newDescriptionRow[Constants.DESCRIPTION] = parameterStartValue.Description;
            descriptionTable.Rows.Add(newDescriptionRow);
         }
         descriptionTable.EndLoadData();
         parameterStartValueTable.EndLoadData();

         if (verbose)
         {
            var ds = new DataSet();
            ds.Tables.Add(parameterStartValueTable);
            ds.Tables.Add(descriptionTable);
            ds.Relations.Add(parameterStartValueTable.Columns[Constants.ID], descriptionTable.Columns[Constants.ID]);
         }

         return parameterStartValueTable;
      }

   }
}