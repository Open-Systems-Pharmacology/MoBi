using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Extensions
{
   public static class FormulaExtensions
   {
      public static void ResetOrigin(this IFormula formula)
      {
         if (formula is ExplicitFormula explicitFormula)
            explicitFormula.OriginId = string.Empty;
      }
   }
}
