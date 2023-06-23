using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Events
{
   public class BuildingBlockChartTemplatesModifiedEvent
   {
      public SimulationSettings SimulationSettings { get; }

      public BuildingBlockChartTemplatesModifiedEvent(SimulationSettings simulationSettings)
      {
         SimulationSettings = simulationSettings;
      }
   }
}