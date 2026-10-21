using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Comparison;

namespace MoBi.Core.Domain.Comparison
{
   public class MoBiSimulationDiffBuilder : DiffBuilder<IMoBiSimulation>
   {
      private readonly IObjectComparer _objectComparer;

      public MoBiSimulationDiffBuilder(IObjectComparer objectComparer)
      {
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IMoBiSimulation> comparison)
      {
         _objectComparer.Compare(comparison.ChildComparison(x => x.Model));
         _objectComparer.Compare(comparison.ChildComparison(x => x.Configuration));
         compareValidationOptions(comparison);
      }

      private void compareValidationOptions(IComparison<IMoBiSimulation> comparison)
      {
         compareValidationOption(x => x.ValidationOptions.ShowPKSimObserverMessages, AppConstants.Captions.ValidatePKSimStandardObserver, comparison);
         compareValidationOption(x => x.ValidationOptions.ShowUnresolvedEndosomesWarningsForInitialConditions, AppConstants.Captions.ShowUnresolvedEndosomeWarningsForInitialConditions, comparison);
         compareValidationOption(x => x.ValidationOptions.WarnForNonFiniteQuantities, AppConstants.Captions.ShowWarningsForNonFiniteQuantities, comparison);
         compareValidationOption(x => x.ValidationOptions.CheckDimensions, AppConstants.Captions.ValidateDimensions, comparison);
         compareValidationOption(x => x.ValidationOptions.ShowCannotCalcErrors, AppConstants.Captions.ShowUnableCalculateWarnings, comparison);
         compareValidationOption(x => x.ValidationOptions.ShowPKSimDimensionProblemWarnings, AppConstants.Captions.ShowPKSimParameterWarnings, comparison);
         compareValidationOption(x => x.ValidationOptions.CheckRules, AppConstants.Captions.ValidateRules, comparison);
         compareValidationOption(x => x.ValidationOptions.CheckCircularReference, AppConstants.Captions.CircularReferenceCheck, comparison);
      }

      //An option that is not set uses the user setting, which is different from explicitly setting it
      private void compareValidationOption(Func<IMoBiSimulation, bool?> validationOption, string optionName, IComparison<IMoBiSimulation> comparison) =>
         CompareValues(validationOption, optionName, comparison, (option1, option2) => Equals(option1, option2), (simulation, option) => option?.ToString() ?? AppConstants.Captions.ValidationOptionFromUserSettings);
   }
}
