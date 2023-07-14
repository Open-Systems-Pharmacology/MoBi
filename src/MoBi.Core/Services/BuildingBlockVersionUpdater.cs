using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IBuildingBlockVersionUpdater
   {
      void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, uint newVersion);
      void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, bool shouldIncrementVersion);
   }

   public class BuildingBlockVersionUpdater : IBuildingBlockVersionUpdater
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IEventPublisher _eventPublisher;

      public BuildingBlockVersionUpdater(IMoBiProjectRetriever projectRetriever, IEventPublisher eventPublisher)
      {
         _projectRetriever = projectRetriever;
         _eventPublisher = eventPublisher;
      }

      public void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, uint newVersion)
      {
         if (buildingBlock == null) return;
         buildingBlock.Version = newVersion;
         publishSimulationStatusChangedEvents(buildingBlock);
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

      private void publishSimulationStatusChangedEvents(IBuildingBlock changedBuildingBlock)
      {
         var affectedSimulations = _projectRetriever.Current.SimulationsUsing(changedBuildingBlock);
         affectedSimulations.Each(refreshSimulation);
      }

      private void refreshSimulation(IMoBiSimulation simulation)
      {
         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }
   }
}