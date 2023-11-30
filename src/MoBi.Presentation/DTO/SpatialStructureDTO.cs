using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class SpatialStructureDTO : ObjectBaseDTO
   {
      public SpatialStructureDTO(SpatialStructure spatialStructure) : base(spatialStructure)
      {
      }

      public IReadOnlyList<ContainerDTO> TopContainers { get; set; }
      public ContainerDTO Neighborhoods { get; set; }
      public ContainerDTO MoleculeProperties { get; set; }
   }

   public class SpatialStructureRootItem : ObjectBaseDTO
   {
   }
}