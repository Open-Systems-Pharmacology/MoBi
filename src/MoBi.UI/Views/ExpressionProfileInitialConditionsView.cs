using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public class ExpressionProfileInitialConditionsView : BaseInitialConditionsView<IExpressionProfileInitialConditionsPresenter>, IExpressionProfileInitialConditionsView
   {
      public ExpressionProfileInitialConditionsView(ValueOriginBinder<InitialConditionDTO> valueOriginBinder) : base(valueOriginBinder)
      {
      }

      public override IExpressionProfileInitialConditionsPresenter InitialConditionPresenter => _presenter.DowncastTo<IExpressionProfileInitialConditionsPresenter>();
      public void AttachPresenter(IExpressionProfileInitialConditionsPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}