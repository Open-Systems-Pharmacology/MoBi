using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Events
{
   public class TableFormulaRestartSolverChangedEvent : TableFormulaEvent
   {
      public ValuePoint ValuePoint { set; get; }

      public TableFormulaRestartSolverChangedEvent(TableFormula tableFormula, ValuePoint point)
         : base(tableFormula)
      {
         ValuePoint = point;
      }
   }
}