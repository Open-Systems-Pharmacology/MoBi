using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mappers
{
   public interface ISumFormulaToDTOSumFormulaMapper : IMapper<SumFormula, SumFormulaDTO>
   {
   }

   internal class SumFormulaToDTOSumFormulaMapper : ObjectBaseToObjectBaseDTOMapperBase, ISumFormulaToDTOSumFormulaMapper
   {
      private readonly IDescriptorConditionToDescriptorConditionDTOMapper _descriptorConditionToDTODescriptorConditionMapper;

      public SumFormulaToDTOSumFormulaMapper(IDescriptorConditionToDescriptorConditionDTOMapper descriptorConditionToDTODescriptorConditionMapper)
      {
         _descriptorConditionToDTODescriptorConditionMapper = descriptorConditionToDTODescriptorConditionMapper;
      }

      public SumFormulaDTO MapFrom(SumFormula sumFormula)
      {
         var dto = Map<SumFormulaDTO>(sumFormula);
         dto.Variable = sumFormula.Variable;
         dto.VariablePattern = sumFormula.VariablePattern;
         dto.VariableCriteria = sumFormula.Criteria.MapAllUsing(_descriptorConditionToDTODescriptorConditionMapper);
         return dto;
      }
   }
}