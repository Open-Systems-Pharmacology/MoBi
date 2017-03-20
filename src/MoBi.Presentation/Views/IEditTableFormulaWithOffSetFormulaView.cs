using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditTableFormulaWithOffSetFormulaView : IView<IEditTableFormulaWithOffSetFormulaPresenter>, IEditTypedFormulaView
   {
      void Show(TableFormulaWithOffsetDTO dtoTableFormulaWithOffset);
      void ShowOffsetObjectPath(FormulaUsablePathDTO dtoFormulaUsablePath);
      void ShowTableObjectPath(FormulaUsablePathDTO dtoFormulaUsablePath);
   }
}