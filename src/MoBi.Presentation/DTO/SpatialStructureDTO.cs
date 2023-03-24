using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class SpatialStructureDTO : ObjectBaseDTO
   {
      public SpatialStructureDTO(ISpatialStructure spatialStructure) : base(spatialStructure)
      {
      }

      public IEnumerable<ContainerDTO> TopContainer { get; set; }
      public ContainerDTO Neighborhoods { get; set; }
      public ContainerDTO MoleculeProperties { get; set; }
   }

   public class SpatialStructureRootItem : ObjectBaseDTO
   {
   }
}