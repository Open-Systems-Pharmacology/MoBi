using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditSumFormulaView : IView<IEditSumFormulaPresenter>, IEditTypedFormulaView
   {
      void Show(SumFormulaDTO sumFormulaDTO);
      void AddDescriptorConditionListView(IView view);
      void AddFormulaPathListView(IView view);
      void SetValidationMessage(string message);
   }
}