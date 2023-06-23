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
   internal class ParameterValuesBuildingBlockReporter : BuildingBlockReporter<ParameterValuesBuildingBlock, ParameterValue>
   {
      private readonly ReportingHelper _reportingHelper;

      public ParameterValuesBuildingBlockReporter(IDisplayUnitRetriever displayUnitRetriever) : base(Constants.PARAMETER_VALUES_BUILDING_BLOCK, Constants.PARAMETER_VALUES_BUILDING_BLOCKS)
      {
         _reportingHelper = new ReportingHelper(displayUnitRetriever);
      }

      protected override void AddBuildersReport(ParameterValuesBuildingBlock parameterValues, List<object> listToReport, OSPSuiteTracker buildTracker)
      {
         var table = tableFor(parameterValues, buildTracker.Settings.Verbose);
         var containers = table.DefaultView.ToTable(true, Constants.CONTAINER_PATH, Constants.LEVELS).DefaultView;
         containers.Sort = $"{Constants.CONTAINER_PATH}, {Constants.LEVELS}";

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
            listToReport.Add(new Table(tableToReport.DefaultView, String.Format("{0} for {1}", Constants.PARAMETER_VALUES, containerPath)));
         }

         if (!parameterValues.FormulaCache.Any())
            return;
         listToReport.Add(new SubSection(Constants.FORMULA));
         listToReport.AddRange(parameterValues.FormulaCache.OrderBy(f => f.Name));
      }

      private DataTable tableFor(IEnumerable<ParameterValue> parameterValues, bool verbose)
      {
         var parameterValueTable = new DataTable(Constants.PARAMETER_VALUES);

         parameterValueTable.Columns.Add(Constants.ID, typeof (string)).AsHidden();
         parameterValueTable.Columns.Add(Constants.PARAMETER, typeof (string));
         parameterValueTable.Columns.Add(Constants.CONTAINER_PATH, typeof (string)).AsHidden();
         parameterValueTable.Columns.Add(Constants.LEVELS, typeof (int)).AsHidden();
         parameterValueTable.Columns.Add(Constants.START_VALUE, typeof(double));
         parameterValueTable.Columns.Add(Constants.UNIT, typeof (string));
         parameterValueTable.Columns.Add(Constants.FORMULA, typeof(string));

         var descriptionTable = new DataTable();
         descriptionTable.Columns.Add(Constants.ID, typeof (string));
         descriptionTable.Columns.Add(Constants.DESCRIPTION, typeof (string));

         parameterValueTable.BeginLoadData();
         descriptionTable.BeginLoadData();
         foreach (var parameterValue in parameterValues)
         {
            var newParameterValueRow = parameterValueTable.NewRow();
            var levels = parameterValue.Path.PathAsString.Split('|');
            newParameterValueRow[Constants.ID] = parameterValue.Id;
            newParameterValueRow[Constants.PARAMETER] = levels[levels.Length - 1];
            newParameterValueRow[Constants.CONTAINER_PATH] = String.Join("|", levels.Take(levels.Length - 1));
            newParameterValueRow[Constants.LEVELS] = levels.Length;
            if (parameterValue.Value != null)
               newParameterValueRow[Constants.START_VALUE] = _reportingHelper.ConvertToDisplayUnit(parameterValue, parameterValue.Value);
            newParameterValueRow[Constants.UNIT] = _reportingHelper.GetDisplayUnitFor(parameterValue);
            newParameterValueRow[Constants.FORMULA] = (parameterValue.Formula == null) ? (object)DBNull.Value : parameterValue.Formula.Name;
            parameterValueTable.Rows.Add(newParameterValueRow);

            if (String.IsNullOrEmpty(parameterValue.Description)) continue;
            var newDescriptionRow = descriptionTable.NewRow();
            newDescriptionRow[Constants.ID] = parameterValue.Id;
            newDescriptionRow[Constants.DESCRIPTION] = parameterValue.Description;
            descriptionTable.Rows.Add(newDescriptionRow);
         }
         descriptionTable.EndLoadData();
         parameterValueTable.EndLoadData();

         if (verbose)
         {
            var ds = new DataSet();
            ds.Tables.Add(parameterValueTable);
            ds.Tables.Add(descriptionTable);
            ds.Relations.Add(parameterValueTable.Columns[Constants.ID], descriptionTable.Columns[Constants.ID]);
         }

         return parameterValueTable;
      }

   }
}