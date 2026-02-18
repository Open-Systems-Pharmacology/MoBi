using OSPSuite.Core.Domain;

namespace MoBi.Core.Services;

public class InitialConditionPropertiesForMerge
{
   public ObjectPath ObjectPath { set; get; }
   public string DimensionName { set; get; }
   public double ValueInBaseUnit { set; get; }
   public double ScaleDivisor { set; get; }
   public bool IsPresent { set; get; }
   public bool NegativeAllowed { set; get; }
}