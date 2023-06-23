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

      public ValueFormulaDTO(ExplicitFormula explicitFormula)
      {
         Formula = explicitFormula;
         FormulaString = explicitFormula.FormulaString;
      }

      public override string ToString()
      {
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