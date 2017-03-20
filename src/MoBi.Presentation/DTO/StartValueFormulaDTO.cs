using MoBi.Assets;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class StartValueFormulaDTO
   {
      public string FormulaString { get; set; }
      public IFormula Formula { get; set; }

      public StartValueFormulaDTO()
      {
      }

      public StartValueFormulaDTO(ExplicitFormula explicitFormula)
      {
         Formula = explicitFormula;
         FormulaString = explicitFormula.FormulaString;
      }

      public override string ToString()
      {
         return Formula != null ? string.Format("{0} ({1})",Formula.Name, FormulaString) : FormulaString;
      }
   }

   public class EmptyFormulaDTO : StartValueFormulaDTO
   {
      public EmptyFormulaDTO()
      {
         Formula = null;
         FormulaString = AppConstants.Captions.FormulaNotAvailable;
      }
   }
}