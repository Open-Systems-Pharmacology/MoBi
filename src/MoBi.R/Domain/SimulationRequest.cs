using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Domain
{
   public class SimulationRequest
   {
      private readonly List<ModuleConfiguration> _moduleConfigurations = new();
      private readonly List<ExpressionProfileBuildingBlock> _expressionProfiles = new();

      public IndividualBuildingBlock Individual { get; private set; }
      public IReadOnlyList<ModuleConfiguration> ModuleConfigurations => _moduleConfigurations;
      public IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles => _expressionProfiles;

      public void AddModuleConfiguration(ModuleConfiguration moduleConfiguration)
      {
         if (moduleConfiguration != null)
            _moduleConfigurations.Add(moduleConfiguration);
      }

      public void AddExpressionProfile(ExpressionProfileBuildingBlock expressionProfile)
      {
         if (expressionProfile != null)
            _expressionProfiles.Add(expressionProfile);
      }

      public void SetIndividual(IndividualBuildingBlock individual)
      {
         Individual = individual;
      }
   }
}