using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class InteractionContainerDTO : ContainerDTO
   {
      public IEnumerable<ParameterDTO> Parameters { get; set; }
   }
}