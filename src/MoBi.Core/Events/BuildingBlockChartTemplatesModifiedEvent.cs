using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Events
{
   public class BuildingBlockChartTemplatesModifiedEvent
   {
      public ISimulationSettings SimulationSettings { get; private set; }

      public BuildingBlockChartTemplatesModifiedEvent(ISimulationSettings simulationSettings)
      {
         SimulationSettings = simulationSettings;
      }
   }
}