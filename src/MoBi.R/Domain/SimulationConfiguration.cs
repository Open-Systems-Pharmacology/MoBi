using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Domain
{
   public class SimulationConfiguration
   {
      public IndividualBuildingBlock Individual { get; set; }
      public List<ModuleConfiguration> ModuleConfigurations { get; set; } = new List<ModuleConfiguration>();
      public List<ExpressionProfileBuildingBlock> ExpressionProfiles { get; set; } = new List<ExpressionProfileBuildingBlock>();
      public string SimulationName { get; set; }
   }
}