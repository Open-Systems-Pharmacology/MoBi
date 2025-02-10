using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class InitialConditionsBuildingBlockDTO
   {
      public InitialConditionsBuildingBlock BuildingBlock { get; }

      public InitialConditionsBuildingBlockDTO(InitialConditionsBuildingBlock buildingBlock)
      {
         BuildingBlock = buildingBlock;
      }

      public IReadOnlyList<InitialConditionDTO> ParameterDTOs { get; set; }
   }
}