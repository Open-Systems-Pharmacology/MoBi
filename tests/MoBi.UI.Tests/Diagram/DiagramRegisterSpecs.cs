using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Container;

namespace MoBi.UI.Diagram
{
   public abstract class concern_for_DiagramRegister : ContextSpecification<DiagramRegister>
   {
      protected IContainer _container;

      protected override void Context()
      {
         sut = new DiagramRegister();
         _container = new CastleWindsorContainer();
      }

      protected override void Because()
      {
         sut.RegisterInContainer(_container);
      }
   }

   public class When_registering_the_diagram_components : concern_for_DiagramRegister
   {
      [Observation]
      public void should_register_the_ui_free_container_node()
      {
         _container.Resolve<IContainerNode>().ShouldBeAnInstanceOf<ContainerNode>();
      }

      [Observation]
      public void should_register_the_ui_free_neighborhood_node()
      {
         _container.Resolve<INeighborhoodNode>().ShouldBeAnInstanceOf<NeighborhoodNode>();
      }

      [Observation]
      public void should_register_the_force_layout_configuration()
      {
         _container.Resolve<IForceLayoutConfiguration>().ShouldBeAnInstanceOf<ForceLayoutConfiguration>();
      }
   }
}