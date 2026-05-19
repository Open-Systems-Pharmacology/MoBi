using MoBi.Core;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class CoreUserSettings : ICoreUserSettings
   {
      public bool CheckDimensions { get; set; }
      public bool ShowPKSimDimensionProblemWarnings { get; set; }
      public bool ShowCannotCalcErrors { get; set; }
      public bool CheckRules { get; set; }

      public int MaximumNumberOfCoresToUse { get; set; }
      public int NumberOfBins { get; set; }
      public int NumberOfIndividualsPerBin { get; set; }
      public bool WarnForNonFiniteQuantities { get; set; }
   }
}