using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetMoleculeStartValueWithUnitCommand : StartValueValueOrUnitChangedCommand<IMoleculeStartValue, IMoleculeStartValuesBuildingBlock>
   {
      public SetMoleculeStartValueWithUnitCommand(IMoleculeStartValue moleculeStartValue, double? newBaseValue, Unit newDisplayUnit, IMoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
         : base(moleculeStartValue, newBaseValue, newDisplayUnit, moleculeStartValuesBuildingBlock)
      {
      }
   }
}