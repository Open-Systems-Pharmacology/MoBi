using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.ProjectConversion.v3_3
{
   public abstract class concern_for_PassiveTransportBuildignBlockConversionSpecs : ContextWithLoadedProject
   {
      protected IPassiveTransportBuildingBlock _passiveTransportBuildingBlock;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _passiveTransportBuildingBlock = LoadPKML<IPassiveTransportBuildingBlock>("Passive Transports");
      }
   }

   internal class When_converting_an_passive_tranport_building_block : concern_for_PassiveTransportBuildignBlockConversionSpecs
   {
      [Observation]
      public void should_have_changed_GallbladderEmptyin_Transport()
      {
         var gallbladderEmptying = _passiveTransportBuildingBlock.FirstOrDefault(pt => pt.Name.Equals("GallbladderEmptying"));
         gallbladderEmptying.ShouldNotBeNull();
         var kinetic = ((ExplicitFormula) gallbladderEmptying.Formula);
         kinetic.FormulaString.ShouldBeEqualTo("EHC_Active ? ln(2) / EHC_Halftime * M * EHC_EjectionFraction : 0");
         var alias = kinetic.ObjectPaths.Select(op => op.Alias);
         alias.ShouldOnlyContain("EHC_Active", "EHC_Halftime", "M", "EHC_EjectionFraction");
      }
   }
}