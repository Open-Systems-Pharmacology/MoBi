using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditIndividualParameterView : IView<IEditIndividualParameterPresenter>
   {
      void BindTo(IndividualParameterDTO individualParameterDTO);
      void AddValueOriginView(IView view);
      void AddFormulaView(IView view);
      void HideFormulaEdit();
      void ShowFormulaEdit();
      void ShowWarningFor(string buildingBlockName);
   }
}