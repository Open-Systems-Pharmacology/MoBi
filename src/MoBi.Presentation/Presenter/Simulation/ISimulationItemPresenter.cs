using MoBi.Core.Domain.Model;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter.Simulation
{
   public interface ISimulationItemPresenter : ISubPresenter
   {
      void Edit(IMoBiSimulation simulation);
   }

   public interface ISimulationConfigurationItemPresenter : ISimulationItemPresenter
   {
   
   }
}