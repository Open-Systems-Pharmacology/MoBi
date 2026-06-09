using MoBi.Presentation.Presenter.ModelDiagram;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Views.BaseDiagram;
using OSPSuite.Core.Diagram;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views.ModelDiagram
{
   public class SimulationDiagramView : MoBiBaseDiagramView, ISimulationDiagramView
   {
      public SimulationDiagramView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
      }

      public void AttachPresenter(ISimulationDiagramPresenter presenter)
      {
         base.AttachPresenter(presenter);
      }

      public void ObserverLinksVisible(IDiagramModel diagramModel, bool visible)
      {
         foreach (var observerLink in diagramModel.GetAllChildren<ObserverLink>())
         {
            observerLink.IsVisible = visible;
         }
      }
   }
}