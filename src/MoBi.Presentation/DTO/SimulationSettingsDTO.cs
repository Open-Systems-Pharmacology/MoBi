using MoBi.Core.Domain.Model;

namespace MoBi.Presentation.DTO
{
   public class SimulationSettingsDTO : ObjectBaseDTO
   {
      public IMoBiSimulation Simulation { get; set; }

      public SimulationSettingsDTO(IMoBiSimulation simulation) : base(simulation.Settings)
      {
         Simulation = simulation;
      }
   }
}