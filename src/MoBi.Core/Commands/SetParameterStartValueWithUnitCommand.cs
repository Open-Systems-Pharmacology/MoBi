using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetParameterStartValueWithUnitCommand : StartValueOrUnitChangedCommand<IParameterStartValue, IParameterStartValuesBuildingBlock>
   {
      public SetParameterStartValueWithUnitCommand(IParameterStartValue parameterBuilder, double? newBaseValue, Unit newDisplayUnit, IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
         : base(parameterBuilder, newBaseValue, newDisplayUnit, parameterStartValuesBuildingBlock)
      {
      }

   }
}