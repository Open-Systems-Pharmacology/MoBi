using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class InteractionContainerDTO : ContainerDTO
   {
      public InteractionContainerDTO(InteractionContainer interactionContainer) : base(interactionContainer)
      {
      }

      public IEnumerable<ParameterDTO> Parameters { get; set; }
   }
}