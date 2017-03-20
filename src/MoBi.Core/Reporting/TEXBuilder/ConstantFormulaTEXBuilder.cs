using System.Globalization;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class ConstantFormulaTEXBuilder : OSPSuiteTeXBuilder<ConstantFormula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ConstantFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ConstantFormula constantFormula, OSPSuiteTracker buildTracker)
      {
         var text = $"{Constants.VALUE} = {constantFormula.Value.ToString(CultureInfo.InvariantCulture)}";
         _builderRepository.Report(text, buildTracker);
      }
   }
}