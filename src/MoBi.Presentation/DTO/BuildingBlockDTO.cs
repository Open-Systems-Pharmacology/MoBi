using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class BuildingBlockDTO : ObjectBaseDTO
   {
      public BuildingBlockDTO()
      {
         Builder = new List<ObjectBaseDTO>();
      }

      public IEnumerable<ObjectBaseDTO> Builder { get; set; }
   }
}