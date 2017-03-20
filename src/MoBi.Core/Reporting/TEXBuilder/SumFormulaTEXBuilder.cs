using System;
using System.Collections.Generic;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX.Converter;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class SumFormulaTEXBuilder : OSPSuiteTeXBuilder<SumFormula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public SumFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(SumFormula sumFormula, OSPSuiteTracker buildTracker)
      {
         if (String.IsNullOrEmpty(sumFormula.Name)) return;

         var listToReport = new List<object>();
         listToReport.AddRange(this.ReportDescription(sumFormula, buildTracker));

         var formula = new Text("${0}$",
                                new Text(OSPSuite.TeXReporting.TeX.EquationWriter.Sum(String.Empty, String.Empty,
                                                                                 DefaultConverter.Instance.StringToTeX(sumFormula.Variable)))
                                   {Converter = NoConverter.Instance})
                          {Converter = NoConverter.Instance};
         var content = new Text("{0} = {1}{2}{3}{4}",
                                sumFormula.Name, formula, new LineBreak(), Constants.OVER_ALL_PARAMETERS_WITH, sumFormula.Criteria);
         listToReport.Add(new TextBox(Constants.SUM_FORMULA, content));

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}