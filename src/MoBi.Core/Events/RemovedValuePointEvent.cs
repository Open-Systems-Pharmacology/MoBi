using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Events
{
   public class RemovedValuePointEvent : TableFormulaEvent
   {
      public ValuePoint ValuePoint { get; private set; }
      public RemovedValuePointEvent(TableFormula tableFormula, ValuePoint valuePoint) : base(tableFormula)
      {
         ValuePoint = valuePoint;
      }
   }
}