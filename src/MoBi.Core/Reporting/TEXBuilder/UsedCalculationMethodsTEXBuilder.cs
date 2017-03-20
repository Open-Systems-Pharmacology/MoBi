using System.Collections.Generic;
using System.Data;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class UsedCalculationMethodsTEXBuilder : OSPSuiteTeXBuilder<IEnumerable<UsedCalculationMethod>>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public UsedCalculationMethodsTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IEnumerable<UsedCalculationMethod> objectToReport, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();

         var dataTable = new DataTable(Constants.CALCULATION_METHODS);
         dataTable.Columns.Add(Constants.CATEGORY, typeof (string));
         dataTable.Columns.Add(Constants.CALCULATION_METHOD, typeof (string));

         dataTable.BeginLoadData();
         foreach (var usedCalculationMethod in objectToReport)
         {
            var newRow = dataTable.NewRow();
            newRow[Constants.CATEGORY] = usedCalculationMethod.Category;
            newRow[Constants.CALCULATION_METHOD] = usedCalculationMethod.CalculationMethod;
            dataTable.Rows.Add(newRow);
         }
         dataTable.EndLoadData();

         listToReport.Add(dataTable);

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}