using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Diagram
{
   public abstract class concern_for_SpatialStructureDiagramManager : ContextSpecification<SpatialStructureDiagramManager>
   {
      protected override void Context()
      {
         sut = new SpatialStructureDiagramManager();
      }
   }

   public class When_creating_a_new_spatial_structure_diagram_manager : concern_for_SpatialStructureDiagramManager
   {
      [Observation]
      public void the_manager_must_implement_the_interface()
      {
         sut.Create().IsAnImplementationOf<ISpatialStructureDiagramManager>().ShouldBeTrue();
      }
   }

   public class When_initializing_the_diagram_manager_with_a_spatial_structure : concern_for_SpatialStructureDiagramManager
   {
      private MoBiSpatialStructure _spatialStructure;
      private IContainer _organism;
      private IContainer _liver;
      private IContainer _kidney;
      private NeighborhoodBuilder _neighborhood;

      protected override void Context()
      {
         base.Context();
         _organism = new Container { Id = "Organism", Name = "Organism", ContainerType = ContainerType.Organism };
         _liver = new Container { Id = "Liver", Name = "Liver", ContainerType = ContainerType.Compartment };
         _kidney = new Container { Id = "Kidney", Name = "Kidney", ContainerType = ContainerType.Compartment };
         _organism.Add(_liver);
         _organism.Add(_kidney);

         _neighborhood = new NeighborhoodBuilder
         {
            Id = "Neighborhood",
            Name = "Neighborhood",
            FirstNeighborPath = new ObjectPath(_organism.Name, _liver.Name),
            SecondNeighborPath = new ObjectPath(_organism.Name, _kidney.Name)
         };

         _spatialStructure = new MoBiSpatialStructure
         {
            DiagramModel = new DiagramModel(),
            DiagramManager = sut,
            NeighborhoodsContainer = new Container { Id = "Neighborhoods", Name = "Neighborhoods" }
         };
         _spatialStructure.AddTopContainer(_organism);
         _spatialStructure.AddNeighborhood(_neighborhood);
      }

      protected override void Because()
      {
         sut.InitializeWith(_spatialStructure, new DiagramOptions());
      }

      [Observation]
      public void should_create_a_ui_free_container_node_for_the_top_container()
      {
         _spatialStructure.DiagramModel.GetNode(_organism.Id).ShouldBeAnInstanceOf<ContainerNode>();
      }

      [Observation]
      public void should_nest_the_compartment_nodes_in_the_top_container_node()
      {
         _spatialStructure.DiagramModel.GetNode(_liver.Id).GetParent().ShouldBeEqualTo<IContainerBase>(_spatialStructure.DiagramModel.GetNode<IContainerNode>(_organism.Id));
      }

      [Observation]
      public void should_create_a_ui_free_neighborhood_node_connecting_both_neighbors()
      {
         var neighborhoodNode = _spatialStructure.DiagramModel.GetNode<NeighborhoodNode>(_neighborhood.Id);
         neighborhoodNode.FirstNeighbor.Id.ShouldBeEqualTo(_liver.Id);
         neighborhoodNode.SecondNeighbor.Id.ShouldBeEqualTo(_kidney.Id);
      }

      [Observation]
      public void should_hide_the_neighborhoods_container_node()
      {
         _spatialStructure.DiagramModel.GetNode(_spatialStructure.NeighborhoodsContainer.Id).IsVisible.ShouldBeFalse();
      }
   }
}