namespace MoBi.Core.Domain.Model
{
   /// <summary>
   ///    Validation and warning options of a simulation, used whenever the simulation is created or rebuilt and when it is
   ///    run. An option that is not set falls back to the corresponding user setting.
   /// </summary>
   public class SimulationValidationOptions
   {
      public bool? CheckDimensions { get; set; }
      public bool? ShowPKSimDimensionProblemWarnings { get; set; }
      public bool? ShowCannotCalcErrors { get; set; }
      public bool? ShowPKSimObserverMessages { get; set; }
      public bool? CheckRules { get; set; }
      public bool? CheckCircularReference { get; set; }
      public bool? ShowUnresolvedEndosomesWarningsForInitialConditions { get; set; }
      public bool? WarnForNonFiniteQuantities { get; set; }

      /// <summary>
      ///    Creates options with all values set from <paramref name="validationSettings" /> and
      ///    <paramref name="warnForNonFiniteQuantities" />
      /// </summary>
      public static SimulationValidationOptions From(ValidationSettings validationSettings, bool warnForNonFiniteQuantities)
      {
         return new SimulationValidationOptions
         {
            CheckDimensions = validationSettings.CheckDimensions,
            ShowPKSimDimensionProblemWarnings = validationSettings.ShowPKSimDimensionProblemWarnings,
            ShowCannotCalcErrors = validationSettings.ShowCannotCalcErrors,
            ShowPKSimObserverMessages = validationSettings.ShowPKSimObserverMessages,
            CheckRules = validationSettings.CheckRules,
            CheckCircularReference = validationSettings.CheckCircularReference,
            ShowUnresolvedEndosomesWarningsForInitialConditions = validationSettings.ShowUnresolvedEndosomesWarningsForInitialConditions,
            WarnForNonFiniteQuantities = warnForNonFiniteQuantities
         };
      }

      /// <summary>
      ///    Returns the validation settings to use for the simulation: the options that are set, and the
      ///    <paramref name="userSettings" /> for the others
      /// </summary>
      public ValidationSettings ValidationSettingsWith(ICoreUserSettings userSettings)
      {
         //CheckDimensions must be set first since setting it to false also resets the dependent properties
         return new ValidationSettings
         {
            CheckDimensions = CheckDimensions ?? userSettings.CheckDimensions,
            ShowPKSimDimensionProblemWarnings = ShowPKSimDimensionProblemWarnings ?? userSettings.ShowPKSimDimensionProblemWarnings,
            ShowCannotCalcErrors = ShowCannotCalcErrors ?? userSettings.ShowCannotCalcErrors,
            ShowPKSimObserverMessages = ShowPKSimObserverMessages ?? userSettings.ShowPKSimObserverMessages,
            CheckRules = CheckRules ?? userSettings.CheckRules,
            CheckCircularReference = CheckCircularReference ?? userSettings.CheckCircularReference,
            ShowUnresolvedEndosomesWarningsForInitialConditions = ShowUnresolvedEndosomesWarningsForInitialConditions ?? userSettings.ShowUnresolvedEndosomeMessagesForInitialConditions
         };
      }

      /// <summary>
      ///    Returns whether the simulation warns for non-finite quantities: the option if it is set, otherwise the
      ///    <paramref name="userSettings" />
      /// </summary>
      public bool WarnForNonFiniteQuantitiesWith(OSPSuite.Core.ICoreUserSettings userSettings) => WarnForNonFiniteQuantities ?? userSettings.WarnForNonFiniteQuantities;

      public SimulationValidationOptions Clone() => (SimulationValidationOptions)MemberwiseClone();
   }
}
