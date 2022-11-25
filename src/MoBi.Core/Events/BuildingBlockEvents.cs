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

   public class StartValuesBuildingBlockChangedEvent : BuildingBlockEvent
   {
      public StartValuesBuildingBlockChangedEvent(IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
      }
   }
}