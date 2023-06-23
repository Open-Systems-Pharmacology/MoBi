using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateNeighborhoodBuilderView : IView<ICreateNeighborhoodBuilderPresenter>,  IActivatableView
   {
      void AddFirstNeighborView(IView view);
      void AddSecondNeighborView(IView view);
      void BindTo(ObjectBaseDTO objectBaseDTO);
   }
}