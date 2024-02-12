using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IInitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper : IMapper<InitialConditionsBuildingBlock, InitialConditionsBuildingBlockDTO>
   {
   }

   public class InitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper :
      PathAndValueEntityBuildingBlockToPathAndValueEntityBuildingBlockDTOMapper<InitialConditionsBuildingBlock, InitialCondition, InitialConditionsBuildingBlockDTO, InitialConditionDTO>,
      IInitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper
   {
      private readonly IInitialConditionToInitialConditionDTOMapper _initialConditionMapper;

      public InitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper(IInitialConditionToInitialConditionDTOMapper initialConditionMapper)
      {
         _initialConditionMapper = initialConditionMapper;
      }

      protected override InitialConditionDTO BuilderDTOFor(InitialCondition pathAndValueEntity, InitialConditionsBuildingBlock buildingBlock)
      {
         return _initialConditionMapper.MapFrom(pathAndValueEntity, buildingBlock);
      }

      protected override InitialConditionsBuildingBlockDTO MapBuildingBlockDTO(InitialConditionsBuildingBlock buildingBlock, List<InitialConditionDTO> parameterDTOs)
      {
         return new InitialConditionsBuildingBlockDTO(buildingBlock) { ParameterDTOs = parameterDTOs };
      }
   }
}