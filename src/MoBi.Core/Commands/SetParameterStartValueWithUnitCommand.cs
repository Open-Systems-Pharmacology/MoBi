using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetParameterStartValueWithUnitCommand : StartValueValueOrUnitChangedCommand<IParameterStartValue, IParameterStartValuesBuildingBlock>
   {
      public SetParameterStartValueWithUnitCommand(IParameterStartValue parameterStartValue, double? newBaseValue, Unit newDisplayUnit, IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
         : base(parameterStartValue, newBaseValue, newDisplayUnit, parameterStartValuesBuildingBlock)
      {
      }

   }
}