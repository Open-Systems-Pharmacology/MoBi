using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IProjectChartView : IMdiChildView<IProjectChartPresenter>
   {
      void AddView(IView view);
   }
}