using System;
using System.Collections.Generic;
using OSPSuite.TeXReporting.Builder;
using MoBi.Core.Reporting.Items;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class ExplicitFormulaTEXBuilder : OSPSuiteTeXBuilder<ExplicitFormula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ExplicitFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ExplicitFormula explicitFormula, OSPSuiteTracker buildTracker)
      {
         if (String.IsNullOrEmpty(explicitFormula.Name)) return;

         var listToReport = new List<object>
                               {
                                  new FormulaTextBox(Constants.EXPLICIT_FORMULA, explicitFormula)
                               };

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}