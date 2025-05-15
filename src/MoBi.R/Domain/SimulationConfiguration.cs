using System.Collections.Generic;
using MoBi.R.Services;

namespace MoBi.R.Domain
{
   public class SimulationConfiguration
   {
      public string IndividualName { get; set; }
      public List<ModuleConfiguration> ModuleConfigurations { get; set; } = new List<ModuleConfiguration>();
      public List<string> ExpressionProfileNames { get; set; } = new List<string>();
      public string SimulationName { get; set; }
   }
}