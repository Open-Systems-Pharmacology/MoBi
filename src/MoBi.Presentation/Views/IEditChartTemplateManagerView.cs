using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditChartTemplateManagerView : IView<IEditChartTemplateManagerPresenter>
   {
      void SetSubView(IView baseView);
   }
}