using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateSimulationView : IWizardView, IModalView<ICreateSimulationPresenter>
   {
      void BindTo(IObjectBaseDTO simulationDTO);
   }
}