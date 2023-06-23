using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditIndividualBuildingBlockView : IView<IEditIndividualBuildingBlockPresenter>, IEditBuildingBlockBaseView
   {
      void AddIndividualView(IView baseView);
   }
}