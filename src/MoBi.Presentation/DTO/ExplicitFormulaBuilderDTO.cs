using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class ExplicitFormulaBuilderDTO : FormulaBuilderDTO
   {
      public ExplicitFormulaBuilderDTO(ExplicitFormula explicitFormula) : base(explicitFormula)
      {
      }
   }
}