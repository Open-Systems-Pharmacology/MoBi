using System.Collections.Generic;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.DTO
{
   public class SpatialStructureDTO : ObjectBaseDTO
   {
      public IEnumerable<ContainerDTO> TopContainer { get; set; }
      public ContainerDTO Neighborhoods { get; set; }
      public ContainerDTO MoleculeProperties { get; set; }
   }

   public class SpatialStructureRootItem : ObjectBaseDTO
   {
      
   }
   
}