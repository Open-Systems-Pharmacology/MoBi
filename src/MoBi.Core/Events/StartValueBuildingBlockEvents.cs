using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Events
{
   public class StartValueBuildingBlockEvent
   {
      public IBuildingBlock BuildingBlock { get; private set; }

      public StartValueBuildingBlockEvent(IBuildingBlock buildingBlock)
      {
         BuildingBlock = buildingBlock;
      }
   }

   public class StartValuesBuildingBlockChangedEvent : StartValueBuildingBlockEvent
   {
      public StartValuesBuildingBlockChangedEvent(IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
      }
   }
}