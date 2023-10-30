using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class TableFormulaTEXBuilder : OSPSuiteTeXBuilder<TableFormula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public TableFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(TableFormula tableFormula, OSPSuiteTracker buildTracker)
      {
         if (String.IsNullOrEmpty(tableFormula.Name)) return;

         var listToReport = new List<object>();
         listToReport.Add(new Paragraph(Constants.TABLE_FORMULA));
         listToReport.Add(string.Format(Constants.PROPERTY_PROMPT_FORMAT, tableFormula.Name, tableFormula.ToString()));
         listToReport.AddRange(this.ReportDescription(tableFormula, buildTracker));
         listToReport.Add(new Table(toDataTable(tableFormula.AllPoints).DefaultView, Constants.TABLE_POINTS));

         _builderRepository.Report(listToReport, buildTracker);
      }

      private DataTable toDataTable(IEnumerable<ValuePoint> data)
      {
         var dataTable = new DataTable();
         dataTable.Columns.Add(Constants.X, typeof(double));
         dataTable.Columns.Add(Constants.Y, typeof(double));
         dataTable.Columns.Add(Constants.RESTART_SOLVER, typeof (bool));

         dataTable.BeginLoadData();
         foreach (var valuePoint in data)
         {
            var newRow = dataTable.NewRow();
            newRow[Constants.X] = valuePoint.X;
            newRow[Constants.Y] = valuePoint.Y;
            newRow[Constants.RESTART_SOLVER] = valuePoint.RestartSolver;
            dataTable.Rows.Add(newRow);
         }
         dataTable.EndLoadData();

         return dataTable;
      }
   }
}
