using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class CommitTargetDTO
   {
      public Module Module { get; set; }
      public ParameterValuesBuildingBlock ParameterValues { get; set; }
      public InitialConditionsBuildingBlock InitialConditions { get; set; }
   }
}
