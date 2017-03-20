﻿using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class SimulationSettingsReporter : BuildingBlocksReporter<ISimulationSettings>
   {
      public SimulationSettingsReporter(SimulationSettingReporter simulationSettingReporter)
         : base(simulationSettingReporter, Constants.SIMULATION_SETTINGS)
      {
      }
   }
}