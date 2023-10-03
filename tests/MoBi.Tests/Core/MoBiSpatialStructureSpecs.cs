using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiSpatialStructure : ContextSpecification<MoBiSpatialStructure>
   {
      protected override void Context()
      {
         sut = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS)
         };
      }
   }

   public class When_updating_properties_from_source_spatial_structure : concern_for_MoBiSpatialStructure
   {
      private ICloneManager _cloneManager;
      private ISpatialStructureDiagramManager _diagramManager;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         _diagramManager = A.Fake<ISpatialStructureDiagramManager>();
         _spatialStructure = new MoBiSpatialStructure {DiagramManager = _diagramManager};
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_spatialStructure, _cloneManager);
      }

      [Observation]
      public void the_source_diagram_manager_creates_new_diagram_manager_for_target()
      {
         A.CallTo(() => _diagramManager.Create()).MustHaveHappened();
      }
   }

   public class When_retrieving_the_list_of_neighborhood_connected_to_some_containers : concern_for_MoBiSpatialStructure
   {
      private Container _parentContainer1;
      private Container _container2;
      private Container _container1;
      private Container _container3;
      private IObjectPathFactory _objectPathFactory;
      private IReadOnlyList<NeighborhoodBuilder> _result;
      private NeighborhoodBuilder _neighborhoodBetweenCont1AndCont2;
      private NeighborhoodBuilder _neighborhoodBetweenCont2AndUnknown;
      private NeighborhoodBuilder _randomNeighborhood;

      protected override void Context()
      {
         base.Context();
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _parentContainer1 = new Container().WithName("_parentContainer1").WithMode(ContainerMode.Logical);
         _container1 = new Container().WithName("_container1").WithMode(ContainerMode.Physical);
         _container2 = new Container().WithName("_container2").WithMode(ContainerMode.Physical);
         _container3 = new Container().WithName("_container3").WithMode(ContainerMode.Physical);

         _neighborhoodBetweenCont1AndCont2 = new NeighborhoodBuilder
         {
            FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_container1),
            SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_container2),
            Name = "_neighborhoodBetweenCont1AndCont2"
         };
         sut.AddNeighborhood(_neighborhoodBetweenCont1AndCont2);

         _neighborhoodBetweenCont2AndUnknown = new NeighborhoodBuilder
         {
            FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_container2),
            SecondNeighborPath = new ObjectPath("A", "PATH"),
            Name = "_neighborhoodBetweenCont2AndUnknown"
         };
         sut.AddNeighborhood(_neighborhoodBetweenCont2AndUnknown);

         _randomNeighborhood = new NeighborhoodBuilder
         {
            FirstNeighborPath = new ObjectPath("PATH1"),
            SecondNeighborPath = new ObjectPath("PATH2"),
            Name = "_randomNeighborhood"
         };

         sut.AddNeighborhood(_randomNeighborhood);
      }

      protected override void Because()
      {
         _result = sut.GetConnectingNeighborhoods(new[] {_parentContainer1, _container2, _container3}, _objectPathFactory);
      }

      [Observation]
      public void should_return_the_list_of_all_neighborhood_connected_to_at_least_one_neighbors()
      {
         _result.ShouldOnlyContain(_neighborhoodBetweenCont1AndCont2, _neighborhoodBetweenCont2AndUnknown);
      }
   }

   public class When_retrieving_the_list_of_neighborhood_connected_to_a_container_without_parent_but_with_a_parent_path_set : concern_for_MoBiSpatialStructure
   {
      private Container _container;
      private IObjectPathFactory _objectPathFactory;
      private IReadOnlyList<NeighborhoodBuilder> _result;
      private NeighborhoodBuilder _neighborhood;

      protected override void Context()
      {
         base.Context();
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _container = new Container().WithName("Muscle").WithMode(ContainerMode.Physical);
         _container.ParentPath = new ObjectPath("Organism");

         _neighborhood = new NeighborhoodBuilder
         {
            FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_container).AndAddAtFront("Organism"),
            SecondNeighborPath = new ObjectPath("A", "PATH"),
            Name = "_neighborhoodBetweenCont2AndUnknown"
         };
         sut.AddNeighborhood(_neighborhood);
      }

      protected override void Because()
      {
         _result = sut.GetConnectingNeighborhoods(new[] {_container}, _objectPathFactory);
      }

      [Observation]
      public void should_return_the_list_of_all_neighborhood_connected_to_at_least_one_neighbors()
      {
         _result.ShouldContain(_neighborhood);
      }
   }
}