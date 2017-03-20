using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IObserverListView : IView<IObserverBuilderListPresenter>
   {
      void Show(IEnumerable<ObserverBuilderDTO> dtoObserverBuilders);
   }
}