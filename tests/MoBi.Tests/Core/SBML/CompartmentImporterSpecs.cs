using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using libsbmlcs;
using MoBi.IntegrationTests;
using IContainer = OSPSuite.Core.Domain.IContainer;
using Model = libsbmlcs.Model;

namespace MoBi.Core.SBML
{
   public abstract class ConcernForCompartmentImporterSpecs : ContextForIntegration<CompartmentImporter>
   {
      public class CompartmentImporterTests : ConcernForCompartmentImporterSpecs
      {
         private IContainer _container;
         private IContainer _container2;
         private IContainer _container3;

         protected override void Because()
         {
            var model = new Model(3, 1);
            model.setName("TestModel");
            model.setNotes("TestNotes");
            model.setMetaId("TestMetaId");
            sut.CreateEventsTopContainer();
            sut.CreateTopContainer(model);
            var compartment = new Compartment(3, 1);
            compartment.setName("Compartment");
            compartment.setId("c1");
            compartment.setSpatialDimensions(1);
            compartment.setSize(5);

            var compartment2 = new Compartment(3, 1);
            compartment2.setName("Compartment");
            compartment2.setId("c2");
            compartment2.setSpatialDimensions(2);
            compartment2.setSize(5);

            var compartment3 = new Compartment(3, 1);
            compartment3.setName("Compartment");
            compartment3.setId("c3");
            compartment3.setSpatialDimensions(3);
            compartment3.setSize(5);

            _container = sut.CreateContainerFromCompartment(compartment);
            _container2 = sut.CreateContainerFromCompartment(compartment2);
            _container3 = sut.CreateContainerFromCompartment(compartment3);
            sut.CreateSpatialStructureFromModel(sut._topContainer, model);
         }

         [Observation]
         public void EventsTopContainerCreationTest()
         {
            sut._eventsTopContainer.ShouldNotBeNull();
         }

         [Observation]
         public void TopContainerCreationTest()
         {
            sut._topContainer.ShouldNotBeNull();
         }

         [Observation]
         public void SpatialStructureCreationTest()
         {
            sut.SpatialStructure.ShouldNotBeNull();
         }


         [Observation]
         public void ContainerCreationTest()
         {
            _container.ShouldNotBeNull();
            _container.Name.ShouldBeEqualTo("c1");
            _container.Children.ShouldNotBeNull();
            var containsSizeParam = _container.Children.Any(x => x.Name == SBMLConstants.SIZE);
            var containsVolumeParam = _container.Children.Any(x => x.Name == SBMLConstants.VOLUME);
            containsSizeParam.ShouldBeTrue();
            containsVolumeParam.ShouldBeTrue();
         }

         [Observation]
         public void ContainerCreationTest2()
         {
            _container2.ShouldNotBeNull();
            _container2.Name.ShouldBeEqualTo("c2");
            _container2.Children.ShouldNotBeNull();
            var containsSizeParam = _container2.Children.Any(x => x.Name == SBMLConstants.SIZE);
            var containsVolumeParam = _container2.Children.Any(x => x.Name == SBMLConstants.VOLUME);
            containsSizeParam.ShouldBeTrue();
            containsVolumeParam.ShouldBeTrue();
         }

         [Observation]
         public void ContainerCreationTest3()
         {
            _container3.ShouldNotBeNull();
            _container3.Name.ShouldBeEqualTo("c3");
            _container3.Children.ShouldNotBeNull();
            var containsSizeParam = _container3.Children.Any(x => x.Name == SBMLConstants.SIZE);
            var containsVolumeParam = _container3.Children.Any(x => x.Name == SBMLConstants.VOLUME);
            containsSizeParam.ShouldBeTrue();
            containsVolumeParam.ShouldBeTrue();
         }
      }
   }
}
