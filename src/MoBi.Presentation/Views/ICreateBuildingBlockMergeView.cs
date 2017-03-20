using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateBuildingBlockMergeView : IModalView<ICreateBuildingBlockMergePresenter>
   {
      void AddView(IView view);
   }
}