namespace MoBi.Core.Snapshots;

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
}
