using MoBi.R.Domain;
using OSPSuite.R.Domain;

namespace MoBi.R.Services
{
   public interface ISimulationCreateTask
   {
      Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration);
   }

   public class SimulationCreateTask : ISimulationCreateTask
   {
      private readonly ISimulationFactory _simulationFactory;

      public SimulationCreateTask(ISimulationFactory simulationFactory)
      {
         _simulationFactory = simulationFactory;
      }

      public Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration) =>
         _simulationFactory.CreateSimulation(simulationConfiguration);
   }
}