using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditNeighborhoodBuilderView : IView<IEditNeighborhoodBuilderPresenter>, IActivatableView, IEditViewWithParameters
   {
      void AddTagsView(IView view);
      void BindTo(NeighborhoodBuilderDTO neighborhoodBuilderDTO);
   }
}