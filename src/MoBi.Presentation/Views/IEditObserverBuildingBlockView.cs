using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditObserverBuildingBlockView : IView<IEditObserverBuildingBlockPresenter>, IEditBuildingBlockBaseView
   {
      void SetAmountObserverView(IView view);
      void SetContainerObserverView(IView view);
      void SetEditObserverBuilderView(IView view);
      ObserverType ObserverType { get; }
   }

   public enum ObserverType
   {
      AmountObserver,
      ContainerObserver,
      NeighborhoodObserver
   }
}