using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISimulationChangesView : IView<ISimulationChangesPresenter>
   {
      void BindTo(IReadOnlyList<OriginalQuantityValueDTO> originalQuantityValues);
   }
}
