using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class BuildingBlockDTO : ObjectBaseDTO
   {
      public BuildingBlockDTO()
      {
         Builder = new List<IObjectBaseDTO>();
      }

      public IEnumerable<IObjectBaseDTO> Builder { get; set; }
   }
}