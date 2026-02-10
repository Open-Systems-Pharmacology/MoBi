using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Domain
{
   public class SimulationRequest
   {
      private readonly List<ModuleConfiguration> _moduleConfigurations = new();
      private readonly List<ExpressionProfileBuildingBlock> _expressionProfiles = new();

      public SimulationSettings SimulationSettings { get; set; }
      public bool CreateAllProcessRateParameters { get; set; }
      public IndividualBuildingBlock Individual { get; private set; }
      public IReadOnlyList<ModuleConfiguration> ModuleConfigurations => _moduleConfigurations;
      public IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles => _expressionProfiles;

      /// <summary>
      ///    Adds a module configuration to this simulation request.
      /// </summary>
      /// <param name="moduleConfiguration">The module configuration to add to the request.</param>
      public void AddModuleConfiguration(ModuleConfiguration moduleConfiguration)
      {
         if (moduleConfiguration != null)
            _moduleConfigurations.Add(moduleConfiguration);
      }

      /// <summary>
      ///    Adds an expression profile to this simulation request.
      /// </summary>
      /// <param name="expressionProfile">The expression profile building block to add to the request.</param>
      public void AddExpressionProfile(ExpressionProfileBuildingBlock expressionProfile)
      {
         if (expressionProfile != null)
            _expressionProfiles.Add(expressionProfile);
      }

      /// <summary>
      ///    Sets the individual building block for this simulation request.
      /// </summary>
      /// <param name="individual">The individual building block to associate with the request.</param>
      public void SetIndividual(IndividualBuildingBlock individual)
      {
         Individual = individual;
      }
   }
}