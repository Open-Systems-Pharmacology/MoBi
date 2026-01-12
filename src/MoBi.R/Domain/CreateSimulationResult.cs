using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.R.Domain
{
   public sealed class CreateSimulationResult
   {
      public IMoBiSimulation Simulation { get; }
      public IEnumerable<string> Warnings { get; }
      public IEnumerable<string> Errors { get; }

      public CreateSimulationResult(IMoBiSimulation simulation, IEnumerable<string> warnings = null, IEnumerable<string> errors = null)
      {
         Simulation = simulation;
         Warnings = warnings;
         Errors = errors;
      }
   }
}