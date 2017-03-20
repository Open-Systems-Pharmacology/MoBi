using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditMoleculeBuilderView :  IView<IEditMoleculeBuilderPresenter>, IActivatableView,IViewWithFormula
   {
      void SetParametersView(IView subView);
      void Show(MoleculeBuilderDTO moleculeBuilder);
      void ShowParameters();
      void UpdateStartAmountDisplay(string amoutOrConcentrationText);
   }
}