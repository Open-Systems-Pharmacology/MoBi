using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface INewFormulaView : IModalView<INewFormulaPresenter>
   {
      void BindTo(ObjectBaseDTO dto);
      void AddReferenceView(IView view);
      void AddFormulaView(IView view);
   }
}