using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditTableFormulaView : IView<IEditTableFormulaPresenter>, IEditTypedFormulaView
   {
      void Show(TableFormulaBuilderDTO dtoTableFormulaBuilder);
   }

   public interface INewValuePointView : IModalView<INewValuePointPresenter>
   {
      void BindTo(ValuePointDTO valuePointDTO);
   }
}