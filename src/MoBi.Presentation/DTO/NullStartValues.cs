using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public static class NullStartValues
   {
      public static InitialConditionsBuildingBlock NullInitialConditions { get; } = new InitialConditionsBuildingBlock().WithName(AppConstants.Captions.NoInitialConditions);
      public static ParameterValuesBuildingBlock NullParameterValues { get; } = new ParameterValuesBuildingBlock().WithName(AppConstants.Captions.NoParameterValues);
   }
}