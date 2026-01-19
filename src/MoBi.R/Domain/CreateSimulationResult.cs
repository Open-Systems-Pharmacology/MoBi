using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.R.Domain;

namespace MoBi.R.Domain
{
   public sealed class CreateSimulationResult
   {
      public Simulation Simulation { get; }
      public string[] Warnings { get; }
      public string[] Errors { get; }

      public CreateSimulationResult(Simulation simulation, IEnumerable<string> warnings = null, IEnumerable<string> errors = null)
      {
         Simulation = simulation;
         Warnings = warnings?.ToArray() ?? Array.Empty<string>();
         Errors = errors?.ToArray() ?? Array.Empty<string>();
      }
   }
}