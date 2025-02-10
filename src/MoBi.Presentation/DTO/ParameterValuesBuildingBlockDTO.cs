using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ParameterValuesBuildingBlockDTO
   {
      public ParameterValuesBuildingBlock BuildingBlock { get; }

      public ParameterValuesBuildingBlockDTO(ParameterValuesBuildingBlock buildingBlock)
      {
         BuildingBlock = buildingBlock;
      }

      public IReadOnlyList<ParameterValueDTO> ParameterDTOs { get; set; }
   }
}