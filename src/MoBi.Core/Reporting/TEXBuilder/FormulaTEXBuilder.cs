using System;
using System.Collections.Generic;
using OSPSuite.TeXReporting.Builder;
using MoBi.Core.Reporting.Items;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class FormulaTEXBuilder : OSPSuiteTeXBuilder<Formula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public FormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(Formula formula, OSPSuiteTracker buildTracker)
      {
         if (String.IsNullOrEmpty(formula.Name)) return;

         var listToReport = new List<object>
                               {
                                  new FormulaTextBox(Constants.FORMULA, formula)
                               };
         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}