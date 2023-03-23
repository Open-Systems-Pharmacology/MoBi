using OSPSuite.Assets;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditSimulationConfigurationView : IView<IEditSimulationConfigurationPresenter>
   {
      void AddSelectionView(IResizableView view, string caption, ApplicationIcon icon);
      void AddEmptyPlaceHolder();
   }
}