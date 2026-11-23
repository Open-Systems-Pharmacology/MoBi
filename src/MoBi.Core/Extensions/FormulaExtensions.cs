using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Extensions
{
   public static class FormulaExtensions
   {
      // Formulas sharing an origin id are serialized as a single formula, so a formula that was edited must not keep its origin
      public static void ResetOrigin(this IFormula formula)
      {
         if (formula is ExplicitFormula explicitFormula)
            explicitFormula.OriginId = string.Empty;
      }
   }
}
