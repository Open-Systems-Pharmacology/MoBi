using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
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
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathDTOMapper;

      public SumFormulaToDTOSumFormulaMapper(
         IDescriptorConditionToDescriptorConditionDTOMapper descriptorConditionDTOMapper,
         IFormulaUsablePathToFormulaUsablePathDTOMapper formulaUsablePathDTOMapper)
      {
         _descriptorConditionDTOMapper = descriptorConditionDTOMapper;
         _formulaUsablePathDTOMapper = formulaUsablePathDTOMapper;
      }

      public SumFormulaDTO MapFrom(SumFormula sumFormula)
      {
         var dto = Map<SumFormulaDTO>(sumFormula);
         dto.Variable = sumFormula.Variable;
         dto.Dimension = sumFormula.Dimension;
         dto.VariablePattern = sumFormula.VariablePattern;
         dto.VariableCriteria = sumFormula.Criteria.MapAllUsing(_descriptorConditionDTOMapper);
         dto.ObjectPaths = _formulaUsablePathDTOMapper.MapFrom(sumFormula.ObjectPaths, sumFormula);
         dto.FormulaString = sumFormula.FormulaString;
         return dto;
      }

   }
}