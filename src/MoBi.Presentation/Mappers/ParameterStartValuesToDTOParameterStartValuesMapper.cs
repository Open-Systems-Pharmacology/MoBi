using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IParameterStartValueToParameterStartValueDTOMapper : IStartValueToStartValueDTOMapper<IParameterStartValue, ParameterStartValueDTO>
   {
   }

   public class ParameterStartValueToParameterStartValueDTOMapper : IParameterStartValueToParameterStartValueDTOMapper
   {
      private readonly IFormulaToValueFormulaDTOMapper _formulaMapper;

      public ParameterStartValueToParameterStartValueDTOMapper(IFormulaToValueFormulaDTOMapper formulaMapper)
      {
         _formulaMapper = formulaMapper;
      }

      public ParameterStartValueDTO MapFrom(IParameterStartValue parameterStartValue, IStartValuesBuildingBlock<IParameterStartValue> buildingBlock)
      {
         var dto = new ParameterStartValueDTO(parameterStartValue, buildingBlock)
         {
            ContainerPath = parameterStartValue.ContainerPath,
            Formula = _formulaMapper.MapFrom(parameterStartValue.Formula)
         };

         return dto;
      }
   }
}