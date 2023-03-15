using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface ISumFormulaToDTOSumFormulaMapper : IMapper<SumFormula, SumFormulaDTO>
   {
   }

   internal class SumFormulaToDTOSumFormulaMapper : ObjectBaseToObjectBaseDTOMapperBase, ISumFormulaToDTOSumFormulaMapper
   {
      private readonly IDescriptorConditionToDescriptorConditionDTOMapper _descriptorConditionDTOMapper;

      public SumFormulaToDTOSumFormulaMapper(IDescriptorConditionToDescriptorConditionDTOMapper descriptorConditionDTOMapper)
      {
         _descriptorConditionDTOMapper = descriptorConditionDTOMapper;
      }

      public SumFormulaDTO MapFrom(SumFormula sumFormula)
      {
         var dto = Map<SumFormulaDTO>(new SumFormulaDTO(sumFormula));
         dto.Variable = sumFormula.Variable;
         dto.Dimension = sumFormula.Dimension;
         dto.VariablePattern = sumFormula.VariablePattern;
         dto.VariableCriteria = sumFormula.Criteria.MapAllUsing(_descriptorConditionDTOMapper);
         dto.FormulaString = sumFormula.FormulaString;
         return dto;
      }
   }
}