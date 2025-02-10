using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class NeighborhoodDTO : ContainerDTO
   {
      public NeighborhoodDTO(Neighborhood neighborhood) : base(neighborhood)
      {
         Icon = ApplicationIcons.Neighborhood;
      }

      public IEnumerable<TransportDTO> Transports { get; set; }
   }
}