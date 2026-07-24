using MoBi.Core.Domain.Repository;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core
{
   public abstract class concern_for_SimulationContentRepository : ContextSpecification<ISimulationContentRepository>
   {
      protected byte[] _content;

      protected override void Context()
      {
         sut = new SimulationContentRepository();
         _content = "compressed-simulation-content"u8.ToArray();
      }
   }

   public class When_registering_and_retrieving_simulation_content : concern_for_SimulationContentRepository
   {
      protected override void Because()
      {
         sut.Register("sim-1", _content);
      }

      [Observation]
      public void should_report_that_content_is_available_for_the_registered_id()
      {
         sut.Contains("sim-1").ShouldBeTrue();
      }

      [Observation]
      public void should_return_the_registered_content()
      {
         sut.ContentFor("sim-1").ShouldBeEqualTo(_content);
      }

      [Observation]
      public void should_not_report_content_for_an_unknown_id()
      {
         sut.Contains("unknown").ShouldBeFalse();
         sut.ContentFor("unknown").ShouldBeNull();
      }
   }

   public class When_clearing_the_simulation_content_repository : concern_for_SimulationContentRepository
   {
      protected override void Context()
      {
         base.Context();
         sut.Register("sim-1", _content);
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_no_longer_contain_any_content()
      {
         sut.Contains("sim-1").ShouldBeFalse();
      }
   }
}