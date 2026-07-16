namespace MoBi.Core.Snapshots;

public class UpdatedParameterValue
{
   public string Path { get; set; }
   public double? NewValue { get; set; }
   public string NewUnit { get; set; }
   public string NewFormulaId { get; set; }
}