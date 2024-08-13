using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Services
{
   public interface ISimulationEventsOnlyBuildingBlockVersionUpdater
   {
      void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, bool shouldIncrementVersion);
   }

   public class SimulationEventsOnlyBuildingBlockVersionUpdater : BuildingBlockVersionUpdater, ISimulationEventsOnlyBuildingBlockVersionUpdater
   {
      public SimulationEventsOnlyBuildingBlockVersionUpdater(IMoBiProjectRetriever projectRetriever, IEventPublisher eventPublisher, IDialogCreator dialogCreator) : base(projectRetriever, eventPublisher, dialogCreator)
      {
      }
      public void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, bool shouldIncrementVersion)
      {
         if (buildingBlock == null) return;
         var version = buildingBlock.Version;

         if (shouldIncrementVersion)
            version++;
         else
            version--;

         UpdateBuildingBlockVersion(buildingBlock, version);
      }
   }
}