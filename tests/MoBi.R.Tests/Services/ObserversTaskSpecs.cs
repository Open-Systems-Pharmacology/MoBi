using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_ObserversTask : ContextForIntegration<IObserversTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetObserversTask();
   }
}

internal class When_loading_observers_from_pkml : concern_for_ObserversTask
{
   private ObserverBuildingBlock _result;

   protected override void Because()
   {
      _result = sut.LoadFromPKML(HelperForSpecs.DataTestFileFullPath("simulation with two modules.pkml"));
   }

   [Observation]
   public void should_return_the_observer_building_block_from_the_file()
   {
      _result.ShouldNotBeNull();
      _result.Name.ShouldBeEqualTo("Observer");
   }
}
