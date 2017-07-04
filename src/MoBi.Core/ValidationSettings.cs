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
   }
}