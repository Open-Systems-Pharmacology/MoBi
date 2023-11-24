using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IWithDistributedPathAndValueGridView
   {
      void AddDistributedParameterView(IView view);
   }

   public interface IIndividualBuildingBlockView : IView<IIndividualBuildingBlockPresenter>, IWithDistributedPathAndValueGridView
   {
      void BindTo(IndividualBuildingBlockDTO individualBuildingBlockDTO);
   }
}