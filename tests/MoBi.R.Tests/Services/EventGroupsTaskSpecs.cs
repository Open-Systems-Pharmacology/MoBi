using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_EventGroupsTask : ContextForIntegration<IEventGroupsTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetEventGroupsTask();
   }
}

internal class When_loading_event_groups_from_pkml : concern_for_EventGroupsTask
{
   private EventGroupBuildingBlock _result;

   protected override void Because()
   {
      _result = sut.LoadFromPKML(HelperForSpecs.DataTestFileFullPath("simulation with two modules.pkml"));
   }

   [Observation]
   public void should_return_the_event_group_building_block_from_the_file()
   {
      _result.ShouldNotBeNull();
      _result.Name.ShouldBeEqualTo("Events");
   }
}
