using OSPSuite.FuncParser;

namespace MoBi.Engine.Extensions
{

   public static class DimensionInfoExtensions
   {
      public static bool AreEquals(this IDimensionInfo left, IDimensionInfo right)
      {
         return (left.Length == right.Length
                 && left.Mass == right.Mass
                 && left.Time == right.Time
                 && left.ElectricCurrent == right.ElectricCurrent
                 && left.Temperature == right.Temperature
                 && left.Amount == right.Amount
                 && left.LuminousIntensity == right.LuminousIntensity);
      }
   }
}