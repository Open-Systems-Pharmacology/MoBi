using MoBi.Core.Domain.Model;

namespace MoBi.Presentation.DTO
{
   public class SimulationDTO : ObjectBaseDTO
   {
      private readonly IMoBiSimulation _simulation;

      public SimulationDTO(IMoBiSimulation simulation) : base(simulation)
      {
         _simulation = simulation;
      }

      public bool CreateAllProcessRateParameters
      {
         get => _simulation.Configuration.CreateAllProcessRateParameters;
         set => _simulation.Configuration.CreateAllProcessRateParameters = value;
      }
   }
}
