using OSPSuite.Utility.Reflection;

namespace MoBi.Core
{
   public class ValidationSettings : Notifier
   {
      private bool _checkDimensions;
      private bool _showPKSimDimensionProblemWarnings;
      private bool _showCannotCalcErrors;
      private bool _showPKSimObserverMessages;
      private bool _checkRules;
      private bool _checkCircularReference;
      private bool _showUnresolvedEndosomesWarningsForInitialConditions;

      public bool CheckDimensions
      {
         get => _checkDimensions;
         set
         {
            if (SetProperty(ref _checkDimensions, value) && !value)
            {
               ShowPKSimDimensionProblemWarnings = false;
               ShowCannotCalcErrors = false;
            }
         }
      }

      public bool ShowPKSimDimensionProblemWarnings
      {
         get => _showPKSimDimensionProblemWarnings;
         set => SetProperty(ref _showPKSimDimensionProblemWarnings, value);
      }

      public bool ShowCannotCalcErrors
      {
         get => _showCannotCalcErrors;
         set => SetProperty(ref _showCannotCalcErrors, value);
      }

      public bool ShowPKSimObserverMessages
      {
         get => _showPKSimObserverMessages;
         set => SetProperty(ref _showPKSimObserverMessages, value);
      }

      public bool CheckRules
      {
         get => _checkRules;
         set => SetProperty(ref _checkRules, value);
      }

      public bool CheckCircularReference
      {
         get => _checkCircularReference;
         set => SetProperty(ref _checkCircularReference, value);
      }

      public bool ShowUnresolvedEndosomesWarningsForInitialConditions
      {
         get => _showUnresolvedEndosomesWarningsForInitialConditions;
         set => SetProperty(ref _showUnresolvedEndosomesWarningsForInitialConditions, value);
      }

      public ValidationSettings Clone()
      {
         var clone = new ValidationSettings();
         clone.UpdatePropertiesFrom(this);
         return clone;
      }

      public void UpdatePropertiesFrom(ValidationSettings source)
      {
         CheckDimensions = source.CheckDimensions;
         ShowPKSimDimensionProblemWarnings = source.ShowPKSimDimensionProblemWarnings;
         ShowCannotCalcErrors = source.ShowCannotCalcErrors;
         ShowPKSimObserverMessages = source.ShowPKSimObserverMessages;
         CheckRules = source.CheckRules;
         CheckCircularReference = source.CheckCircularReference;
         ShowUnresolvedEndosomesWarningsForInitialConditions = source.ShowUnresolvedEndosomesWarningsForInitialConditions;
      }
   }
}