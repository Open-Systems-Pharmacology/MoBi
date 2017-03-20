using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Events
{
   public class AddedValuePointEvent : TableFormulaEvent
   {
      public ValuePoint ValuePoint { get; private set; }
      public AddedValuePointEvent(TableFormula tableFormula, ValuePoint valuePoint) : base(tableFormula)
      {
         ValuePoint = valuePoint;
      }
   }
}