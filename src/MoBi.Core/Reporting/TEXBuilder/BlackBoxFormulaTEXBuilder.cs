using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX.Converter;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class BlackBoxFormulaTEXBuilder : OSPSuiteTeXBuilder<BlackBoxFormula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public BlackBoxFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(BlackBoxFormula blackBoxFormula, OSPSuiteTracker tracker)
      {
         if (String.IsNullOrEmpty(blackBoxFormula.Name)) return;

         var listToReport = new List<object>();
         listToReport.AddRange(this.ReportDescription(blackBoxFormula, tracker));


         var formula = new Text(blackBoxFormula.ToString()) {Converter = FormulaConverter.Instance};
         formula = new Text("{0} = {1}", blackBoxFormula.Name, formula) {Alignment = Text.Alignments.flushleft};
         var content = new Text("{0}{1}", formula, blackBoxFormula.ObjectPaths);

         listToReport.Add(new TextBox(Constants.BLACK_BOX_FORMULA, content));

         _builderRepository.Report(listToReport, tracker);
      }
   }
}