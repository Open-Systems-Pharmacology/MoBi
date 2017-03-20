using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;

namespace MoBi.Core.Domain.Extensions
{
   public static class CurveExtensions
   {
      public static bool IsObserved(this ICurve curve)
      {
         return curve.yData.DataInfo.Origin.Equals(ColumnOrigins.Observation)
                || curve.yData.DataInfo.Origin.Equals(ColumnOrigins.ObservationAuxiliary)
                || curve.xData.DataInfo.Origin.Equals(ColumnOrigins.Observation)
                || curve.xData.DataInfo.Origin.Equals(ColumnOrigins.ObservationAuxiliary);
      }

   }
}