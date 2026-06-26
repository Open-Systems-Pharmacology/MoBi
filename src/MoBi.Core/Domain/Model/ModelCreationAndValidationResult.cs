using OSPSuite.Core.Domain;
using SimulationConfiguration = OSPSuite.Core.Domain.Builder.SimulationConfiguration;

namespace MoBi.Core.Domain.Model;

public class ModelCreationAndValidationResult
{
   public IMoBiSimulation Simulation { get; }
   public CreationResult CreationResult { get; set; }
   public SimulationConfiguration ClonedConfiguration { get; set; }

   public ModelCreationAndValidationResult(IMoBiSimulation simulation)
   {
      Simulation = simulation;
   }

   public bool IsValid => CreationResult != null && !CreationResult.IsInvalid;

   public bool WasConfigured => CreationResult != null || ClonedConfiguration != null;
}