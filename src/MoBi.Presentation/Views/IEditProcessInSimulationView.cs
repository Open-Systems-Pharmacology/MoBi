using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditProcessInSimulationView<TPresenter> : IView<TPresenter>
      where TPresenter : IPresenter
   {
      void SetParameterView(IView view);
      void SetFormulaView(IView view);
      void ShowParameters();
   }
}
