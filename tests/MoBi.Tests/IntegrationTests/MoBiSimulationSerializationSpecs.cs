using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.IntegrationTests
{
   public class When_serializing_and_deserializing_a_simulation_with_a_saved_diagram_layout : ContextForIntegration<IXmlSerializationService>
   {
      private XElement _diagramModelXml;
      private string _xml;
      private MoBiSimulation _deserializedSimulation;

      protected override void Because()
      {
         var simulation = DomainFactoryForSpecs.CreateSimulationFor(DomainFactoryForSpecs.CreateDefaultConfiguration());
         _diagramModelXml = XElement.Parse(@"<DiagramModel IsLayouted=""True""><MultiPortContainerNode Id=""organism"" Name=""Organism"" Location=""10 20"" Size=""100 50"" IsExpanded=""false"" /></DiagramModel>");
         simulation.DiagramModelXml = _diagramModelXml;
         _xml = sut.SerializeAsString(simulation);
         _deserializedSimulation = sut.Deserialize<MoBiSimulation>(_xml, new MoBiProject());
      }

      [Observation]
      public void should_write_the_layout_as_the_legacy_diagram_model_element_of_the_simulation()
      {
         var element = XElement.Parse(_xml).Element(Constants.Serialization.DIAGRAM_MODEL);
         element.ShouldNotBeNull();
         XNode.DeepEquals(element, _diagramModelXml).ShouldBeTrue();
      }

      [Observation]
      public void should_read_the_layout_back_unchanged()
      {
         XNode.DeepEquals(_deserializedSimulation.DiagramModelXml, _diagramModelXml).ShouldBeTrue();
      }

      [Observation]
      public void should_keep_a_detached_copy_of_the_element()
      {
         _deserializedSimulation.DiagramModelXml.Parent.ShouldBeNull();
      }
   }

   public class When_serializing_and_deserializing_a_simulation_without_a_saved_diagram_layout : ContextForIntegration<IXmlSerializationService>
   {
      private MoBiSimulation _deserializedSimulation;

      protected override void Because()
      {
         var simulation = DomainFactoryForSpecs.CreateSimulationFor(DomainFactoryForSpecs.CreateDefaultConfiguration());
         _deserializedSimulation = sut.Deserialize<MoBiSimulation>(sut.SerializeAsString(simulation), new MoBiProject());
      }

      [Observation]
      public void should_not_have_a_saved_diagram_layout()
      {
         _deserializedSimulation.DiagramModelXml.ShouldBeNull();
      }
   }
}
