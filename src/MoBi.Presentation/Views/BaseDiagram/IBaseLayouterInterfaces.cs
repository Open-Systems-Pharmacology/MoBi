using OSPSuite.Presentation.Diagram.Elements;

namespace MoBi.Presentation.Views.BaseDiagram
{
   public enum LayoutTarget
   {
      VisibleChildren,
      VisibleGrandChildren,
      VisibleGreatGrandChildren,
      AllVisibleDescendants
   }


   public interface IGeneralDiagramLayouter : IContainerBaseLayouter
   { }
}