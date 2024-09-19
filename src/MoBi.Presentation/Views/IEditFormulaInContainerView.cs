
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IFormulaEditView
   {
      void SetEditFormulaInstanceView(IView subView);
      void ClearFormulaView();
      void BindTo(FormulaInfoDTO dtoFormulaInfo);
      bool IsComplexFormulaView { set; }
      bool IsNamedFormulaView { set; }
      void SetReferenceView(ISelectReferenceView view);
   }

   public interface IEditFormulaInContainerView : IView<IEditFormulaInContainerPresenter>, IFormulaEditView
   {

   }

   public interface IEditFormulaInPathAndValuesView : IView<IEditFormulaInPathAndValuesPresenter>, IFormulaEditView
   {

   }
}