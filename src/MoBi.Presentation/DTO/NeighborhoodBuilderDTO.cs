using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class NeighborhoodBuilderDTO : ObjectBaseDTO
   {
      public IList<TagDTO> Tags { get; set; }
      public ContainerMode Mode { get; set; }
      public ContainerType ContainerType { get; set; }
   }
}