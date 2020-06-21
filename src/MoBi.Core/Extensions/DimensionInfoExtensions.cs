using OSPSuite.FuncParser;
namespace MoBi.Core.Extensions
{

   public static class DimensionInfoExtensions
   {
      public static bool AreEquals(this DimensionInformation left, DimensionInformation right)
      {
         return (left.LengthExponent == right.LengthExponent
                 && left.MassExponent == right.MassExponent
                 && left.TimeExponent == right.TimeExponent
                 && left.ElectricCurrentExponent == right.ElectricCurrentExponent
                 && left.TemperatureExponent == right.TemperatureExponent
                 && left.AmountExponent == right.AmountExponent
                 && left.LuminousIntensityExponent == right.LuminousIntensityExponent);
      }
   }
}