using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Domain
{
   public class SimulationConfiguration
   {
      public IndividualBuildingBlock Individual { get; set; }
      public ModuleConfiguration[] ModuleConfigurations { get; set; }
      public ExpressionProfileBuildingBlock[] ExpressionProfiles { get; set; }
   }
}