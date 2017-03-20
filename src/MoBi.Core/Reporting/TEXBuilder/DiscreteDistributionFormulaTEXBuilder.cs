using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Reporting.Items;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class DiscreteDistributionFormulaTEXBuilder : OSPSuiteTeXBuilder<DiscreteDistributionFormula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public DiscreteDistributionFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(DiscreteDistributionFormula discreteDistributionFormula, OSPSuiteTracker tracker)
      {
         if (!discreteDistributionFormula.ObjectPaths.Any()) return;

         var listToReport = new List<object>
                               {
                                  new DistributionFormulaTextBox(Constants.DISCRETE_DISTRIBUTION_FORMULA, discreteDistributionFormula)
                               };

         _builderRepository.Report(listToReport, tracker);
      }
   }
}