using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Services
{
   public interface ISimulationEventsOnlyBuildingBlockVersionUpdater
   {
      void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, uint newVersion);
      void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, bool shouldIncrementVersion);
   }

   public class SimulationEventsOnlyBuildingBlockVersionUpdater : BuildingBlockVersionUpdater, ISimulationEventsOnlyBuildingBlockVersionUpdater
   {
      public SimulationEventsOnlyBuildingBlockVersionUpdater(IMoBiProjectRetriever projectRetriever, IEventPublisher eventPublisher, IDialogCreator dialogCreator) : base(projectRetriever, eventPublisher, dialogCreator)
      {
      }

      public override void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, uint newVersion)
      {
         if (buildingBlock == null)
            return;

         buildingBlock.Version = newVersion;
         publishSimulationStatusChangedEvents(buildingBlock);
      }
   }
}