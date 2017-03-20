using System.Windows.Forms;
using OSPSuite.UI.Extensions;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class ChartView : BaseUserControl, IChartView
   {
      private IChartPresenter _presenter;

      public void AttachPresenter(IChartPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetChartView(Control control)
      {
         this.FillWith(control);
      }
   }
}