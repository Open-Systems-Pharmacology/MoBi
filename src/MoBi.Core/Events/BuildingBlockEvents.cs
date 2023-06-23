using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Events
{
   public class BuildingBlockEvent
   {
      public IBuildingBlock BuildingBlock { get; }

      public BuildingBlockEvent(IBuildingBlock buildingBlock)
      {
         BuildingBlock = buildingBlock;
      }
   }

   public class PathAndValueEntitiesBuildingBlockChangedEvent : BuildingBlockEvent
   {
      public PathAndValueEntitiesBuildingBlockChangedEvent(IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
      }
   }
}