using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditObserverBuilderView : IView<IEditObserverBuilderPresenter>, IViewWithFormula, IActivatableView
   {
      void BindTo(ObserverBuilderDTO observerBuilderDTO);
      bool FormulaHasError { set; }
      void AddMoleculeListView(IView view);
      void AddDescriptorConditionListView(IView view);
   }
}