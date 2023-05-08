using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{

   public interface IStartValueToStartValueDTOMapper<TStartValue, out TStartValueDTO> where TStartValue : PathAndValueEntity
   {
      TStartValueDTO MapFrom(TStartValue startValue, PathAndValueEntityBuildingBlock<TStartValue> buildingBlock);
   }

   public interface IInitialConditionToInitialConditionDTOMapper : IStartValueToStartValueDTOMapper<InitialCondition, InitialConditionDTO>
   {
      
   }

   public class InitialConditionToInitialConditionDTOMapper : IInitialConditionToInitialConditionDTOMapper
   {
      private readonly IFormulaToValueFormulaDTOMapper _formulaMapper;

      public InitialConditionToInitialConditionDTOMapper(IFormulaToValueFormulaDTOMapper formulaMapper)
      {
         _formulaMapper = formulaMapper;
      }
      public InitialConditionDTO MapFrom(InitialCondition initialCondition, PathAndValueEntityBuildingBlock<InitialCondition> buildingBlock)
      {
         var dto = new InitialConditionDTO(initialCondition, buildingBlock)
         {
            ContainerPath = initialCondition.ContainerPath,
            Formula = _formulaMapper.MapFrom(initialCondition.Formula)
         };
         return dto;
      }
   }
}