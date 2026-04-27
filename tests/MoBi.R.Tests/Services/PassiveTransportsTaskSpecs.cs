using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_PassiveTransportsTask : ContextForIntegration<IPassiveTransportsTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetPassiveTransportsTask();
   }
}

internal class When_loading_passive_transports_from_pkml : concern_for_PassiveTransportsTask
{
   private PassiveTransportBuildingBlock _result;

   protected override void Because()
   {
      _result = sut.LoadFromPKML(HelperForSpecs.DataTestFileFullPath("simulation with two modules.pkml"));
   }

   [Observation]
   public void should_return_the_passive_transport_building_block_from_the_file()
   {
      _result.ShouldNotBeNull();
      _result.Name.ShouldBeEqualTo("Passive Transports");
   }
}
