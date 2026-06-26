using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views;

public interface ISimulationUpdateView : IModalView<ISimulationUpdatePresenter>
{
   void BindTo(IEnumerable<SimulationUpdateStatusDTO> dtos);
   void RefreshData();

   void ShowCompleted();
}