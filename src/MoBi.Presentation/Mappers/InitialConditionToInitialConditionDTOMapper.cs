using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IPathAndValueEntityToPathAndValueEntityDTOMapper<TPathAndValueEntity, out TPathAndValueEntityDTO> where TPathAndValueEntity : PathAndValueEntity
   {
      TPathAndValueEntityDTO MapFrom(TPathAndValueEntity parameterValue, IBuildingBlock<TPathAndValueEntity> buildingBlock);
   }

   public interface IInitialConditionToInitialConditionDTOMapper : IPathAndValueEntityToPathAndValueEntityDTOMapper<InitialCondition, InitialConditionDTO>
   {
   }

   public class InitialConditionToInitialConditionDTOMapper : IInitialConditionToInitialConditionDTOMapper
   {
      private readonly IFormulaToValueFormulaDTOMapper _formulaMapper;

      public InitialConditionToInitialConditionDTOMapper(IFormulaToValueFormulaDTOMapper formulaMapper)
      {
         _formulaMapper = formulaMapper;
      }

      public InitialConditionDTO MapFrom(InitialCondition parameterValue, IBuildingBlock<InitialCondition> buildingBlock)
      {
         var dto = new InitialConditionDTO(parameterValue, buildingBlock)
         {
            ContainerPath = parameterValue.ContainerPath,
            Formula = _formulaMapper.MapFrom(parameterValue.Formula)
         };
         return dto;
      }
   }
}