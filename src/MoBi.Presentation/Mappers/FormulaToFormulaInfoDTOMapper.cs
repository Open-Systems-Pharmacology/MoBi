using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mappers
{
   public interface IFormulaToFormulaInfoDTOMapper : IMapper<IFormula, FormulaInfoDTO>
   {
   }

   internal class FormulaToFormulaInfoDTOMapper : IFormulaToFormulaInfoDTOMapper
   {
      public FormulaInfoDTO MapFrom(IFormula formula)
      {
         return new FormulaInfoDTO {Name = formula.Name, Type = formula.GetType()};
      }
   }
}