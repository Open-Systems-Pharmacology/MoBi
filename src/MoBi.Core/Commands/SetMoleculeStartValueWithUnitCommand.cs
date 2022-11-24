using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetMoleculeStartValueWithUnitCommand : StartValueValueOrUnitChangedCommand<IMoleculeStartValue, IMoleculeStartValuesBuildingBlock>
   {
      public SetMoleculeStartValueWithUnitCommand(IMoleculeStartValue moleculeBuilder, double? newBaseValue, Unit newDisplayUnit, IMoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
         : base(moleculeBuilder, newBaseValue, newDisplayUnit, moleculeStartValuesBuildingBlock)
      {
      }
   }
}