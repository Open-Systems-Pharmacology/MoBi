using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Domain
{
   public class ModuleConfiguration
   {
      public Module Module { get; set; }
      public ParameterValuesBuildingBlock SelectedParameterValue { get; set; }
      public InitialConditionsBuildingBlock SelectedInitialCondition { get; set; }
   }
}