using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mappers
{
   public interface IParameterStartValueToParameterStartValueDTOMapper : IStartValueToStartValueDTOMapper<IParameterStartValue, ParameterStartValueDTO>
   {
   }

   public class ParameterStartValueToParameterStartValueDTOMapper : IParameterStartValueToParameterStartValueDTOMapper
   {
      public ParameterStartValueDTO MapFrom(IParameterStartValue parameterStartValue, IStartValuesBuildingBlock<IParameterStartValue> buildingBlock)
      {
         var dto = new ParameterStartValueDTO(parameterStartValue, buildingBlock)
         {
            ContainerPath = parameterStartValue.ContainerPath,
         };

         var formula = parameterStartValue.Formula as ExplicitFormula;
         dto.Formula = formula != null ? new StartValueFormulaDTO(formula) : new EmptyFormulaDTO();
         return dto;
      }
   }
}