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

      public ParameterValueDTO MapFrom(ParameterValue parameterValue, IBuildingBlock<ParameterValue> buildingBlock) =>
         new ParameterValueDTO(parameterValue, buildingBlock)
         {
            ContainerPath = parameterValue.ContainerPath,
            Formula = _formulaMapper.MapFrom(parameterValue.Formula)
         };
   }
}