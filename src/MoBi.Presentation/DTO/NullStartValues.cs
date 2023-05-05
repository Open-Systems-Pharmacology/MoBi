using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public static class NullStartValues
   {
      public static InitialConditionsBuildingBlock NullMoleculeStartValues { get; } = new InitialConditionsBuildingBlock().WithName(AppConstants.Captions.NoMoleculeStartValues);
      public static ParameterStartValuesBuildingBlock NullParameterStartValues { get; } = new ParameterStartValuesBuildingBlock().WithName(AppConstants.Captions.NoParameterStartValues);
   }
}