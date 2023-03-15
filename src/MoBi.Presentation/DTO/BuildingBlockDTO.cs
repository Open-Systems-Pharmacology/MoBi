using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class BuildingBlockDTO : ObjectBaseDTO
   {
      public BuildingBlockDTO(IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         Builder = new List<ObjectBaseDTO>();
      }

      public IEnumerable<ObjectBaseDTO> Builder { get; set; }
   }
}