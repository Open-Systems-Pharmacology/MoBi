using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Model
{
   public class SimulationAndValidationResult
   {
      public IMoBiSimulation Simulation { get; }
      public ValidationResult ValidationResult { get; }

      public SimulationAndValidationResult(IMoBiSimulation simulation, ValidationResult validationResult)
      {
         Simulation = simulation;
         ValidationResult = validationResult;
      }
      public void Deconstruct(out IMoBiSimulation simulation, out ValidationResult validationResult)
      {
         simulation = Simulation;
         validationResult = ValidationResult;
      }
   }
}