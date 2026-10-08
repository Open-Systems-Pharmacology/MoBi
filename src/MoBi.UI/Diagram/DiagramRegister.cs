using OSPSuite.Utility.Container;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace MoBi.UI.Diagram
{
   public class DiagramRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.Register<IContainerNode, ContainerNode>();
         container.Register<INeighborhoodNode, NeighborhoodNode>();
      }
   }
}
