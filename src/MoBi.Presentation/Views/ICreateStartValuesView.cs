
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateStartValuesView : IModalView<ICreateStartValuesPresenter>
   {
      void Show(StartValuesDTO dto);
   }
}