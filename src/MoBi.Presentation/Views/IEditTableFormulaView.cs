using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditTableFormulaView : IView<IEditTableFormulaPresenter>, IEditTypedFormulaView
   {
      void AddTableView(IView tableView);
      void AddChartView(IView chartView);
   }
}