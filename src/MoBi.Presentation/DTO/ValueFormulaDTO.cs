using MoBi.Assets;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class ValueFormulaDTO
   {
      public string FormulaString { get; set; }
      public IFormula Formula { get; set; }

      public ValueFormulaDTO()
      {
      }

      public ValueFormulaDTO(IFormula formula)
      {
         Formula = formula;
         if(formula is ExplicitFormula explicitFormula)
            FormulaString = explicitFormula.FormulaString;

         else FormulaString = string.Empty;
      }

      public override string ToString()
      {
         if(string.IsNullOrEmpty(FormulaString))
            return $"{Formula.Name}";

         return Formula != null ? $"{Formula.Name} ({FormulaString})" : FormulaString;
      }
   }

   public class EmptyFormulaDTO : ValueFormulaDTO
   {
      public EmptyFormulaDTO()
      {
         Formula = null;
         FormulaString = AppConstants.Captions.FormulaNotAvailable;
      }
   }
}