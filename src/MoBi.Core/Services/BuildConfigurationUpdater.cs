using OSPSuite.Utility.Events;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;

namespace MoBi.Core.Services
{
   public interface IBuildConfigurationUpdater
   {
      void UpdateBuildingConfiguration(object changedObject, IMoBiSimulation simulation, bool incrementSimulationChange);
   }

   public class BuildConfigurationUpdater : IBuildConfigurationUpdater
   {
      private readonly IAffectedBuildingBlockRetriever _affectedBuildingBlockRetriever;
      private readonly IEventPublisher _eventPublisher;

      public BuildConfigurationUpdater(IAffectedBuildingBlockRetriever affectedBuildingBlockRetriever, IEventPublisher eventPublisher)
      {
         _affectedBuildingBlockRetriever = affectedBuildingBlockRetriever;
         _eventPublisher = eventPublisher;
      }

      public void UpdateBuildingConfiguration(object changedObject, IMoBiSimulation simulation, bool incrementSimulationChange)
      {
         var affectedBuildingBlockInfo = _affectedBuildingBlockRetriever.RetrieveFor(changedObject, simulation);

         if (affectedBuildingBlockInfo?.UntypedBuildingBlock == null)
            return;

         if (incrementSimulationChange)
            affectedBuildingBlockInfo.SimulationChanges++;
         else
            affectedBuildingBlockInfo.SimulationChanges--;

         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }
   }
}