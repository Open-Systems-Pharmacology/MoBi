using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
    public interface IEditExpressionProfileBuildingBlockView : IView<IEditExpressionProfileBuildingBlockPresenter>, IEditBuildingBlockBaseView
    {
        void AddExpressionProfileView(IView baseView);
    }
}