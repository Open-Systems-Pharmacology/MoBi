using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class NeighborhoodDTO : ContainerDTO
   {
      public NeighborhoodDTO(Neighborhood neighborhood) : base(neighborhood)
      {
      }

      public IEnumerable<TransportDTO> Transports { get; set; }
   }
}