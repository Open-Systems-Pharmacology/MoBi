using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateSimulationConfigurationView : IWizardView, IModalView<ICreateSimulationConfigurationPresenter>
   {
      void BindTo(ObjectBaseDTO simulationDTO);
      void DisableNaming();
   }
}