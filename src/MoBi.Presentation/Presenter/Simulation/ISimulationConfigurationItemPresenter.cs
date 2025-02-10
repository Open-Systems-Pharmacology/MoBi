using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter.Simulation
{
   public interface ISimulationConfigurationItemPresenter : ISubPresenter
   {
      void Edit(SimulationConfiguration simulationConfiguration);
   }
}