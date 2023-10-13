using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeFirstNeighborPathCommand : ContextSpecification<ChangeFirstNeighborPathCommand>
   {
      protected string _newPath;
      protected NeighborhoodBuilder _neighborhoodBuilder;
      protected SpatialStructure _spatialStructure;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _neighborhoodBuilder = new NeighborhoodBuilder();
         _spatialStructure = new SpatialStructure();
         _context = A.Fake<IMoBiContext>();
      }
   }

   public class When_setting_the_first_neighbor_of_a_neighborhood_to_something_defined : concern_for_ChangeFirstNeighborPathCommand
   {
      private NeighborhoodChangedEvent _event;

      protected override void Context()
      {
         base.Context();
         _newPath = "A|B";
         sut = new ChangeFirstNeighborPathCommand(_newPath, _neighborhoodBuilder, _spatialStructure);

         A.CallTo(() => _context.PublishEvent(A<NeighborhoodChangedEvent>._))
            .Invokes(x => _event = x.GetArgument<NeighborhoodChangedEvent>(0));

      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_set_the_path_as_expected()
      {
         _neighborhoodBuilder.FirstNeighborPath.PathAsString.ShouldBeEqualTo("A|B");
      }

      [Observation]
      public void should_notify_a_neighborhood_change_event()
      {
         _event.NeighborhoodBuilder.ShouldBeEqualTo(_neighborhoodBuilder);
      }
   }

   public class When_setting_the_first_neighbor_of_a_neighborhood_to_something_undefined : concern_for_ChangeFirstNeighborPathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new ChangeFirstNeighborPathCommand("", _neighborhoodBuilder, _spatialStructure);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_set_the_path_as_expected()
      {
         _neighborhoodBuilder.FirstNeighborPath.ShouldBeNull();
      }
   }
}