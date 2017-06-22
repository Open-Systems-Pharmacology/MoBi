using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class ChartView : BaseUserControl, IChartView
   {
      private IChartPresenter _presenter;

      public void AttachPresenter(IChartPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetChartView(IView control)
      {
         this.FillWith(control);
      }
   }
}