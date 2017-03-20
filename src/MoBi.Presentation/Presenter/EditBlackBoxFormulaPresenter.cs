using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IEditBlackBoxFormulaPresenter : IEditTypedFormulaPresenter
   {
   }

   public class EditBlackBoxFormulaPresenter : EditTypedFormulaPresenter<IEditBlackBoxFormulaView, IEditBlackBoxFormulaPresenter, BlackBoxFormula>, IEditBlackBoxFormulaPresenter
   {
      public EditBlackBoxFormulaPresenter(IEditBlackBoxFormulaView view, IDisplayUnitRetriever displayUnitRetriever) : base(view, displayUnitRetriever)
      {
      }

      public override void Edit(BlackBoxFormula objectToEdit)
      {
         /*nothing to do for now*/
      }
   }
}