using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.R.Domain
{
   public sealed class CreateSimulationResult
   {
      public IMoBiSimulation Simulation { get; }
      public string[] Warnings { get; }
      public string[] Errors { get; }

      public CreateSimulationResult(IMoBiSimulation simulation, IEnumerable<string> warnings = null, IEnumerable<string> errors = null)
      {
         Simulation = simulation;
         Warnings = warnings?.ToArray() ?? Array.Empty<string>();
         Errors = errors?.ToArray() ?? Array.Empty<string>();
      }
   }
}