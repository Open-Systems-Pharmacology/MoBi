using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class NeighborhoodDTO :ContainerDTO
   {
      public IEnumerable<TransportDTO> Transports { get; set; }
   }
}