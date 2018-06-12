using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditTableFormulaWithOffsetFormulaView : IView<IEditTableFormulaWithOffsetFormulaPresenter>, IEditTypedFormulaView
   {
      void BindTo(TableFormulaWithOffsetDTO tableFormulaWithOffsetDTO);
   }
}