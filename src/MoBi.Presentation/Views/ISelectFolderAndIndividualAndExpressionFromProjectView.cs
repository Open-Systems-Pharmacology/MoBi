using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectFolderAndIndividualAndExpressionFromProjectView : IModalView, IView<ISelectFolderAndIndividualAndExpressionFromProjectPresenter>
   {
      void BindTo(IndividualExpressionAndFilePathDTO dto);
   }
}