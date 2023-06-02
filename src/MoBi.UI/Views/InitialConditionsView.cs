using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public class InitialConditionsView : BaseInitialConditionsView<IInitialConditionsPresenter>, IInitialConditionsView
   {
      public InitialConditionsView(ValueOriginBinder<InitialConditionDTO> valueOriginBinder) : base(valueOriginBinder)
      {
      }

      public override IInitialConditionsPresenter InitialConditionPresenter => _presenter.DowncastTo<IInitialConditionsPresenter>();
      public void AttachPresenter(IInitialConditionsPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}