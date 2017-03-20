using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.ProjectConversion.v3_2
{
   public abstract class concern_for_PassiveTransportBuildingBlockConversionSpecs : ContextWithLoadedProject
   {
      protected IPassiveTransportBuildingBlock _passiveTransportBuildingBlock;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _passiveTransportBuildingBlock = LoadPKML<IPassiveTransportBuildingBlock>("Passive Transports");
      }
   }

   internal class When_converting_a_passive_transport_building_block : concern_for_PassiveTransportBuildingBlockConversionSpecs
   {
      [Observation]
      public void should_return_parameter_as_children()
      {
         var passiveTransport = _passiveTransportBuildingBlock.First();
         passiveTransport.GetChildren<IParameter>().Count().ShouldBeEqualTo(1);
         passiveTransport.Parameters.First().BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
      }
   }
}