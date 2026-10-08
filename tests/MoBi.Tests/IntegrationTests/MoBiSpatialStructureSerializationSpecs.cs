using System.Linq;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   public class When_serializing_and_deserializing_a_spatial_structure : ContextForIntegration<IXmlSerializationService>
   {
      private MoBiSpatialStructure _deserializedSpatialStructure;

      protected override void Because()
      {
         var spatialStructure = IoC.Resolve<IMoBiSpatialStructureFactory>().CreateDefault();
         spatialStructure.DiagramModel = IoC.Resolve<IDiagramModelFactory>().Create();
         _deserializedSpatialStructure = sut.Deserialize<MoBiSpatialStructure>(sut.SerializeAsString(spatialStructure), new MoBiProject());
      }

      [Observation]
      public void should_create_the_diagram_model_with_the_diagram_model_factory()
      {
         _deserializedSpatialStructure.DiagramModel.ShouldBeAnInstanceOf<DiagramModel>();
      }

      [Observation]
      public void should_initialize_the_diagram_manager()
      {
         _deserializedSpatialStructure.DiagramManager.ShouldNotBeNull();
      }
   }

   public class When_building_the_diagram_of_a_deserialized_spatial_structure_with_neighborhoods : ContextForIntegration<IXmlSerializationService>
   {
      private MoBiSpatialStructure _deserializedSpatialStructure;

      protected override void Because()
      {
         var spatialStructure = IoC.Resolve<IMoBiSpatialStructureFactory>().CreateDefault();
         var organism = new Container {ContainerType = ContainerType.Organism}.WithName("Organism").WithId("organism");
         organism.Add(new Container {ContainerType = ContainerType.Compartment}.WithName("Plasma").WithId("plasma"));
         organism.Add(new Container {ContainerType = ContainerType.Compartment}.WithName("Cells").WithId("cells"));
         spatialStructure.AddTopContainer(organism);
         spatialStructure.AddNeighborhood(new NeighborhoodBuilder
         {
            FirstNeighborPath = new ObjectPath("Organism", "Plasma"),
            SecondNeighborPath = new ObjectPath("Organism", "Cells")
         }.WithName("Plasma_Cells").WithId("neighborhood"));

         _deserializedSpatialStructure = sut.Deserialize<MoBiSpatialStructure>(sut.SerializeAsString(spatialStructure), new MoBiProject());
         _deserializedSpatialStructure.DiagramModel = IoC.Resolve<IDiagramModelFactory>().Create();
         _deserializedSpatialStructure.DiagramManager = new SpatialStructureDiagramManager();
         _deserializedSpatialStructure.DiagramManager.InitializeWith(_deserializedSpatialStructure, new DiagramOptions());
      }

      [Observation]
      public void should_create_the_container_nodes_nested_like_the_spatial_structure()
      {
         var organismNode = _deserializedSpatialStructure.DiagramModel.FindByName("Organism").DowncastTo<ContainerNode>();
         organismNode.GetDirectChildren<IContainerNode>().Select(x => x.Name).ShouldOnlyContain("Plasma", "Cells");
      }

      [Observation]
      public void should_wire_the_neighborhood_node_to_both_container_nodes()
      {
         var neighborhoodNode = _deserializedSpatialStructure.DiagramModel.GetAllChildren<NeighborhoodNode>().Single();
         neighborhoodNode.FirstNeighbor.Name.ShouldBeEqualTo("Plasma");
         neighborhoodNode.SecondNeighbor.Name.ShouldBeEqualTo("Cells");
      }
   }
}
