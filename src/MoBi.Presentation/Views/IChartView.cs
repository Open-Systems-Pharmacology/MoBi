using System.Windows.Forms;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IChartView : IView<IChartPresenter>
   {
      void SetChartView(Control control);
   }
}