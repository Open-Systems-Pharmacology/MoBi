using MoBi.Core.Domain.Model;
using MoBi.R.Domain;
using OSPSuite.R.Domain;

namespace MoBi.R.Services
{
   public interface ISimulationCreateTask
   {
      Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration, MoBiProject moBiProject);
   }

   public class SimulationCreateTask : ISimulationCreateTask
   {
      private readonly ISimulationFactory _simulationFactory;

      public SimulationCreateTask(ISimulationFactory simulationFactory)
      {
         _simulationFactory = simulationFactory;
      }

      public Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration, MoBiProject moBiProject) =>
         _simulationFactory.CreateSimulation(simulationConfiguration, moBiProject);
   }
}