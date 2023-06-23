using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IIndividualBuildingBlockView : IView<IIndividualBuildingBlockPresenter>
   {
      void BindTo(IndividualBuildingBlockDTO individualBuildingBlockDTO);
   }
}