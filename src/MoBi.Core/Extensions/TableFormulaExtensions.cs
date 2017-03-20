using System.Linq;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Extensions
{
   public static class TableFormulaExtensions
   {
      public static ValuePoint GetPointWithCoordinates(this TableFormula formula, double x, double y)
      {
         return formula.AllPoints().Single(point => ValueComparer.AreValuesEqual(point.X, x) && ValueComparer.AreValuesEqual(point.Y, y));
      }
   }
}
