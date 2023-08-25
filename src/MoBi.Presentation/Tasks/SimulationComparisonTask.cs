using OSPSuite.Utility.Events;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Tasks
{
   public interface ISimulationComparisonTask
   {
      void ShowDifferencesBetween(IBuildingBlock templateBuildingBlock, IBuildingBlock simulationBuildingBlock);
   }

   public class SimulationComparisonTask : ISimulationComparisonTask
   {
      private readonly IEventPublisher _eventPublisher;

      public SimulationComparisonTask(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      public void ShowDifferencesBetween(IBuildingBlock templateBuildingBlock, IBuildingBlock simulationBuildingBlock)
      {
         _eventPublisher.PublishEvent(new StartComparisonEvent(templateBuildingBlock, simulationBuildingBlock, leftCaption: ObjectTypes.BuildingBlock, rightCaption: ObjectTypes.Simulation));
      }
   }
}