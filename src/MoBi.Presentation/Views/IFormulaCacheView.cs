using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IFormulaCacheView : IView<IFormulaCachePresenter>
   {
      void Show(IEnumerable<FormulaBuilderDTO> dtos);
      void SetEditView(IView subView);
      void Select(FormulaBuilderDTO dtoFormulaBuilder);
   }
}