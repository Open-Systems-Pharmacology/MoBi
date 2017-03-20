using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Events
{
   public class TableFormulaValueChangedEvent : TableFormulaEvent
   {
      public ValuePoint ValuePoint { set; get; }

      public TableFormulaValueChangedEvent(TableFormula tableFormula, ValuePoint point)
         : base(tableFormula)
      {
         ValuePoint = point;
      }
   }
}