using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Events
{
   public class TableFormulaUnitChangedEvent : TableFormulaEvent
   {
      public TableFormulaUnitChangedEvent(TableFormula tableFormula) : base(tableFormula)
      {
      }
   }
}