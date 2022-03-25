using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditTransportBuilderView : IView<IEditTransportBuilderPresenter>, IViewWithFormula, IActivatableView
   {
      void Show(TransportBuilderDTO dto);
      void AddMoleculeSelectionView(IView view);
      void SetParameterView(IView view);
      void ShowParameters();
      bool FormulaHasError { set; }
      bool ShowMoleculeList { set; }
      void EnableDisablePlotProcessRateParameter(bool newValue);
      void AddSourceCriteriaView(IView view);
      void AddTargetCriteriaView(IView view);
   }

   
}