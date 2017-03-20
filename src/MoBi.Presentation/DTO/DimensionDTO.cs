using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class DimensionDTO
   {
      public DimensionDTO()
      {
         Units = new List<Unit>();
      }

      public double Length { get; set; }
      public double Mass { get; set; }
      public double Time { get; set; }
      public double ElectricCurrent { get; set; }
      public double Temperature { get; set; }
      public double Amount { get; set; }
      public double LuminousIntensity { get; set; }
      public string Name { get; set; }
      public string BaseUnit { get; set; }
      public IList<Unit> Units { get; set; }
   }
}