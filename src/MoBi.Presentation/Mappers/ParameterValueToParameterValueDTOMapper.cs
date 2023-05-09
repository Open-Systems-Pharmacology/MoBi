using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IParameterValueToParameterValueDTOMapper : IPathAndValueEntityToPathAndValueEntityDTOMapper<ParameterValue, ParameterValueDTO>
   {
   }

   public class ParameterValueToParameterValueDTOMapper : IParameterValueToParameterValueDTOMapper
   {
      private readonly IFormulaToValueFormulaDTOMapper _formulaMapper;

      public ParameterValueToParameterValueDTOMapper(IFormulaToValueFormulaDTOMapper formulaMapper)
      {
         _formulaMapper = formulaMapper;
      }

      public ParameterValueDTO MapFrom(ParameterValue initialCondition, PathAndValueEntityBuildingBlock<ParameterValue> buildingBlock)
      {
         var dto = new ParameterValueDTO(initialCondition, buildingBlock)
         {
            ContainerPath = initialCondition.ContainerPath,
            Formula = _formulaMapper.MapFrom(initialCondition.Formula)
         };

         return dto;
      }
   }
}