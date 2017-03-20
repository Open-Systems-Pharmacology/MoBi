using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core.Diagram;

namespace MoBi.Presentation.Presenter.BaseDiagram
{
   public interface IDiagramOptionsPresenter : ISimpleEditPresenter<IDiagramOptions>
   {}

   public class DiagramOptionsPresenter : SimpleEditPresenter<IDiagramOptions>, IDiagramOptionsPresenter
   {
      public DiagramOptionsPresenter(IDiagramOptionsView view) : base(view)
      {}
   }
}