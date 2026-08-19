namespace MoBi.Core.Snapshots;

public abstract class ParameterValueSnapshotWithUpdates
{
   /// <summary>
   ///    The name of the building block in the MoBi project. The <see cref="PKSimSnapshot" /> carries the PK-Sim name, but
   ///    the MoBi building block may have been renamed on import to avoid a collision with an existing building block (for
   ///    example when importing two simulations that share the same individual). Preserving the MoBi name here keeps the
   ///    round-trip unique so simulations can still resolve their building blocks after the snapshot is loaded.
   /// </summary>
   public string MoBiName { get; set; }
   public object PKSimSnapshot { get; set; }
   public UpdatedParameterValue[] UpdatedValues { get; set; }
   public string FormulaCache { get; set; }
}

public class ExpressionProfileSnapshot : ParameterValueSnapshotWithUpdates;

public class IndividualSnapshot : ParameterValueSnapshotWithUpdates;