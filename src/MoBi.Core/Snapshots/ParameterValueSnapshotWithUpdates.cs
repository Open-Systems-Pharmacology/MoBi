namespace MoBi.Core.Snapshots;

public abstract class ParameterValueSnapshotWithUpdates
{
   public object PKSimSnapshot { get; set; }
   public UpdatedParameterValue[] UpdatedValues { get; set; }
   public string FormulaCache { get; set; }
}

public class ExpressionProfileSnapshot : ParameterValueSnapshotWithUpdates;

public class IndividualSnapshot : ParameterValueSnapshotWithUpdates;