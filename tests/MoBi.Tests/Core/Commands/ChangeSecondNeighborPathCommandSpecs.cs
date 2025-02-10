using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeSecondNeighborPathCommand : ContextSpecification<ChangeSecondNeighborPathCommand>
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

   public class When_setting_the_second_neighbor_of_a_neighborhood_to_something_defined : concern_for_ChangeSecondNeighborPathCommand
   {
      protected override void Context()
      {
         base.Context();
         _newPath = "A|B";
         sut = new ChangeSecondNeighborPathCommand(_newPath, _neighborhoodBuilder, _spatialStructure);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_set_the_path_as_expected()
      {
         _neighborhoodBuilder.SecondNeighborPath.PathAsString.ShouldBeEqualTo("A|B");
      }
   }

   public class When_setting_the_second_neighbor_of_a_neighborhood_to_something_undefined : concern_for_ChangeSecondNeighborPathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new ChangeSecondNeighborPathCommand("", _neighborhoodBuilder, _spatialStructure);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_set_the_path_as_expected()
      {
         _neighborhoodBuilder.SecondNeighborPath.ShouldBeNull();
      }
   }
}