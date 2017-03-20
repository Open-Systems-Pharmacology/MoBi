using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditDimensionView : IView<IEditDimensionPresenter>,  IActivatableView
   {
      void BindToSource(DimensionDTO dto);
   }
}