using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class NeighborhoodBuilderDTO : ObjectBaseDTO
   {
      public NeighborhoodBuilderDTO(NeighborhoodBuilder neighborhoodBuilder) : base(neighborhoodBuilder)
      {
      }

      public IList<TagDTO> Tags { get; set; }
      public ContainerMode Mode { get; set; }
      public ContainerType ContainerType { get; set; }
   }
}