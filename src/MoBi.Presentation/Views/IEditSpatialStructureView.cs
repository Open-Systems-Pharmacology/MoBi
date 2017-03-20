using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditSpatialStructureView : IView<IEditSpatialStructurePresenter>, IEditBuildingBlockBaseView
   {
      void SetEditView(IView view);
      void SetHierarchicalStructureView(IView view);
      void SetSpaceDiagramView(ISpatialStructureDiagramView view);
   }
}