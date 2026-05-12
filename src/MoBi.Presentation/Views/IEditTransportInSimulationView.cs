using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.Views
{
   public interface IEditTransportInSimulationView : IEditProcessInSimulationView<IEditTransportInSimulationPresenter>
   {
      void BindTo(TransportDTO transportDTO);
   }
}
