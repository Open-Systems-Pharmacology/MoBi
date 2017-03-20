using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.DTO
{
   public class BuildingBlockMappingDTO
   {
      public ApplicationIcon BuildingBlockIcon { get; set; }
      public IBuildingBlock ProjectBuildingBlock { get; set; }
      public IBuildingBlock BuildingBlockToMerge { get; set; }
      public IEnumerable<IBuildingBlock> AllAvailableBuildingBlocks { get; set; }
   }
}