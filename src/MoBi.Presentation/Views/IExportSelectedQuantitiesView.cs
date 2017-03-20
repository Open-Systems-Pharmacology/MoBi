using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IExportSelectedQuantitiesView : IModalView<IExportSelectedQuantitiesPresenter>
   {
      void AddSelectionView(IView view);
      void BindTo(ExportQuantitiesSelectionDTO exportQuantitiesSelectionDTO);
   }
}