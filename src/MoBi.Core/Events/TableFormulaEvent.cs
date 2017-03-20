using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Events
{
   public abstract class TableFormulaEvent
   {
      public TableFormula TableFormula { get; private set; }

      protected TableFormulaEvent(TableFormula tableFormula)
      {
         TableFormula = tableFormula;
      }
   }
}