using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditQuantityInSimulationView : IView<IEditQuantityInSimulationPresenter>
   {
      void BindTo(QuantityDTO quantityDTO);
      bool AllowValueChange { get; set; }
      string SetInitialValueLabel { set; }
      void SetFormulaView(IView view);
      void SetParametersView(IView view);
      void HideParametersView();
      void SetWarning(string message);
      void ClearWarning();
      void EnableResetButton(bool enable);
      void ShowParameters();
      void SetQuantityInfoView(IView view);
   }
}