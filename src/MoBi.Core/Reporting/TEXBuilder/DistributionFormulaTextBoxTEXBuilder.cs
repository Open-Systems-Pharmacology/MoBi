using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Reporting.Items;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class DistributionFormulaTextBoxTEXBuilder : OSPSuiteTeXBuilder<DistributionFormulaTextBox>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public DistributionFormulaTextBoxTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(DistributionFormulaTextBox distributionFormulaTextBox, OSPSuiteTracker tracker)
      {
         var formula = distributionFormulaTextBox.Formula;

         if (!formula.ObjectPaths.Any()) return;

         var listToReport = new List<object>();
         listToReport.AddRange(this.ReportDescription(formula, tracker));

         var content = new Text("{0}", formula.ObjectPaths);
         listToReport.Add(new TextBox(distributionFormulaTextBox.Caption, content));

         _builderRepository.Report(listToReport, tracker);
      }
   }
}
