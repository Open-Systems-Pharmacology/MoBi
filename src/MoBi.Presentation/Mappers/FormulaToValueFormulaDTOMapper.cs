using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IFormulaToValueFormulaDTOMapper : IMapper<IFormula, ValueFormulaDTO>
   {

   }

   public class FormulaToValueFormulaDTOMapper : IFormulaToValueFormulaDTOMapper
   {
      public ValueFormulaDTO MapFrom(IFormula formula)
      {
         return formula is ExplicitFormula explicitFormula ? new ValueFormulaDTO(explicitFormula) : emptyFormulaFor(formula);
      }

      private static EmptyFormulaDTO emptyFormulaFor(IFormula formula)
      {
         return formula == null ? new EmptyFormulaDTO() : new EmptyFormulaDTO { FormulaString = formula.Name };
      }
   }
}