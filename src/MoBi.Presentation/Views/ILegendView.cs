using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ILegendView : IView<ILegendPresenter>
   {
      void AddLegendItem(LegendItemDTO legendItem);
   }
}
