using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.R.Domain
{
   public class SimulationRequest
   {
      private readonly List<ModuleConfiguration> _moduleConfigurations = new();
      private readonly List<ExpressionProfileBuildingBlock> _expressionProfiles = new();
      private readonly Cache<(string moleculeName, string category), string> _moleculeCalculationMethodOverrides = new();

      public SimulationSettings SimulationSettings { get; set; }
      public bool CreateAllProcessRateParameters { get; set; }
      public IndividualBuildingBlock Individual { get; private set; }
      public IReadOnlyList<ModuleConfiguration> ModuleConfigurations => _moduleConfigurations;
      public IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles => _expressionProfiles;

      /// <summary>
      ///    Returns all molecule calculation method overrides that have been added to this request.
      /// </summary>
      public IReadOnlyList<MoleculeCalculationMethodOverride> AllCalculationMethodOverrides()
      {
         return _moleculeCalculationMethodOverrides.KeyValues
            .GroupBy(keyValue => keyValue.Key.moleculeName)
            .Select(g => moleculeCalculationMethodOverrideForMolecule(g.Key, g.All().Select(kv => (kv.Key.category, kv.Value)))).ToList();
      }

      private static MoleculeCalculationMethodOverride moleculeCalculationMethodOverrideForMolecule(string moleculeName, IEnumerable<(string category, string name)> allOverrides)
      {
         var moleculeOverride = new MoleculeCalculationMethodOverride(moleculeName);
         allOverrides.Each(x => moleculeOverride.AddUsedCalculationMethod(new UsedCalculationMethod(x.category, x.name)));
         return moleculeOverride;
      }

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
      ///    Adds or overrides the used calculation method for a molecule in a given category.
      /// </summary>
      /// <param name="moleculeName">The name of the molecule.</param>
      /// <param name="category">The calculation method category.</param>
      /// <param name="calculationMethodName">The name of the calculation method to use.</param>
      public void AddMoleculeUsedCalculationMethod(string moleculeName, string category, string calculationMethodName)
      {
         _moleculeCalculationMethodOverrides[(moleculeName, category)] = calculationMethodName;
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