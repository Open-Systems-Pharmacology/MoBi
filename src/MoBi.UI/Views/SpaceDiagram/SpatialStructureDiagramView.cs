using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Presenter.SpaceDiagram;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Views.BaseDiagram;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views.SpaceDiagram
{
   public  class SpatialStructureDiagramView : MoBiBaseDiagramView, ISpatialStructureDiagramView 
   {

      public SpatialStructureDiagramView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
      }

      public void AttachPresenter(ISpatialStructureDiagramPresenter presenter)
      {
         AttachPresenter(presenter as IMoBiBaseDiagramPresenter);
      }
    }
}