using System;
using MoBi.Assets;
using MoBi.Core;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class CoreUserSettings : ICoreUserSettings
   {
      public bool CheckDimensions { get; set; }
      public bool ShowPKSimDimensionProblemWarnings { get; set; }
      public bool ShowCannotCalcErrors { get; set; }
      public bool CheckRules { get; set; }

      public int MaximumNumberOfCoresToUse { get; set; } = Math.Max(Environment.ProcessorCount - 1, 1);
      public int NumberOfBins { get; set; } = AppConstants.DEFAULT_NUMBER_OF_BINS;
      public int NumberOfIndividualsPerBin { get; set; } = AppConstants.DEFAULT_NUMBER_OF_INDIVIDUALS_PER_BIN;
      public bool WarnForNonFiniteQuantities { get; set; }
   }
}
