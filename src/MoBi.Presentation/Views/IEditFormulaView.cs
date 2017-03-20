
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditFormulaView : IView<IEditFormulaPresenter>
   {
      void SetEditFormualInstanceView(IView subView);
      void ClearFormulaView();
      void BindTo(FormulaInfoDTO dtoFormulaInfo);
      bool IsComplexFormulaView { set; }
      bool IsNamedFormulaView { set; }
      void SetReferenceView(ISelectReferenceView view);
   }
}